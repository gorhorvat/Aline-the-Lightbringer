using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float normalSpeed = 5f;
    [SerializeField] float sprintSpeed = 10f;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float jumpForce = 6f;
    [SerializeField] float jumpCutMultiplier = 0.5f;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float attackRange = 1f;
    [SerializeField] float attackForce = 100f;
    [SerializeField] float attackUpwardForce = 20f;
    [SerializeField] LayerMask enemyLayer;

    CharacterController cc;
    PlayerInputActions inputActions;
    Transform modelMesh;
    Quaternion meshInitialRotation;
    Vector2 moveInput;
    Vector3 velocity;
    Vector3 facingDirection;
    bool isJumping;
    bool isSprinting;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        inputActions = new PlayerInputActions();
        modelMesh = GetComponentInChildren<MeshRenderer>().transform;
        meshInitialRotation = modelMesh.localRotation;
        facingDirection = Vector3.forward;
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Sprint.performed += OnSprint;
        inputActions.Player.Sprint.canceled += OnSprint;
        inputActions.Player.Jump.started += OnJumpStarted;
        inputActions.Player.Jump.canceled += OnJumpCanceled;
        inputActions.Player.Attack.started += OnAttack;
    }

    void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Sprint.performed -= OnSprint;
        inputActions.Player.Sprint.canceled -= OnSprint;
        inputActions.Player.Jump.started -= OnJumpStarted;
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
        if (cc.isGrounded)
        {
            velocity.y = jumpForce;
            isJumping = true;
        }
    }

    void OnJumpCanceled(InputAction.CallbackContext ctx)
    {
        if (isJumping && velocity.y > 0)
            velocity.y *= jumpCutMultiplier;
        isJumping = false;
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

    void Update()
    {
        if (cc.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float currentGravity = Physics.gravity.y;
        if (velocity.y < 0)
            currentGravity *= fallMultiplier;

        velocity.y += currentGravity * Time.deltaTime;

        float currentSpeed = isSprinting ? sprintSpeed : normalSpeed;
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y) * currentSpeed;

        if (move.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            modelMesh.localRotation = Quaternion.Slerp(modelMesh.localRotation, targetRotation * meshInitialRotation, Time.deltaTime * rotationSpeed);
            facingDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }

        cc.Move((move + new Vector3(0f, velocity.y, 0f)) * Time.deltaTime);

        if (Utils.IsBelowKillPlane(transform.position))
            GameManager.Instance.LoseLife();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Enemy"))
            GameManager.Instance.LoseLife();

        if (hit.gameObject.TryGetComponent<CrumblingPlatform>(out CrumblingPlatform crumbling))
            crumbling.OnPlayerLand();
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
    }
}