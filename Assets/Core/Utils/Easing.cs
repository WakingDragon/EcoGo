using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Easing
{
    public static float In(float start, float end, float elapsedTime, float duration, float power)
    {
        var t = Mathf.Pow(elapsedTime / duration,power);
        var lerp = Mathf.Lerp(start, end, t);
        return lerp;
    }

    public static float Out(float start, float end, float elapsedTime, float duration,float power)
    {
        var t = elapsedTime / duration;
        t = 1- Mathf.Pow(1-t,power);
        var lerp = Mathf.Lerp(start, end, t);
        return lerp;
    }
}
