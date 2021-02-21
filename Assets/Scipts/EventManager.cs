using System;
using UnityEngine;

public class EventManager : MonoSingletonGeneric<EventManager>
{
    public delegate void Executable();
    public static event Executable DisableInteractability;
    public static event Executable EnableInteractability;

    public delegate void Paramterized(Transform temp);
    public static event Paramterized ButtonClickEvent;

    public void DisableEvent()
    {
        DisableInteractability?.Invoke();
    }
    
    public void EnableEvent()
    {
        EnableInteractability?.Invoke();
    }

    public void ButtonClickedEvent(Transform temp)
    {
        ButtonClickEvent?.Invoke(temp);
    }
}
