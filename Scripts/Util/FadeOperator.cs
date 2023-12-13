
using System;
using System.Collections;
using UnityEngine;

public static class FadeOperator
{
    public static IEnumerator FadeLinear(float start, float end, float fadeTime, Action<float> fadeFunction)
    {
        float curTime = 0f;
        float rate = 0f;

        while (rate < 1)
        {
            curTime += Time.unscaledDeltaTime;
            rate = curTime / fadeTime;

            fadeFunction(Mathf.Lerp(start, end, rate));

            yield return null;
        }
    }
}
