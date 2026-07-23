using UnityEngine;
using UnityEngine.UI;

public class SC_GameSettingsMenu : MonoBehaviour
{
    public Slider _musicSlider,_sfxSlider;

    [SerializeField] GameObject sliders;

    void Start()
    {
        //sliders.SetActive(false);
    }

    public void Volume()
    {

    }

    public void ToggleMusic()
    {
        SC_AudioManager.Instance.ToggleMusic();
    }

    public void ToggleSFX()
    {
        SC_AudioManager.Instance.ToggleSFX();
    }

    public void MusicVolume()
    {
        SC_AudioManager.Instance.MusicVolume(_musicSlider.value);
    }

    public void SFXVolume()
    {
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
