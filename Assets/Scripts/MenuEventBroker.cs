using UnityEngine;
using System;

public class MenuEventBroker
{
    public static event Action PlayerKilled;
    public static event Action<float> HealthChange;
    public static event Action PauseMenuSwitch;
    public static event Action EndMenuSwitch;
    public static event Action Retry;
    
    public static event Action<int> LevelChange;
    

    public static void CallPlayerKilled()
    {
        PlayerKilled?.Invoke();
    }

    public static void CallHealthChange(float ratio)
    {
        HealthChange?.Invoke(ratio);
    }
    
    public static void CallPauseMenuSwitch()
    {
        PauseMenuSwitch?.Invoke();
    }
    
    public static void CallEndMenuSwitch()
    {
        EndMenuSwitch?.Invoke();
    }

    public static void CallRetry()
    {
        Retry?.Invoke();
    }
    
    public static void CallLevelChange(int level)
    {
        LevelChange?.Invoke(level);
    }
}