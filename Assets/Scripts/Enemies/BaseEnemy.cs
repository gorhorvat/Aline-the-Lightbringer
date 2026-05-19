using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float rotationSpeed = 10f;

    protected Rigidbody rb;
    AudioSource enemyDeathSfx;
    Transform modelMesh;
    Quaternion meshInitialRotation;
    bool isHit;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        enemyDeathSfx = GetComponent<AudioSource>();
        modelMesh = GetComponentInChildren<MeshRenderer>().transform;
        meshInitialRotation = modelMesh.localRotation;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().Die(DamageType.Enemy);
        }
    }

    void MoveTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position);
        Vector3 flatDirection = new Vector3(direction.x, 0f, direction.z).normalized;

        rb.linearVelocity = new Vector3(
            flatDirection.x * moveSpeed,
            rb.linearVelocity.y,
            flatDirection.z * moveSpeed
        );

        // rotate model when walking towards target
        float targetAngle = Mathf.Atan2(flatDirection.x, flatDirection.z) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
        modelMesh.localRotation = Quaternion.Slerp(modelMesh.localRotation, targetRotation * meshInitialRotation, Time.deltaTime * rotationSpeed);
    }

    public void GetHit(Vector3 force)
    {
        if (isHit) return;

        isHit = true;

        // disable any enemy movement/AI so it stops fighting the physics
        enabled = false;

        if (enemyDeathSfx != null)
        {
            enemyDeathSfx.Play();
        }

        if (TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = false;
            rb.AddForce(force, ForceMode.Impulse);
        }

        Destroy(gameObject, 1.5f);
    }
}
