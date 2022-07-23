using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource _audioSource;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        EventManager.playSoundEvent += PlaySound;
    }

    private void OnDisable()
    {
        EventManager.playSoundEvent -= PlaySound;
    }


    void PlaySound(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }
}
