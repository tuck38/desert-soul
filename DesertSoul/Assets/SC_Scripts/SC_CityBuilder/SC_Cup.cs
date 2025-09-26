using UnityEngine;

public class SC_Cup : MonoBehaviour
{
    public int cupNumber;

    private void OnMouseDown()
    {
        SC_BallCupGame.OnCupClicked.Invoke(cupNumber);
    }
}
