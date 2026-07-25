using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SC_ResourceNode : MonoBehaviour
{
    [SerializeField] ResouceTypes type;
    [SerializeField] AttackType attackTypeThatCanBreakNode;
    [SerializeField] int amountPerHit;
    [SerializeField] int healthTotal;

    [SerializeField] AudioClip hit;

    [SerializeField] AudioClip breakNode;

    [SerializeField] PolygonCollider2D box;

    int currentHealth;
    SpriteRenderer spriteRenderer;

    private void OnEnable()
    {
        GameManager.OnTimeOfDayChanged += ResetResourceNode;
    }

    private void OnDisable()
    {
        GameManager.OnTimeOfDayChanged -= ResetResourceNode;
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = healthTotal;
    }

    //takes damage and returns true if the attack killed the enemy
    public void TakeDamage(int dmg)
    {
        currentHealth -= 1;
        SC_Player_Prop.OnResourcesAmountChanged?.Invoke(type, amountPerHit);
        spriteRenderer.color = Color.Lerp(Color.white, Color.black, currentHealth / healthTotal);
        if (currentHealth <= 0) 
        {
            spriteRenderer.enabled = false;
            GameManager.Instance.playSFX(breakNode.name, true);
            box.enabled = false;
            return;
        }
        GameManager.Instance.playSFX(hit.name, true);
    }

    public AttackType AttackTypeToBreakNode()
    {
        return attackTypeThatCanBreakNode;
    }

    private void ResetResourceNode(TimeOfDay timeOfDay)
    {
        if (timeOfDay != TimeOfDay.MORNING) return;

        currentHealth = healthTotal;
        spriteRenderer.color = Color.white;
        //spriteRenderer.enabled = true;
    }
}
