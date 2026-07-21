using UnityEngine;
using UnityEngine.UI;

public class SC_GameSettingsMenu : MonoBehaviour
{
    public Slider _musicSlider,_sfxSlider;

    [SerializeField] GameObject sliders;

    void Start()
    {
        sliders.SetActive(false);
    }

    public void Volume()
    {
        sliders.SetActive(true);
        if(Input.GetKeyDown("Left bracket")) {
            sliders.SetActive(false);
        }
        if (Input.GetKeyDown("Right bracket"))
        {
            sliders.SetActive(false);
        }
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
        SC_AudioManager.Instance.SFXVolume(_sfxSlider.value);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
