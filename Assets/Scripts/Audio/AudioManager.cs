using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource _audioSource;
    [SerializeField]
    bool isSFX = false;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(this.gameObject);
    }
    private void OnEnable()
    {
        EventManager.playSoundEvent += PlayBackgroundSound;
        EventManager.playSoundSFXEvent += PlaySFXSound;
    }

    private void OnDisable()
    {
        EventManager.playSoundEvent -= PlayBackgroundSound;
        EventManager.playSoundSFXEvent -= PlaySFXSound;
    }


    void PlayBackgroundSound(AudioClip clip)
    {
        if(!isSFX)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }   
    }

    void PlaySFXSound(AudioClip clip)
    {
        if(isSFX)
        {
            _audioSource.PlayOneShot(clip);
        }
        
    }

}
