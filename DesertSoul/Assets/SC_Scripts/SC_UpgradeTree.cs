using UnityEngine;
using UnityEngine.UI;

public class SC_UpgradeTree : MonoBehaviour
{
    [SerializeField] GameObject minimap;
    [SerializeField] GameObject uiParent;
    [SerializeField] Button scytheTab;
    [SerializeField] Button drillTab;
    [SerializeField] Button hookshotTab;
    [SerializeField] GameObject scytheUpgradeTree;
    [SerializeField] GameObject drillUpgradeTree;
    [SerializeField] GameObject hookshotUpgradeTree;
    [SerializeField] KeyCode toggleKey;
    [SerializeField] SC_Player_Move player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiParent?.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(toggleKey)) ToggleTree();
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            minimap.gameObject.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            minimap.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Changes the active Upgrade Tree: 1 = Scythe, 2 = Drill, 3 = Hookshot
    /// </summary>
    /// <param name="tabIndex"></param>
    public void TabChange(int tabIndex)
    {
        switch(tabIndex)
        {
            case 1:
                scytheTab.interactable = false;
                drillTab.interactable = true;
                //hookshotTab.interactable = true;
                scytheUpgradeTree?.SetActive(true);
                drillUpgradeTree?.SetActive(false);
                //hookshotUpgradeTree?.SetActive(false);
                break;
            case 2:
                scytheTab.interactable = true;
                drillTab.interactable = false;
                //hookshotTab.interactable = true;
                scytheUpgradeTree?.SetActive(false);
                drillUpgradeTree?.SetActive(true);
                //hookshotUpgradeTree?.SetActive(false);
                break;
            case 3:
                scytheTab.interactable = true;
                drillTab.interactable = true;
                //hookshotTab.interactable = false;
                scytheUpgradeTree?.SetActive(false);
                drillUpgradeTree?.SetActive(false);
                //hookshotUpgradeTree?.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// Toggles the active state of the Upgrade Tree UI
    /// </summary>
    void ToggleTree()
    {
        uiParent.SetActive(!uiParent.activeSelf);
        player.SetCanMove(!uiParent.activeSelf);
    }
}
