using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class SC_AudioManager : MonoBehaviour
{
    public static SC_AudioManager instance;


    [SerializeField] private AudioClip[] music, sfx;

    [SerializeField] private AudioSource musicSource, sfxSource;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
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
}
