
using System.Collections.Generic;
using UnityEngine;

public static class CoroutineTime
{
    private static Dictionary<float, WaitForSeconds> waitForSecondsDic = new Dictionary<float, WaitForSeconds>();
    private static Dictionary<float, WaitForSecondsRealtime> waitForSecondsRealTimeDic = new Dictionary<float, WaitForSecondsRealtime>();
    public static WaitForSeconds GetWaitForSeconds(float time)
    {
        WaitForSeconds seconds;
        if (!waitForSecondsDic.TryGetValue(time, out seconds))
            seconds = new WaitForSeconds(time);
        
        return seconds;
    }

    public static WaitForSecondsRealtime GetWaitForSecondsRealtime(float time)
    {
        WaitForSecondsRealtime seconds;
        if (!waitForSecondsRealTimeDic.TryGetValue(time, out seconds))
            seconds = new WaitForSecondsRealtime(time);
        
        return seconds;
    }
}
