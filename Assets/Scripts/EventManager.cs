using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action<int> EnterEvent;
    public static event Action<int> ExitEvent;

    public static void EnterAction(int triggerId)
    {
        EnterEvent.Invoke(triggerId);
    }

    public static void ExitAction(int triggerId)
    {
        ExitEvent.Invoke(triggerId);
    }
}
