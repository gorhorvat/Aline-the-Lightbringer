using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float normalSpeed = 5f;
    [SerializeField] float sprintSpeed = 10f;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float jumpSpeed = 6f;
    [SerializeField] float jumpCutMultiplier = 0.5f;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float attackRange = 1f;
    [SerializeField] float attackForce = 100f;
    [SerializeField] float attackUpwardForce = 20f;
    [SerializeField] float groundDistance = 0.2f;
    [SerializeField] Transform modelMesh;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] LayerMask groundMask;
    [SerializeField] Transform groundCheck;
    [SerializeField] AudioClip playerFallDeathSfx;
    [SerializeField] AudioClip playerEnemyDeathSfx;

    PlayerInputActions inputActions;
    Rigidbody rb;
    Rigidbody currentPlatform;
    AudioSource playerSfxSource;
    Transform cam;
    Quaternion meshInitialRotation;
    Vector2 moveInput;
    Vector3 velocity;
    Vector3 facingDirection;
    bool isGrounded;
    bool isJumping;
    bool isHoldingJump;
    bool isSprinting;
    bool isDead;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputActions = new PlayerInputActions();
        playerSfxSource = GetComponent<AudioSource>();
        meshInitialRotation = modelMesh.localRotation;
        facingDirection = Vector3.forward;
        cam = Camera.main.transform;
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
        if (isHoldingJump && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier, rb.linearVelocity.z);
        }

        isHoldingJump = false;
    }

    void OnAttack(InputAction.CallbackContext ctx)
    {
        Vector3 boxCenter = transform.position + facingDirection * attackRange + Vector3.up * 0.5f;
        Vector3 boxHalfExtents = new Vector3(1f, 1f, attackRange) * 0.5f;

        Collider[] hits = Physics.OverlapBox(boxCenter, boxHalfExtents, Quaternion.LookRotation(facingDirection), enemyLayer);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy") && hit.TryGetComponent<BaseEnemy>(out BaseEnemy enemy))
            {
                Vector3 direction = (hit.transform.position - transform.position).normalized;
                direction.y = 0f;
                Vector3 force = direction * attackForce + Vector3.up * attackUpwardForce;
                enemy.GetHit(force);
            }
        }
    }

    void FixedUpdate()
    {
        // set movement speed
        float currentSpeed = isSprinting && isGrounded ? sprintSpeed : normalSpeed;

        // set camera-free movement
        Vector3 cameraForward = cam.forward;
        Vector3 cameraRight = cam.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = cameraForward * moveInput.y + cameraRight * moveInput.x;

        velocity = moveDirection * currentSpeed;
        velocity.y = rb.linearVelocity.y;

        // jump
        if (isJumping)
        {
            velocity.y = jumpSpeed;
            isJumping = false;
        }

        // rotate based on input only, before adding platform velocity
        Vector3 inputVelocity = moveDirection;
        if (inputVelocity.magnitude > 0.1f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(inputVelocity);

            modelMesh.rotation = Quaternion.Slerp(
                modelMesh.rotation,
                targetRotation * meshInitialRotation,
                Time.fixedDeltaTime * rotationSpeed
            );

            facingDirection = inputVelocity.normalized;
        }

        // add platform velocity after rotation
        if (currentPlatform != null)
        {
            Vector3 platformVelocity = currentPlatform.linearVelocity;
            velocity.x += platformVelocity.x;
            velocity.z += platformVelocity.z;
        }

        // move player
        rb.linearVelocity = velocity;

        // apply fall multiplier
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }

        GroundCheck();

        if (!isDead && Utils.IsBelowKillPlane(transform.position))
        {
            Die(DamageType.Fall);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(
            transform.position + facingDirection * attackRange + Vector3.up * 0.5f,
            Quaternion.LookRotation(facingDirection),
            Vector3.one
        );
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(1f, 1f, attackRange));

        //Gizmos.color = Color.red;

        //Gizmos.DrawWireSphere(
        //    groundCheck.position,
        //    groundDistance
        //);
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
    }

    public void Die(DamageType damageType)
    {
        isDead = true;
        playerSfxSource.clip = damageType switch
        {
            DamageType.Fall => playerFallDeathSfx,
            DamageType.Enemy => playerEnemyDeathSfx,
            _ => null
        };

        playerSfxSource.Play();
        GameManager.Instance.LoseLife();
    }

    public void SetCurrentPlatform(Rigidbody rb)
    {
        currentPlatform = rb;
    }
}