using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GemScript : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int type { get; set; }
    public int index_X { get; set; }
    public int index_Y { get; set; }
    public bool clickSelected { get; set; }
    public bool dragActive { get; set; }
    public Image myImg { get; set; }
    RectTransform myRect;
    public Vector2 sourcePos { get; set; }
    public bool markedForKill { get; set; }
    public bool initDone { get; set; }

    // Use this for initialization
    public void manualInit ()
    {
        myImg = this.GetComponent<Image>();
        myRect = myImg.rectTransform;
        sourcePos = new Vector2(getX(), getY());
        clickSelected = false;
        dragActive = false;
        markedForKill = false;
        randomizeMe(); //depends on resources being loaded
        initDone = true;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update ()
    {
        if (!initDone) return;

        if (!dragActive)
        {
            if ((int)getY() > (int)sourcePos.y)
            {
                myImg.translateIUPosY( -Speed.get(4.9f) );
                if ((int)getY() < (int)sourcePos.y){
                    myImg.setIUPosY(sourcePos.y);
                    var matches = Core.getMatches();
                    Core.resolveMatches(matches);
                }
            }
        }

        if (markedForKill)
        {
            if (myImg.color.a > 0.0f)
            {
                float na = myImg.color.a - Speed.get(0.01f) ;
                if (na < 0.0f) na = 0.0f;
                myImg.color = new Color(1f,1f,1f, na);
            }
            else if (myImg.color.a < 0.0f) myImg.color = new Color(1f,1f,1f, 0.0f);

            if (myImg.color.a == 0.0f)
            {
                kill();
            }
        }
	}

    public void kill()
    {
        myImg.translateIUPosY(Screen.height);
        myImg.color = Color.white;
        markedForKill = false;
        randomizeMe();
        Debug.Log("Kill on " + index_X + "," + index_Y);
    }

    public void randomizeMe()
    {

        type = UnityEngine.Random.Range(0, MyResources.characterTypes);
        myImg.sprite = MyResources.sprCharacterSprites[type];
    }

    public float getX()
    {
        return myRect.position.x;
    }

    public float getY()
    {
        return myRect.position.y;
    }

    public GameObject getSelectedAdjacentOne()
    {
        if (isClickSelected(index_X - 1, index_Y)) return GO.gems[index_X - 1, index_Y];
        if (isClickSelected(index_X + 1, index_Y)) return GO.gems[index_X + 1, index_Y];
        if (isClickSelected(index_X, index_Y - 1)) return GO.gems[index_X, index_Y - 1];
        if (isClickSelected(index_X, index_Y + 1)) return GO.gems[index_X, index_Y + 1];

        return null;
    }

    public void clickDeselectAll()
    {
        for (int y = 0; y < GO.gems.height(); y++)
        {
            for (int x = 0; x < GO.gems.width(); x++)
            {
                GO.gems[x, y].GetComponent<GemScript>().clickSelected = false;
            }
        }
        GO.gemSelector.enabled = false;
    }

    public void hide()
    {
        myImg.color = new Color(1f, 1f, 1f, 0f);
    }

    public void show()
    {
        myImg.color = new Color(1f, 1f, 1f, 1f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (dragActive) return;
        if (Core.resolvingInProgress) return;

        if (!clickSelected)
        {
            if (isAnyAdjacentClickSelected())
            {
                //resolve
                var adjGem = getSelectedAdjacentOne();

                //animation
                Core.startSwap(GO.gems[index_X, index_Y].GetComponent<GemScript>(), adjGem.GetComponent<GemScript>(), Core.SWAPSTATE.FORWARD);
                
                clickDeselectAll();
            }
            else
            {
                clickDeselectAll();
                clickSelected = true;
                GO.gemSelector.enabled = true;
                GO.gemSelector.setIUPos(getX(), getY());
            }
        }
        else
        {
            clickDeselectAll();
        }
        //Debug.Log("Click: " + this.name + " is now " + clickSelected);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (dragActive) return;
        if (Core.resolvingInProgress) return;
        dragActive = true;
        clickDeselectAll();
        GO.gemDrag.setIUPos(eventData.position);
        //Debug.Log("OnBeginDrag " + this.name);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragActive = false;
        //check if an adjecent is selected as well
        // if not, do nothing
        // if yes, do switch, select both

        //Debug.Log("OnEndDrag " + this.name);

        List<GemScript> intersectList = new List<GemScript>();
        foreach(GameObject g in GO.gems)
        {
            GemScript gs = g.GetComponent<GemScript>();
            if (gs.index_X == index_X && gs.index_Y == index_Y) continue;
            gs.myImg.color = Color.white;
            if (GO.gemDrag.rectTransform.rectOverlaps(gs.myRect) && isAdjacent(gs))
            {
                intersectList.Add(gs);
            }
        }

        GemScript swapGem = null;
        if (intersectList.Count == 2) // shouldnt be over 2, logically
        {
            GemScript gs0 = intersectList[0];
            GemScript gs1 = intersectList[1];
            float distance0 = this.myRect.rectDistance(gs0.myRect);
            float distance1 = this.myRect.rectDistance(gs1.myRect);

            if (distance1 > distance0)      swapGem = gs0;
            else                            swapGem = gs1;
        }
        else if (intersectList.Count == 1)
            swapGem = intersectList[0];

        if (swapGem != null)
        {
            Core.startSwap(swapGem, GO.gems[index_X, index_Y].GetComponent<GemScript>(), Core.SWAPSTATE.FORWARD);
        }

        //myImg.setIUPos(sourcePos.x, sourcePos.y);
    }

    public void OnDrag(PointerEventData data)
    {
        if (Core.resolvingInProgress) return;
        GO.gemDrag.translateIUPos(data.delta);

        transform.SetSiblingIndex(GO.gemSelector.transform.GetSiblingIndex() - 1);
    }

    public bool isAdjacent(GemScript gs)
    {
        return isAdjacent(gs.index_X, gs.index_Y);
    }

    public bool isAdjacent(int ix, int iy)
    {
        if (ix == index_X && iy == index_Y) return false;

        if ( (ix == index_X && iy == index_Y - 1) ||  
            (ix == index_X && iy == index_Y + 1) || 
            (ix == index_X + 1 && iy == index_Y) || 
            (ix == index_X - 1 && iy == index_Y) )
        {
            return true;
        }

        return false;
    }

    public bool intersects(GemScript gs)
    {
        //return (myImg.sprite.bounds.Intersects(gs.myImg.sprite.bounds));
        return myRect.rectOverlaps(gs.myRect);
    }

    public bool isAnyAdjacentClickSelected()
    {
        return ( isClickSelected(index_X - 1, index_Y) || 
            isClickSelected(index_X + 1, index_Y) || 
            isClickSelected(index_X, index_Y - 1) || 
            isClickSelected(index_X, index_Y + 1) );
    }

    public bool isClickSelected(int xIndex, int yIndex)
    {
        if (!Core.gemIndexInBounds(xIndex, yIndex)) return false;
        //Debug.Log(xIndex + "," + yIndex);
        return GO.gems[xIndex, yIndex].GetComponent<GemScript>().clickSelected;
    }


}
