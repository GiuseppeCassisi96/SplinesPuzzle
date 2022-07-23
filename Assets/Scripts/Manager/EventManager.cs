using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action<int> EnterEvent;
    public static event Action<int> ExitEvent;
    public static event Action<Transform> LookEvent;
    public static event Action<AudioClip> playSoundEvent;

    public static void EnterAction(int triggerId)
    {
        EnterEvent.Invoke(triggerId);
    }

    public static void ExitAction(int triggerId)
    {
        ExitEvent.Invoke(triggerId);
    }

    public static void LookAction(Transform tr)
    {
        LookEvent.Invoke(tr);
    }

    public static void PlaySoundAction(AudioClip clip)
    {
        playSoundEvent.Invoke(clip);
    }
}
