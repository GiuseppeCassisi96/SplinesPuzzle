using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    AudioSource _audioSource;
    [SerializeField]
    bool isSFX = false;
    [SerializeField]
    Slider slider;
    static AudioManager SFXinstance = null;
    static AudioManager Backgroundinstance = null;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = slider.value;
    }

    private void Start()
    {
        Singleton();
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

     
    public void SetVolume(Slider slider)
    {
        _audioSource.volume = slider.value;
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

    void Singleton()
    {

        if(isSFX)
        {
            if (SFXinstance == null)
            {
                SFXinstance = this;
                return;
            }
            Destroy(this.gameObject);
        }
        else
        {
            if (Backgroundinstance == null)
            {
                Backgroundinstance = this;
                return;
            }
            Destroy(this.gameObject);
        }
        
    }

}
