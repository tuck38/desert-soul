using UnityEngine;

public class SC_EnemyLOS : MonoBehaviour
{
    [SerializeField] SC_RattleSnake enemy;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            enemy.PlayerDetected(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            enemy.PlayerLost();
        }
    }
}
