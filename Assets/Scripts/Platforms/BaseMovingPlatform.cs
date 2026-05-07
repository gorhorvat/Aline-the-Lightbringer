using UnityEngine;

public abstract class BaseMovingPlatform : MonoBehaviour
{
    [SerializeField] protected float moveDistance = 5f;
    [SerializeField] protected float moveSpeed = 0.5f;

    protected Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void LateUpdate()
    {
        float t = Mathf.PingPong(Time.time * moveSpeed, 1f);
        transform.position = Vector3.Lerp(startPosition, GetTargetPosition(), t);
    }

    // Subclasses define where the platform moves TO
    protected abstract Vector3 GetTargetPosition();
}
