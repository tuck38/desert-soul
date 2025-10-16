using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SC_ResourceNode : MonoBehaviour
{
    [SerializeField] ResouceTypes type;
    [SerializeField] AttackType attackTypeThatCanBreakNode;
    [SerializeField] int amountPerHit;
    [SerializeField] int healthTotal;

    int currentHealth;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = healthTotal;
    }

    //takes damage and returns true if the attack killed the enemy
    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        SC_ResourceTestScript.OnResourcesAmountChanged?.Invoke(type, amountPerHit);
        spriteRenderer.color = Color.Lerp(Color.white, Color.black, currentHealth / healthTotal);
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public AttackType AttackTypeToBreakNode()
    {
        return attackTypeThatCanBreakNode;
    }
}
