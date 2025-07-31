using UnityEngine;

public class WP_HurtBox : MonoBehaviour
{

    public AttackBase currentAttack;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("hmm");
        if (collision.gameObject.tag == "Enemy")
        {
            EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();
            if(enemy != null && currentAttack != null)
            {
                enemy.TakeDamage(currentAttack.getDamage());
                Debug.Log("?");
            }
        }
    }
}
