using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public delegate void OnReadyEvent();
public delegate void OnPlayEvent();
public delegate void OnBossEvent();
public delegate void OnClearEvent();
public delegate void OnDeadEvent();
public class Stage_Mng
{

    public Stage_State State;
    public int MaxCount = 10;
    public int Count;

    public OnReadyEvent ReadyEvent;
    public OnPlayEvent PlayEvent;
    public OnBossEvent  BossEvent;
    public OnClearEvent ClearEvent;
    public OnDeadEvent  DeadEvent;

   
    public void State_Change(Stage_State state)
    {
        State = state;
        switch (state)
        {
            case Stage_State.Ready:
                ReadyEvent?.Invoke();
                Base_Mng.instance.Coroutine_Action(2.0f, () => State_Change(Stage_State.Play));
                break;
            case Stage_State.Play: 
                PlayEvent?.Invoke();
                break;
            case Stage_State.Boss: 
                BossEvent?.Invoke();
                break;
            case Stage_State.Clear: 
                ClearEvent?.Invoke();
                break;
            case Stage_State.Dead: 
                DeadEvent?.Invoke();
                break;

        }
    }

}
