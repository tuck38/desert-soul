using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class SC_AudioManager : MonoBehaviour
{
    public static SC_AudioManager Instance;


    [SerializeField] private AudioClip[] music, sfx;

    [SerializeField] private AudioSource musicSource, sfxSource;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySong(string name)
    {
        AudioClip current = Array.Find(music, x => x.name == name);

        if(current == null)
        {
            Debug.Log("Sound not found");
        }
        else
        {
            musicSource.clip = current;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        AudioClip current = Array.Find(sfx, x => x.name == name);

        if(current == null)
        {
            Debug.Log("sfx not found");
        }
        else
        {
            sfxSource.clip = current;
            sfxSource.Play();
        }
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
