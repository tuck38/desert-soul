using UnityEngine;

public class SC_HurtBox : MonoBehaviour
{
    //Sythe hurtbox, self explanitory 

    public SC_Attack_Base currentAttack;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            SC_Enemy_Base enemy = collision.gameObject.GetComponent<SC_Enemy_Base>();
            if(enemy != null && currentAttack != null)
            {
                enemy.TakeDamage(currentAttack.getDamage());
            }
        }
        if (collision.gameObject.tag == "ResourceNode")
        {
            SC_ResourceNode node = collision.gameObject.GetComponent<SC_ResourceNode>();
            if (node != null && currentAttack != null)
            {
                node.TakeDamage(currentAttack.getDamage());
            }
        }
    }
}
