using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SpriteRenderer))]
public class SC_Building : MonoBehaviour
{
    [SerializeField] string buildingName;
    [Tooltip("x = number of brown resource, y = number of purple resource")]
    [SerializeField] Vector2Int materialCost;
    [SerializeField] bool unlocked;
    [SerializeField] bool isMulticell;
    [Tooltip("The numbers of cells along the x and y axis the building fills, only needed if isMulticell is true")]
    [SerializeField] Vector2Int dimensions;

    public string BuildingName { get => buildingName; }
    public Sprite BuildingSprite { get => GetComponent<SpriteRenderer>().sprite; }
    public Vector2Int MaterialCost { get => materialCost; }
    public bool Unlocked { get => unlocked; }
    public bool IsMulticell { get => isMulticell; }
    public Vector2Int Dimensions { get => dimensions; }

    /// <summary>
    /// Unlock Building in Shop
    /// </summary>
    [ContextMenu("Unlock Building")]
    public void Unlock()
    {
        unlocked = true;
    }

    private void OnMouseDown()
    {
        SceneManager.LoadScene("BuildingTest");
    }

}
