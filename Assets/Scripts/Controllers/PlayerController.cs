using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float normalSpeed = 5f;
    [SerializeField] float sprintSpeed = 10f;

    [SerializeField] float jumpSpeed = 4f;
    [SerializeField] float rotationSpeed = 10f;

    [SerializeField] float attackRange = 1f;
    [SerializeField] float attackForce = 100f;
    [SerializeField] float attackUpwardForce = 20f;

    [SerializeField] float groundDistance = 0.2f;

    [Header("Jump Feel")]
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float jumpCutMultiplier = 0.5f;
    [SerializeField] float jumpApexGravityMultiplier = 0.6f;

    [SerializeField] Transform modelMesh;
    [SerializeField] Transform groundCheck;

    [SerializeField] LayerMask enemyLayer;
    [SerializeField] LayerMask groundMask;

    [SerializeField] GameObject attackVFXPrefab;
    [SerializeField] AudioClip playerFallDeathSfx;
    [SerializeField] AudioClip playerWaterDeathSfx;
    [SerializeField] AudioClip playerEnemyDeathSfx;
    [SerializeField] float deathEffectVolume = 1f;

    PlayerInputActions inputActions;

    Rigidbody rb;
    Rigidbody currentPlatform;

    Transform cam;

    Quaternion meshInitialRotation;

    Vector2 moveInput;
    Vector3 facingDirection = Vector3.forward;
    Vector3 groundNormal = Vector3.up;

    bool isGrounded;
    bool isJumping;
    bool isHoldingJump;
    bool isSprinting;
    bool isDead;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputActions = new PlayerInputActions();

        cam = Camera.main.transform;
        meshInitialRotation = modelMesh.localRotation;
    }

    void OnEnable()
    {
        inputActions.Player.Enable();

        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;

        inputActions.Player.Sprint.performed += OnSprint;
        inputActions.Player.Sprint.canceled += OnSprint;

        inputActions.Player.Jump.performed += OnJumpStarted;
        inputActions.Player.Jump.canceled += OnJumpCanceled;

        inputActions.Player.Attack.started += OnAttack;
    }

    void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;

        inputActions.Player.Sprint.performed -= OnSprint;
        inputActions.Player.Sprint.canceled -= OnSprint;

        inputActions.Player.Jump.performed -= OnJumpStarted;
        inputActions.Player.Jump.canceled -= OnJumpCanceled;

        inputActions.Player.Attack.started -= OnAttack;

        inputActions.Player.Disable();
    }

    void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    void OnSprint(InputAction.CallbackContext ctx)
    {
        isSprinting = ctx.performed;
    }

    void OnJumpStarted(InputAction.CallbackContext ctx)
    {
        if (isGrounded)
        {
            isJumping = true;
            isHoldingJump = true;
        }
    }

    void OnJumpCanceled(InputAction.CallbackContext ctx)
    {
        isHoldingJump = false;

        // Early jump cut (tight hop control)
        if (rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector3(
                rb.linearVelocity.x,
                rb.linearVelocity.y * jumpCutMultiplier,
                rb.linearVelocity.z
            );
        }
    }

    void OnAttack(InputAction.CallbackContext ctx)
    {
        SpawnAttackVFX();

        Vector3 boxCenter =
            transform.position + facingDirection * attackRange + Vector3.up * 0.5f;

        Vector3 boxHalfExtents =
            new Vector3(1f, 1f, attackRange) * 0.5f;

        Collider[] hits = Physics.OverlapBox(
            boxCenter,
            boxHalfExtents,
            Quaternion.LookRotation(facingDirection),
            enemyLayer
        );

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy") &&
                hit.TryGetComponent<BaseEnemy>(out BaseEnemy enemy))
            {
                Vector3 dir = hit.transform.position - transform.position;
                dir.y = 0f;

                Vector3 force =
                    dir.normalized * attackForce +
                    Vector3.up * attackUpwardForce;

                enemy.GetHit(force);
            }
        }
    }

    void SpawnAttackVFX()
    {
        if (attackVFXPrefab == null) return;

        Vector3 spawnPos =
            transform.position +
            facingDirection * 1.0f +
            Vector3.up * 0.5f;

        Quaternion rot = Quaternion.LookRotation(facingDirection);

        GameObject vfx = Instantiate(attackVFXPrefab, spawnPos, rot);

        Destroy(vfx, 1f);
    }

    void FixedUpdate()
    {
        HandleGrounding();

        Vector3 moveDirection = CalculateMoveDirection();

        HandleRotation(moveDirection);

        Vector3 velocity = CalculateVelocity(moveDirection);

        velocity = HandlePlatformInfluence(velocity);

        velocity = ApplyGravity(velocity);

        ApplyMovement(velocity);

        HandleDeath();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            Die(DamageType.Water);
        }
    }

    void HandleGrounding()
    {
        isGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundDistance,
            groundMask
        );

        if (Physics.Raycast(
            groundCheck.position,
            Vector3.down,
            out RaycastHit hit,
            groundDistance + 0.3f,
            groundMask))
        {
            groundNormal = hit.normal;
        }
        else
        {
            groundNormal = Vector3.up;
        }
    }

    Vector3 CalculateMoveDirection()
    {
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        return camForward * moveInput.y + camRight * moveInput.x;
    }

    void HandleRotation(Vector3 moveDirection)
    {
        if (moveDirection.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

        modelMesh.rotation = Quaternion.Slerp(
            modelMesh.rotation,
            targetRotation * meshInitialRotation,
            Time.fixedDeltaTime * rotationSpeed
        );

        facingDirection = moveDirection.normalized;
    }

    Vector3 CalculateVelocity(Vector3 moveDirection)
    {
        float speed =
            isSprinting && isGrounded ? sprintSpeed : normalSpeed;

        Vector3 horizontal =
            isGrounded
                ? Vector3.ProjectOnPlane(moveDirection, groundNormal)
                : moveDirection;

        horizontal *= speed;

        Vector3 vel = rb.linearVelocity;

        if (isJumping)
        {
            vel.y = jumpSpeed;
            isJumping = false;
        }

        return new Vector3(horizontal.x, vel.y, horizontal.z);
    }

    Vector3 HandlePlatformInfluence(Vector3 vel)
    {
        if (currentPlatform != null && isGrounded)
        {
            vel += new Vector3(
                currentPlatform.linearVelocity.x,
                0f,
                currentPlatform.linearVelocity.z
            );
        }

        return vel;
    }

    Vector3 ApplyGravity(Vector3 vel)
    {
        if (vel.y > 0f)
        {
            if (isHoldingJump)
            {
                // softer gravity → higher jump if holding
                vel.y += Physics.gravity.y * (jumpApexGravityMultiplier - 1f) * Time.fixedDeltaTime;
            }
            else
            {
                // stronger gravity → short hop feel
                vel.y += Physics.gravity.y * (fallMultiplier - 1f) * Time.fixedDeltaTime;
            }
        }
        else if (vel.y < 0f)
        {
            vel.y += Physics.gravity.y * fallMultiplier * Time.fixedDeltaTime;
        }

        return vel;
    }

    void ApplyMovement(Vector3 vel)
    {
        rb.linearVelocity = vel;
    }

    void HandleDeath()
    {
        if (!isDead && Utils.IsBelowKillPlane(transform.position))
        {
            Die(DamageType.Fall);
        }
    }

    public void SetCurrentPlatform(Rigidbody platform)
    {
        currentPlatform = platform;
    }

    public void Die(DamageType type)
    {
        if (isDead) return;

        isDead = true;

        AudioClip deathSfx = type switch
        {
            DamageType.Fall => playerFallDeathSfx,
            DamageType.Hazard => playerFallDeathSfx,
            DamageType.Water => playerWaterDeathSfx,
            DamageType.Enemy => playerEnemyDeathSfx,
            _ => null
        };

        AudioManager.Instance.PlaySfx(deathSfx, transform.position, deathEffectVolume);

        GameManager.Instance.LoseLife();
    }
}