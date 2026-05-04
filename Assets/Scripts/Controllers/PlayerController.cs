using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float normalSpeed = 5f;
    [SerializeField] float sprintSpeed = 10f;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float ascendMultiplier = 2f;
    [SerializeField] float jumpForce = 8f;
    [SerializeField] float jumpCutMultiplier = 0.5f;
    [SerializeField] float groundCheckRadius = 0.3f;
    [SerializeField] LayerMask groundLayer;

    private Rigidbody rb;
    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private LayerMask groundLayerMask;
    private bool isGrounded;
    private bool isJumping;
    private bool isSprinting;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputActions = new PlayerInputActions();
        groundLayerMask = groundLayer & ~LayerMask.GetMask("Ignore Raycast");
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
    }

    void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Sprint.performed += OnSprint;
        inputActions.Player.Sprint.canceled += OnSprint;
        inputActions.Player.Jump.started -= OnJumpStarted;
        inputActions.Player.Jump.canceled -= OnJumpCanceled;
        inputActions.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    private void OnSprint(InputAction.CallbackContext ctx)
    {
        isSprinting = !isSprinting;
    }

    private void OnJumpStarted(InputAction.CallbackContext ctx)
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            isJumping = true;
        }
    }

    private void OnJumpCanceled(InputAction.CallbackContext ctx)
    {
        if (isJumping && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier, rb.linearVelocity.z);
        }

        isJumping = false;
    }

    void FixedUpdate()
    {
        if (rb.linearVelocity.y < 0)
        {
            // Make player fall faster
            rb.linearVelocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !isJumping)
        {
            // Half jump, fall faster
            rb.linearVelocity += Vector3.up * Physics.gravity.y * ascendMultiplier * Time.fixedDeltaTime;
        }

        float currentSpeed = isSprinting ? sprintSpeed : normalSpeed;
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y) * currentSpeed;
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);
    }

    void Update()
    {
        // Check if sphere touches the ground. Player's collider is excluded
        Vector3 spherePos = transform.position + Vector3.down * 0.9f;
        isGrounded = Physics.CheckSphere(spherePos, groundCheckRadius, groundLayerMask);

        if (Utils.IsBelowKillPlane(transform.position))
        {
            GameManager.Instance.ReloadScene();
        }
    }
}