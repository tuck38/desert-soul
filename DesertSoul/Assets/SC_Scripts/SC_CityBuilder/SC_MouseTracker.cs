using UnityEngine;
using UnityEngine.InputSystem;

public class SC_MouseTracker : MonoBehaviour
{
    Vector2 defaultPosition;

    float buildingx;

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
        transform.position = mousePosition;

        //Vector3 screenPoint = Camera.main.WorldToScreenPoint(worldPosition);
        //Mouse.current.WarpCursorPosition(screenPoint);
    }

    public void SetBuildingDimensions(int x, int y)
    {
        buildingx = x;

        buildingY = y;
    }
}
