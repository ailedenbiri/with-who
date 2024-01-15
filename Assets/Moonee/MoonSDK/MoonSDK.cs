using GameAnalyticsSDK;
using System;
using UnityEngine;

public static class MoonSDK
{
    public const string Version = "1.0.2";

    public static void TrackCustomEvent(string eventName)
    {
        GameAnalytics.NewDesignEvent(eventName);
    }
    public static void TrackLevelEvents(LevelEvents eventType, int levelIndex)
    {
        string outputValue = "level" + String.Format("{0:D4}", levelIndex);

        if (eventType == LevelEvents.Start)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, outputValue);
        }
        else if (eventType == LevelEvents.Fail)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, outputValue);
        }
        else if (eventType == LevelEvents.Complete)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, outputValue);
        }
    }
    public enum LevelEvents
    {
        Start,
        Fail,
        Complete
    }
}