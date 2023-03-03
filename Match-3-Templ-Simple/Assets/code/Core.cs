using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Core : MonoBehaviour
{
    public enum SWAPSTATE { HIDDEN, FORWARD, BACKWARD }
    static int points = 0;
    static int addToPoints = 0;
    static GemSwapPair swapPair = null;
    public static SWAPSTATE swapState { get; set; }
    public static bool resolvingInProgress { get; set; }

    // Use this for initialization
    void Start ()
    {
        initGame();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
        //points = Random.Range(800, 9000);

        if (addToPoints > 0)
        {
            points += 10;
            GO.txtPoints.text = Util.numToDigits(points, 8);
            addToPoints-= 10;
            if (addToPoints < 0) addToPoints = 0;
        }

        resolvingInProgress = false;
        for (int y = 0; y < 6; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                var gs = GO.gems[x, y].GetComponent<GemScript>();
                if ((int)gs.getY() > (int)gs.sourcePos.y) resolvingInProgress = true;
            }
        }

        if (swapState == Core.SWAPSTATE.FORWARD || swapState == Core.SWAPSTATE.BACKWARD)
        {
            resolvingInProgress = true;
            if (!GO.gemSwitch0.reachedTarget()) GO.gemSwitch0.moveToTarget();
            if (!GO.gemSwitch1.reachedTarget()) GO.gemSwitch1.moveToTarget();

            if (GO.gemSwitch0.reachedTarget() && GO.gemSwitch1.reachedTarget()) // SWAP DONE
            {
				Debug.Log("Some swapping done.");

				if (swapState == Core.SWAPSTATE.FORWARD) 
				{
                    Debug.Log("Normalswapping done.");
                    realGemSwap();
                    var matches = Core.getMatches();
                    if (matches.Count < 1) // no matches from swap
                    {
                        Debug.Log("no matches from swap");
                        startSwap(swapPair.g2, swapPair.g1, SWAPSTATE.BACKWARD);
                        realGemSwap();
                    }
                    else // there are matches
                    {
                        swapState = SWAPSTATE.HIDDEN;
                        Debug.Log("Some matches from swap");
                        resolveMatches(matches);
                        showSwapPair();
                        killSwapPair(); // shows the matrix again
                        GO.gemSwitch0.hideIt();
                        GO.gemSwitch1.hideIt();
                    }
                }
				else if (swapState == Core.SWAPSTATE.BACKWARD)
                {
                    Debug.Log("Backswapping done.");
                    swapState = SWAPSTATE.HIDDEN;
                    showSwapPair();
                    killSwapPair(); // shows the matrix again
                    GO.gemSwitch0.hideIt();
                    GO.gemSwitch1.hideIt();
                }
			}
        }

    }

    public static void killSwapPair()
    {
        Debug.Log("killSwapPair");
        swapPair = null;
    }

    public static void showSwapPair()
    {
        if (swapPair == null) return;
        swapPair.g1.show();
        swapPair.g2.show();
    }

    public static void hideSwapPair()
    {
        if (swapPair == null) return;
        swapPair.g1.hide();
        swapPair.g2.hide();
    }

    public static void realGemSwap()
    {
        GemScript gs1 = swapPair.g1;
        GemScript gs2 = swapPair.g2;
        
        Sprite temp = gs1.myImg.sprite;
        gs1.myImg.sprite = gs2.myImg.sprite;
        gs2.myImg.sprite = temp;

        int tempType = gs1.type;
        gs1.type = gs2.type;
        gs2.type = tempType;
    }

    public static void startSwap(GemScript gs1, GemScript gs2, SWAPSTATE instate)
    {
        swapPair = new GemSwapPair{g1 = gs1, g2 = gs2};
        GO.gemSwitch0.startSwap(gs1, gs2);
        GO.gemSwitch1.startSwap(gs2, gs1);
        swapState = instate;
    }

    public static void resolveMatches(List<GemScript> matchIndicies)
    {
        resolvingInProgress = true;

        foreach (GemScript gs in matchIndicies)
        {
            gs.markedForKill = true;
            Core.addPoints(100);
        }
        if (matchIndicies.Count > 0) GO.audio_sound.Play();
    }

    public static void addPoints(int d)
    {
        addToPoints += d;
    }

    public static void resetPoints()
    {
        addToPoints = 0;
        points = 0;
        GO.txtPoints.text = Util.numToDigits(points, 8);
    }

    private static void randomizeMap()
    {
        int matches = 0;
        do
        {
            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    GO.gems[x, y].GetComponent<GemScript>().randomizeMe();
                }
            }

            matches = getMatches().Count;
        }
        while (matches > 0);
    }

    public static void initGame()
    {
        Screen.SetResolution(450,800,false); // for building, positions will be off

        MyResources.initCharacterSprites();
        GO.init();
        GO.gemSelector.enabled = false;
        swapState = SWAPSTATE.HIDDEN;
        for (int y = 0; y < 6; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                GO.gems[x, y].GetComponent<GemScript>().manualInit();
            }
        }
        randomizeMap();

        //var gemMatrix = GameObject.Find("GemMatrix");

        //UI.Image img = new UI.Image();
        /*
        Image img = gemMatrix.AddComponent<Image>();
        img.name = "newimg";
        img.setIUPos(0,0);
        Image refe = GameObject.Find("Image (5)").GetComponent<Image>();
        img.sprite = refe.sprite;
        */

    }

    public static bool gemIndexInBounds(int ix, int iy)
    {
        return (ix >= 0 && iy >= 0 && iy < GO.gems.height() && ix < GO.gems.width());
    }

    private static void addMatchesUp(List<GemScript> bigList, List<GemScript> curMatches)
    {
        if (curMatches.Count > 2)
        {
            foreach (GemScript l in curMatches)
            {
                bigList.Add(l);
            }
        }
        curMatches.Clear();
    }

    public static List<GemScript> getMatches()
    {
        var list = new List<GemScript>();

        //horizontal checks
        var horiMatches = new List<GemScript>();
        for (int y = 0; y < GO.gems.height(); y++)
        {
            horiMatches.Clear();
            for (int x = 0; x < GO.gems.width(); x++)
            {
                GemScript g = GO.gems[x, y].GetComponent<GemScript>();
                if (gemIndexInBounds(x+1,y)) // we are not at the last item
                {
                    var next = GO.gems[x + 1, y].GetComponent<GemScript>();
                    if (g.type == next.type)
                    {
                        horiMatches.Add(next);
                        horiMatches.Add(g);
                    }
                    else
                        addMatchesUp(list, horiMatches);
                }
            }
            addMatchesUp(list, horiMatches);
        }


        //vert checks
        var vertMatches = new List<GemScript>();
        for (int x = 0; x < GO.gems.width(); x++) 
        {
            vertMatches.Clear();
            for (int y = 0; y < GO.gems.height(); y++)
            {
                GemScript g = GO.gems[x, y].GetComponent<GemScript>();
                if (gemIndexInBounds(x, y + 1)) // we are not at the last item
                {
                    var next = GO.gems[x, y + 1].GetComponent<GemScript>();
                    if (g.type == next.type)
                    {
                        vertMatches.Add(next);
                        vertMatches.Add(g);
                    }
                    else
                        addMatchesUp(list, vertMatches);
                }
            }
            addMatchesUp(list, vertMatches);
        }



        return list;
    }
}

public class GemIndex
{
    public int x;
    public int y;
}

public class GemSwapPair
{
    public GemScript g1;
    public GemScript g2;
}