using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float stoppingDistance = 0.5f;

    protected Rigidbody rb;
    bool isHit;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    protected abstract Vector3 GetTargetPosition();

    void Update()
    {
        if (Utils.IsBelowKillPlane(transform.position))
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        Vector3 target = GetTargetPosition();
        MoveTowards(target);
    }

    void MoveTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position);
        Vector3 flatDirection = new Vector3(direction.x, 0f, direction.z);

        if (flatDirection.magnitude <= stoppingDistance)
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            return;
        }

        flatDirection.Normalize();

        rb.linearVelocity = new Vector3(
            flatDirection.x * moveSpeed,
            rb.linearVelocity.y,
            flatDirection.z * moveSpeed
        );

        transform.rotation = Quaternion.LookRotation(flatDirection);
    }

    public void GetHit(Vector3 force)
    {
        if (isHit) return;

        isHit = true;

        // disable any enemy movement/AI so it stops fighting the physics
        enabled = false;

        if (TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = false;
            rb.AddForce(force, ForceMode.Impulse);
        }

        Destroy(gameObject, 1.5f);
    }
}
