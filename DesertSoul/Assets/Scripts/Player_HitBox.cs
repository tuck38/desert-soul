using UnityEngine;

public class Player_HitBox : MonoBehaviour
{

    [SerializeField] private PlayerProp prop;
    [SerializeField] private PlayerMove player;

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
            EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                prop.TakeDamage(enemy.GetDamage());
                player.Knockback(collision.gameObject);
                player.SetIFrames();
            }
        }
    }
}
