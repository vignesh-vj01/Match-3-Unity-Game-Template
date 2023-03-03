using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyResources
{
    public static readonly int characterTypes = 7;
    public static bool loaded = false;
    public static Sprite[] sprCharacterSprites;
    public static Sprite sprCharacterSelection;

    public static void initCharacterSprites()
    {
        sprCharacterSprites = new Sprite[characterTypes];
        for (int i = 0; i < sprCharacterSprites.Length; i++)
        {
            sprCharacterSprites[i] = Resources.Load<Sprite>("sprites/characters_000" + (i + 1));
        }
        sprCharacterSelection = Resources.Load<Sprite>("sprites/selection");
        loaded = true;
    }

}
