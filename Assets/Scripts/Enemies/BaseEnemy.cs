using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 3f;
    [SerializeField] float stoppingDistance = 0.5f;

    protected Rigidbody rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    protected abstract Vector3 GetTargetPosition();

    private void Update()
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.ReloadScene();
        }
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
}
