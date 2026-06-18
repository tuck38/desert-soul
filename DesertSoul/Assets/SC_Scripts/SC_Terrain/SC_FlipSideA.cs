using UnityEngine;

public class SC_FlipSideA : MonoBehaviour
{
    SC_FlipSideB flipB;
    [SerializeField] GameObject partSystem;
    public bool facingA = false;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if(facingA){
                partSystem.transform.Rotate(180f,0,0);
                facingA = true;
                flipB.facingB = false;
            }
        }
        
    }
}
