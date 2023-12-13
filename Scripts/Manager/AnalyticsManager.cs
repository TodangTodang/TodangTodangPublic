using Firebase;
using Firebase.Analytics;
using Unity.VisualScripting;
using UnityEngine;

public static class AnalyticsManager
{
    private static bool isInit = false;
    public static void Init()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                isInit = true;    
                Debug.Log("애널리틱스 준비완료");
            }
            else
            {
                Debug.Log("연결에 문제가 있습니다");
            }
        });
    }
    public static void LogEvent(string eventName)
    {
        Init();
        FirebaseAnalytics.LogEvent(eventName);
        Debug.Log($"{eventName}을 전송 하였습니다");
    }

    public static void DayEnd(int day)
    {
        LogEvent($"Day{day}SpendEvent");
    }

    public static void AchievementRecipe(string name, int level)
    {
        if(level == 1)
            LogEvent($"Unlock{name}");
        else if(level == 5)
            LogEvent($"Master{name}"); 
    }

    public static void Ending(string Ending)
    {
        LogEvent($"End{Ending}");
    }
}
