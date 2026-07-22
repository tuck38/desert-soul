using UnityEngine;

public class SC_Journal : MonoBehaviour
{


    [SerializeField] GameObject JournalBase;
    int current = 0;

    [SerializeField] GameObject[] tabs;

    [SerializeField] SC_MapManager map;

    [SerializeField] GameObject controls;

    [SerializeField] GameObject video;

    [SerializeField] GameObject audio;

    [SerializeField] AudioClip journalOpen;

    [SerializeField] AudioClip journalClose;

    [SerializeField] AudioClip journalPage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewTab(bool Dir)
    {
        GameManager.Instance.playSFX(journalOpen.name, true);
        if(Dir)
        {
            tabs[current].SetActive(false);
            if(current - 1 < 0)
            {
                current = tabs.Length - 1;
            }
            else
            {
                current--;
            }
            tabs[current].SetActive(true);
            //temp map activate code
            if(tabs[current].name == "Tab_Map")
            {
                map.ActivateMap();
            }

        }
        if(!Dir)
        {
            tabs[current].SetActive(false);
            if(current + 1 >= tabs.Length)
            {
                current = 0;
            }
            else
            {
                current++;
            }
            tabs[current].SetActive(true);
            //temp map activate code
            if(tabs[current].name == "Tab_Map")
            {
                map.ActivateMap();
            }
        }
    }

    public void OpenJournal()
    {
        JournalBase.SetActive(true);
        GameManager.Instance.playSFX(journalOpen.name, true);
    }

    public void CloseJournal()
    {
        JournalBase.SetActive(false);
        GameManager.Instance.playSFX(journalClose.name, true);
    }

    public void OnControlsPressed()
    {
        controls.SetActive(true);
        video.SetActive(false);
        audio.SetActive(false);
    }

    public void OnVideoPressed()
    {
        controls.SetActive(false);
        video.SetActive(true);
        audio.SetActive(false);
    }

    public void OnAudioPressed()
    {
        controls.SetActive(false);
        video.SetActive(false);
        audio.SetActive(true);
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
