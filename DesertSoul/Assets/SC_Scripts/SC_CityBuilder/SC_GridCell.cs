using UnityEngine;

public class SC_GridCell : MonoBehaviour
{
    public bool isOccupied = false;
    [SerializeField] Color occupiedColor = Color.red;
    [SerializeField] Color vaccantColor = Color.green;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isOccupied == true) spriteRenderer.color = occupiedColor;
        else spriteRenderer.color = vaccantColor;
    }
}
