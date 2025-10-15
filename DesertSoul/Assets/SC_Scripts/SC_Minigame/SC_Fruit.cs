using UnityEngine;

public class SC_Fruit : MonoBehaviour
{
    public void FruitHit()
    {
        //Tell Minigame fruit was hit
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.tag != "Player") Destroy(gameObject);
    }
}
