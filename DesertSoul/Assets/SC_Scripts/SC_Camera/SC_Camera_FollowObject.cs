using DG.Tweening;
using UnityEngine;

public class SC_Camera_FollowObject : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float rotationTime = 1f;

    private SC_Player_Move player;
    private bool isFacingRight;

    private void Awake()
    {
        player = playerTransform.gameObject.GetComponent<SC_Player_Move>();
        isFacingRight = player.isFacingRight;
    }

    private void Update()
    {
        transform.position = playerTransform.position;
        isFacingRight = player.isFacingRight;
    }

    public void Turn()
    {
        /// insert dotween turn
        transform.DORotate(DetermineEndRotation(), rotationTime).SetEase(Ease.InOutSine);
    }

    private Vector3 DetermineEndRotation()
    {
        if (isFacingRight)
            return new Vector3(0, 180, 0);
        else
            return new Vector3(0, 0, 0);
    }
}
