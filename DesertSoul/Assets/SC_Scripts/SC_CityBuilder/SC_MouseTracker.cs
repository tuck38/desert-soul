using UnityEngine;
using UnityEngine.InputSystem;

public class SC_MouseTracker : MonoBehaviour
{
    Vector2 defaultPosition;

    float buildingX;

    float buildingY;
    private void Start()
    {
        defaultPosition = transform.position;
    }

    private void OnDisable()
    {
        transform.position = defaultPosition;
    }

    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(mousePosition.x + buildingX, mousePosition.y + buildingY);

        //Vector3 screenPoint = Camera.main.WorldToScreenPoint(worldPosition);
        //Mouse.current.WarpCursorPosition(screenPoint);
    }

    public void SetBuildingDimensions(float x, float y)
    {
        buildingX = x;

        buildingY = y;
    }
}
