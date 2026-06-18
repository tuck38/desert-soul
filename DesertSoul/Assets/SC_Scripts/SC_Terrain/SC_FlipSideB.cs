using UnityEngine;

public class SC_FlipSideB : MonoBehaviour
{
    SC_FlipSideA flipA;
    [SerializeField] GameObject partSystem;
    public bool facingB = false;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if(facingB){
                partSystem.transform.Rotate(180f,0,0);
                facingB = true;
                flipA.facingA = false;
            }
        }
    }
}
