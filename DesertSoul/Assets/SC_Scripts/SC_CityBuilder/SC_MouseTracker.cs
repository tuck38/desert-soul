using UnityEngine;

public class SC_MouseTracker : MonoBehaviour
{
    Vector2 defaultPosition;
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
    }
}
