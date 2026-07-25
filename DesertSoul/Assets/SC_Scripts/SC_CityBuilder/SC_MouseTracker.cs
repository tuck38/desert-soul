using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class SC_MouseTracker : MonoBehaviour
{
    Vector2 defaultPosition;

    float buildingX;

    float buildingY;

    float moveValue = 1f;

    [SerializeField] float moveCD = 0.1f;

    float currentCD;

    bool canMove = true;

    bool pressed = false;

    [SerializeField] SpriteRenderer area;

    [SerializeField] SpriteRenderer rend;

    [SerializeField] Transform boundsHigh;

    [SerializeField] Transform boundsLow;

    Vector3 newPos;

    public float halfWidth, halfHeight;


    private void Start()
    {
        currentCD = moveCD;
        defaultPosition = transform.position;
    }
    private void OnDisable()
    {
        transform.position = defaultPosition;
    }

    void Update()
    {
        if(currentCD < moveCD)
        {
            currentCD += Time.deltaTime;
        }

        if(Gamepad.current.dpad.up.wasPressedThisFrame)
        {
            newPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + moveValue, gameObject.transform.position.z);
            pressed = true;
        }
        else if(Gamepad.current.dpad.down.wasPressedThisFrame)
        {
            newPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - moveValue, gameObject.transform.position.z);
            pressed = true;
        }
        else if(Gamepad.current.dpad.left.wasPressedThisFrame)
        {
            newPos = new Vector3(gameObject.transform.position.x - moveValue, gameObject.transform.position.y, gameObject.transform.position.z);
            pressed = true;
        }
        else if(Gamepad.current.dpad.right.wasPressedThisFrame)
        {
            newPos = new Vector3(gameObject.transform.position.x + moveValue, gameObject.transform.position.y, gameObject.transform.position.z);
            pressed = true;
        }

        //Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //transform.position = new Vector2(mousePosition.x + buildingX, mousePosition.y + buildingY);

        if(pressed == true)
        {
            gameObject.transform.position = newPos;
        }

        /*if(newPos.x < boundsHigh.position.x && newPos.y < boundsHigh.position.y && newPos.x > boundsLow.position.x && newPos.y > boundsLow.position.y)
        {
            gameObject.transform.position = newPos;
        }*/

        //Vector3 screenPoint = Camera.main.WorldToScreenPoint(worldPosition);
        //Mouse.current.WarpCursorPosition(screenPoint);
    }

    public void Move(Vector2 move)
    {
        if(currentCD >= moveCD && move != Vector2.zero)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + move.x, gameObject.transform.position.y + move.y, gameObject.transform.position.z);
            currentCD = 0;
        }
        /*if(move == new Vector2(1, 0))
        {
            //left
        }
        else if(move == new Vector2(-1, 0))
        {
            //right
        }
        else if(move == new Vector2(0, 1))
        {
            //up
        }
        else if(move == new Vector2(0, -1))
        {
            
        }*/
    }

    public void SetBuildingDimensions(float x, float y)
    {
        buildingX = x;

        buildingY = y;
    }

    public void cantPlace()
    {
        area.color = new Color(Color.red.r, Color.red.g, Color.red.b, area.color.a);
    }

    public void canPlace()
    {
        area.color = new Color(Color.green.r, Color.green.g, Color.green.b, area.color.a);
    }
}
