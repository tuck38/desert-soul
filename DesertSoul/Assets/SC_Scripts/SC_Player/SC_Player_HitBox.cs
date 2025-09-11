using UnityEngine;

public class SC_Player_HitBox : MonoBehaviour
{

    [SerializeField] private SC_Player_Prop prop;
    [SerializeField] private SC_Player_Move player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //replace assigned variable with getcomponents
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            SC_Enemy_Base enemy = collision.gameObject.GetComponent<SC_Enemy_Base>();
            if (enemy != null)
            {
                prop.TakeDamage(enemy.GetDamage());
                player.Knockback(collision.gameObject);
                player.SetIFrames();
            }
        }
    }
}
