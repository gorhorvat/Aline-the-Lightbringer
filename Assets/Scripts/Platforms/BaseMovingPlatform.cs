using UnityEngine;

public abstract class BaseMovingPlatform : MonoBehaviour
{
    [SerializeField] protected float moveDistance = 5f;
    [SerializeField] protected float moveSpeed = 0.5f;
    [SerializeField] protected float contactTolerance = 0.9f;

    protected Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void FixedUpdate()
    {
        float t = Mathf.PingPong(Time.time * moveSpeed, 1f);
        transform.position = Vector3.Lerp(startPosition, GetTargetPosition(), t);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        ContactPoint contact = collision.GetContact(0);
        if (contact.normal.y < -contactTolerance)
            collision.transform.SetParent(transform);
    }

    void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        ContactPoint contact = collision.GetContact(0);
        if (contact.normal.y >= -contactTolerance)
            collision.transform.SetParent(null);
    }

    void OnCollisionExit(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        collision.transform.SetParent(null);
    }

    // Subclasses define where the platform moves TO
    protected abstract Vector3 GetTargetPosition();
}
