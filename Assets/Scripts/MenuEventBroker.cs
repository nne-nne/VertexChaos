using UnityEngine;
using System;

public class MenuEventBroker
{
    public static event Action PlayerKilled;
    public static event Action<float> ShipDamage;
    public static event Action PauseMenuSwitch;
    public static event Action EndMenuSwitch;
    public static event Action Retry;
    

    public static void CallPlayerKilled()
    {
        PlayerKilled?.Invoke();
    }

    public static void CallShipDamage(float damage)
    {
        ShipDamage?.Invoke(damage);
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
}