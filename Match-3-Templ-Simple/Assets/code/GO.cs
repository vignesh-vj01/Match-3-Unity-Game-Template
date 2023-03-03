using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class GO
{
    public static Image gemSelector;
    public static GameObject[,] gems;
    public static UnityEngine.UI.Text txtPoints;
    public static GemSwapper gemSwitch0;
    public static GemSwapper gemSwitch1;
    public static Image gemDrag;
    public static AudioSource audio_music;
    public static AudioSource audio_sound;

    public static void init()
    {
        txtPoints = GameObject.Find("txtCurPoints").GetComponent<Text>();
        gemSelector = GameObject.Find("gem_selector").GetComponent<Image>();
        gemDrag = GameObject.Find("gem_drag").GetComponent<Image>();
        gems = new GameObject[5, 6];
        for (int y = 0; y < gems.height(); y++)
        {
            for (int x = 0; x < gems.width(); x++)
            {
                GameObject go = GameObject.Find("gem_" + x + "." + y);
                gems[x, y] = go;
                var gs = gems[x, y].GetComponent<GemScript>();
                gs.index_X = x;
                gs.index_Y = y;
            }
        }
        //Debug.Log("Map: w:" + gems.width() + ", h:" + gems.height());
        gemSwitch0 = GameObject.Find("gem_swap-A").GetComponent<GemSwapper>();
        gemSwitch1 = GameObject.Find("gem_swap-B").GetComponent<GemSwapper>();
        audio_music = GameObject.Find("audio_music").GetComponent<AudioSource>();
        audio_sound = GameObject.Find("audio_sound").GetComponent<AudioSource>();
    }
}
