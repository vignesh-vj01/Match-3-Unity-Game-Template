using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public static class Util {

    #region extensions
    #region UI with RectTransform
    public static void setPositionFromOtherRect(this Graphic g, Graphic ing)
    {
        Vector3 v = g.rectTransform.position;
        v.x = ing.rectTransform.position.x;
        v.y = ing.rectTransform.position.y;
        v.z = ing.rectTransform.position.z;
        g.GetComponent<RectTransform>().position = v;
    }
    /* 
    public static float setPositionFromOtherRect(this RectTransform rectTrans1, RectTransform rectTrans2)
    {
        Rect rect1 = new Rect(rectTrans1.localPosition.x, rectTrans1.localPosition.y, rectTrans1.rect.width, rectTrans1.rect.height);
        Rect rect2 = new Rect(rectTrans2.localPosition.x, rectTrans2.localPosition.y, rectTrans2.rect.width, rectTrans2.rect.height);

        return (rect1.center - rect2.center).magnitude;
    }*/

    public static void setIUPos(this Graphic g, Vector2 inv)
    {
        Vector3 v = g.rectTransform.position;
        v.x = inv.x;
        v.y = inv.y;
        g.GetComponent<RectTransform>().position = v;
    }


    public static void setIUPos(this Graphic g, float inx, float iny)
    {
        Vector3 v = g.rectTransform.position;
        v.x = inx;
        v.y = iny;
        g.GetComponent<RectTransform>().position = v;
    }

    public static void translateIUPos(this Graphic g, Vector2 inv)
    {
        Vector3 v = g.rectTransform.position;
        v.x += inv.x;
        v.y += inv.y;
        g.GetComponent<RectTransform>().position = v;
    }

    public static void translateIUPos(this Graphic g, float inx, float iny)
    {
        Vector3 v = g.rectTransform.position;
        v.x = v.x + inx;
        v.y = v.y + iny;
        g.GetComponent<RectTransform>().position = v;
    }

    public static void setIUPosX(this Graphic g, float inx)
    {
        Vector3 v = g.rectTransform.position;
        v.x = inx;
        g.GetComponent<RectTransform>().position = v;
    }

    public static void translateIUPosX(this Graphic g, float inx)
    {
        Vector3 v = g.rectTransform.position;
        v.x = v.x + inx;
        g.GetComponent<RectTransform>().position = v;
    }

    public static void setIUPosY(this Graphic g, float iny)
    {
        Vector3 v = g.rectTransform.position;
        v.y = iny;
        g.GetComponent<RectTransform>().position = v;
    }

    public static void translateIUPosY(this Graphic g, float iny)
    {
        Vector3 v = g.rectTransform.position;
        v.y = v.y + iny;
        g.GetComponent<RectTransform>().position = v;
    }
    #endregion

    public static string[] Split(this string str, string splitter)
    {
        return str.Split(new string[] { splitter }, StringSplitOptions.None);
    }

    public static bool rectOverlaps(this RectTransform rectTrans1, RectTransform rectTrans2)
    {
        Rect rect1 = new Rect(rectTrans1.localPosition.x, rectTrans1.localPosition.y, rectTrans1.rect.width, rectTrans1.rect.height);
        Rect rect2 = new Rect(rectTrans2.localPosition.x, rectTrans2.localPosition.y, rectTrans2.rect.width, rectTrans2.rect.height);

        return rect1.Overlaps(rect2);
    }

    public static Vector2 rectDistanceVector(this RectTransform rectTrans1, RectTransform rectTrans2)
    {
        Rect rect1 = new Rect(rectTrans1.localPosition.x, rectTrans1.localPosition.y, rectTrans1.rect.width, rectTrans1.rect.height);
        Rect rect2 = new Rect(rectTrans2.localPosition.x, rectTrans2.localPosition.y, rectTrans2.rect.width, rectTrans2.rect.height);

        return rect1.center - rect2.center;
    }

    public static float rectDistance(this RectTransform rectTrans1, RectTransform rectTrans2)
    {
        Rect rect1 = new Rect(rectTrans1.localPosition.x, rectTrans1.localPosition.y, rectTrans1.rect.width, rectTrans1.rect.height);
        Rect rect2 = new Rect(rectTrans2.localPosition.x, rectTrans2.localPosition.y, rectTrans2.rect.width, rectTrans2.rect.height);

        return (rect1.center - rect2.center).magnitude;
    }

    #endregion

    public static int width(this Array ar)
    {
        return ar.GetLength(0);
    }

    public static int height(this Array ar)
    {
        return ar.GetLength(1);
    }

    public static string numToDigits(int number, int n)
    {
        StringBuilder str = new StringBuilder("" + number);
        int zeroesToAdd = n - str.Length;
        if (zeroesToAdd > 0)
        {
            for (int i = 0; i < zeroesToAdd; i++)
            {
                str.Insert(0, "0");
            }
        }
        return str.ToString();
    }

}
