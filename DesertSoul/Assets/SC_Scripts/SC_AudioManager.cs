using System;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;

public class SC_AudioManager : MonoBehaviour
{
    public static SC_AudioManager Instance;


    [SerializeField] private AudioClip[] music, sfx;

    [SerializeField] private AudioSource musicSource, sfxSource;

    [SerializeField] float prioBufferMax;

    private float currentTime;

    private void Awake()
    {
        
    }

    private void Update()
    {
        if(currentTime >= 0)
        {
            currentTime -= Time.deltaTime;
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

    //priority sounds create a buffer until other sounds can be playe, they also jump the buffer
    public void PlaySFX(string name, bool prio = false, float bufferTime = 0.3f)
    {
        AudioClip current = Array.Find(sfx, x => x.name == name);
        if(prio == true)
        {
            Debug.Log("prio sound");
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
                Debug.Log("new sound");
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
        musicSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
