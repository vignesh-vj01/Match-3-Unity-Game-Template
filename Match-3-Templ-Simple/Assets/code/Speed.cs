using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Speed {

    private static readonly int GAMESPEED = 220;

    public static float get(float factor = 1.0f)
    {
        return (Time.smoothDeltaTime * GAMESPEED * factor);
    }
}
