﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMusicManager : MonoBehaviour
{
    public static SoundMusicManager instance;
	public AudioSource backgroundMusic;
	public AudioSource backgroundMenuMusic;

    public AudioMixer mixer;
	

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

        mixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("@music", 0f));
        mixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("@sounds", 0f));
    }


    //  test
    private void Start() {
        //mixer.SetFloat("MusicVolume", -80f);
        //mixer.SetFloat("SFXVolume", -80f);
    }
	
	public void backgroundMenuMusicPlay()
	{
        backgroundMenuMusic.Play();
	}
	
	public void backgroundMusicPlay()
	{
        backgroundMusic.Play();
	}
	
	public void backgroundMenuMusicStop()
	{
        backgroundMenuMusic.Pause();
	}
	
	public void backgroundMusicStop()
	{
        backgroundMusic.Pause();
	}
	
	
    private void OnDestroy()
    {
        float music = 0f;
        float sound = 0f;
        mixer.GetFloat("MusicVolume", out music);
        mixer.GetFloat("SFXVolume", out sound);
        PlayerPrefs.SetFloat("@music", music);
        PlayerPrefs.SetFloat("@sounds", sound);
    }
}
