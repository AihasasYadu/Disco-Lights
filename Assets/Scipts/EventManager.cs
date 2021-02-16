using System;
using UnityEngine;

public class EventManager : MonoSingletonGeneric<EventManager>
{
    public delegate void CheckConnected();
    public static event CheckConnected DisableInteractability;
    public static event CheckConnected EnableInteractability;

    public void DisableEvent()
    {
        DisableInteractability?.Invoke();
    }
    
    public void EnableEvent()
    {
        EnableInteractability?.Invoke();
    }
}
