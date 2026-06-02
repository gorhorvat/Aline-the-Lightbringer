using UnityEngine;
using UnityEngine.AI;

public abstract class BaseEnemy : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] AudioClip enemyDeathSfx;
    [SerializeField] float effectVolume = 1f;

    NavMeshAgent agent;
    Rigidbody rb;
    Transform modelMesh;
    Quaternion meshInitialRotation;
    bool isHit;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        modelMesh = GetComponentInChildren<MeshRenderer>().transform;
        meshInitialRotation = modelMesh.localRotation;
        agent.speed = moveSpeed;
    }

    protected abstract Vector3 GetTargetPosition();

    void Update()
    {
        if (Utils.IsBelowKillPlane(transform.position))
        {
            Destroy(gameObject);
        }

        if (!isHit && agent.enabled)
        {
            agent.SetDestination(GetTargetPosition());
            RotateTowardsMovement();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().Die(DamageType.Enemy);
        }
    }

    void RotateTowardsMovement()
    {
        Vector3 velocity = agent.velocity;
        Vector3 flatVelocity = new Vector3(velocity.x, 0f, velocity.z);

        if (flatVelocity.magnitude < 0.1f) return;

        float targetAngle = Mathf.Atan2(flatVelocity.x, flatVelocity.z) * Mathf.Rad2Deg;
        Quaternion yRotation = Quaternion.Euler(0f, targetAngle, 0f);
        modelMesh.rotation = Quaternion.Slerp(modelMesh.rotation, yRotation * Quaternion.Euler(-90f, 0f, 90f), Time.deltaTime * rotationSpeed);
    }

    public void GetHit(Vector3 force)
    {
        if (isHit) return;

        isHit = true;

        // disable any enemy movement/AI so it stops fighting the physics
        agent.enabled = false;

        AudioManager.Instance.PlaySfx(enemyDeathSfx, transform.position, effectVolume);

        rb.isKinematic = false;
        rb.AddForce(force, ForceMode.Impulse);

        Destroy(gameObject, 1.5f);
    }
}
