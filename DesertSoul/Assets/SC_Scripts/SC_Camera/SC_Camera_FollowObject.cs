using DG.Tweening;
using UnityEngine;

public class SC_Camera_FollowObject : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float rotationTime = 1f;
    public Transform currentFollow;

    private SC_Player_Move player;
    private bool isFacingRight;

    private void Awake()
    {
        player = playerTransform.gameObject.GetComponent<SC_Player_Move>();
        isFacingRight = player.isFacingRight;
        currentFollow = playerTransform;
    }

    private void Update()
    {
        transform.position = currentFollow.position;
        isFacingRight = player.isFacingRight;
    }

    public void lookingUp(Transform target)
    {
        currentFollow = target;
    }

    public void lookingDown(Transform target)
    {
        currentFollow = target;
    }

    public void notLooking()
    {
        currentFollow = playerTransform;
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
