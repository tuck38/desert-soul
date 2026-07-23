using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class SC_AudioManager : MonoBehaviour
{
    public static SC_AudioManager Instance;


    [SerializeField] private AudioClip[] music, sfx;

    [SerializeField] private AudioSource musicSource, sfxSource;

    private float musicVol = 0.5f, sfxVol = 0.5f;

    [SerializeField] float prioBufferMax;

    private float currentTime;

    private void Awake()
    {
        Setup();
    }

    private void Update()
    {
        if(currentTime >= 0)
        {
            currentTime -= Time.deltaTime;
        }
    }

    public void Setup()
    {
        musicSource.volume = musicVol;
        sfxSource.volume = sfxVol;
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

    //priority sounds create a buffer until other sounds can be playe, they also jump the buffer
    public void PlaySFX(string name, bool prio = false, float bufferTime = 0.3f)
    {
        AudioClip current = Array.Find(sfx, x => x.name == name);
        if(prio == true)
        {
            currentTime = bufferTime;
        }

        if(current == null)
        {
            Debug.Log("sfx not found");
        }
        else
        {
            if(currentTime <= 0 || prio == true)
            {
                sfxSource.clip = current;
                sfxSource.Play();
            }
        }
    }

    public void StopSong()
    {
        musicSource.Stop();
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
        musicVol = volume;
        musicSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        sfxVol = volume;
        sfxSource.volume = volume;
    }

    public float GetMusic()
    {
        return musicVol;
    }

    public float GetSFX()
    {
        return sfxVol;
    }

    public void SetSound(float sfx, float music)
    {
        musicVol = music;
        musicSource.volume = music;

        sfxVol = sfx;
        sfxSource.volume = sfx;
    }
}
