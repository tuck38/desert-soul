using UnityEngine;

public class SC_SunBeam : MonoBehaviour
{
    private bool playerIn = false;

    [SerializeField] private int damage = 2;

    [SerializeField] private float sunStackAddFrequency = 0.2f;
    private float sunStackTimer;

    private SC_Player_Prop playerProp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sunStackTimer = sunStackAddFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIn)
        {
            
            AddSunStack();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (playerProp == null)
            {
                playerProp = collision.gameObject.GetComponent<SC_Player_Prop>();
            }
            playerIn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerIn = false;
            playerProp.OutOfSun();
        }
    }

    private void AddSunStack()
    {
        if (sunStackTimer > 0)
        {
            sunStackTimer -= Time.deltaTime;
            if (sunStackTimer <= 0)
            {
                playerProp.giveSunStack(damage);
                sunStackTimer = sunStackAddFrequency;
            }
        }
    }
}
