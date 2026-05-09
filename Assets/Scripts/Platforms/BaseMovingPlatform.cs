using UnityEngine;

public abstract class BaseMovingPlatform : MonoBehaviour
{
    [SerializeField] protected float moveDistance = 5f;
    [SerializeField] protected float moveSpeed = 3f;
    [SerializeField] float smoothTime = 0.5f;

    protected Vector3 startPosition;
    Vector3 targetPosition;
    Vector3 currentVelocity;
    bool movingToTarget = true;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        targetPosition = GetTargetPosition();
    }

    void FixedUpdate()
    {
        Vector3 newPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime, moveSpeed);
        rb.MovePosition(newPosition);

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            targetPosition = movingToTarget ? startPosition : GetTargetPosition();
            movingToTarget = !movingToTarget;
        }
    }

    protected abstract Vector3 GetTargetPosition();
}