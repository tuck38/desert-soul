using UnityEngine;

public class Player_HitBox : MonoBehaviour
{

    [SerializeField] private PlayerProp prop;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
            }
        }
    }
}
