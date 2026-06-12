using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private PlayerStats stats;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform ceilingCheck;
    
    [Header("Crouch")]
    [SerializeField] private Collider2D standingCollider;
    [SerializeField] private Collider2D crouchingCollider;
    
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isGrounded;
    private bool jumpRequested;
    private float currentSpeed;
    private bool isCrouching;
    private bool isCrawling;
    private bool isSprinting;
    private bool facingRight = true;

    // Propiedades públicas de solo lectura
    public bool IsGrounded => isGrounded;
    public bool IsCrouching => isCrouching;
    public bool IsCrawling => isCrawling;
    public bool IsSprinting => isSprinting;
    public bool IsWalking => Mathf.Abs(moveInput.x) > 0.1f;
    public bool IsFacingRight => facingRight;
    
    public event System.Action<bool> OnFacingDirectionChanged;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        inputReader.OnMoveEvent += HandleMoveInput;
        inputReader.OnJumpEvent += HandleJumpInput;
        inputReader.OnCrouchEvent += HandleCrouchInput;
        inputReader.OnSprintEvent += HandleSprintInput;
    }

    void OnDisable()
    {
        inputReader.OnMoveEvent -= HandleMoveInput;
        inputReader.OnJumpEvent -= HandleJumpInput;
        inputReader.OnCrouchEvent -= HandleCrouchInput;
        inputReader.OnSprintEvent -= HandleSprintInput;
    }

    void FixedUpdate()
    {
        CheckGrounded();
        ApplyMovement();
        ApplyJump();
    }

    private void HandleMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    private void HandleJumpInput()
    {
        if (isGrounded && !isCrouching && !isCrawling)
            jumpRequested = true;
    }

    private void HandleCrouchInput(bool pressed)
    {
        if (pressed && isGrounded)
        {
            if (!isCrouching)
            {
                isCrouching = true;
                if (standingCollider != null) standingCollider.enabled = false;
                if (crouchingCollider != null) crouchingCollider.enabled = true;
                
                if (ceilingCheck != null && Physics2D.OverlapCircle(ceilingCheck.position, 0.2f, stats.groundLayer))
                {
                    isCrawling = true;
                }
            }
        }
        else if (!pressed && isCrouching)
        {
            if (ceilingCheck == null || !Physics2D.OverlapCircle(ceilingCheck.position, 0.2f, stats.groundLayer))
            {
                isCrouching = false;
                isCrawling = false;
                if (standingCollider != null) standingCollider.enabled = true;
                if (crouchingCollider != null) crouchingCollider.enabled = false;
            }
        }
    }

    private void HandleSprintInput(bool pressed)
    {
        isSprinting = pressed && !isCrouching && !isCrawling && Mathf.Abs(moveInput.x) > 0.1f;
    }

    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, stats.groundCheckRadius, stats.groundLayer);
    }

    private void ApplyMovement()
    {
        if (isCrawling)
            currentSpeed = stats.crawlSpeed;
        else if (isCrouching)
            currentSpeed = stats.crouchSpeed;
        else if (isSprinting)
            currentSpeed = stats.runSpeed;
        else
            currentSpeed = stats.walkSpeed;

        float velocityX = moveInput.x * currentSpeed;
        rb.linearVelocity = new Vector2(velocityX, rb.linearVelocity.y);

        if (velocityX > 0 && !facingRight)
            Flip();
        else if (velocityX < 0 && facingRight)
            Flip();
    }

    private void ApplyJump()
    {
        if (jumpRequested)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, stats.jumpForce);
            jumpRequested = false;
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        OnFacingDirectionChanged?.Invoke(facingRight);
    }

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            float radius = stats != null ? stats.groundCheckRadius : 0.2f;
            Gizmos.DrawWireSphere(groundCheck.position, radius);
        }
        
        if (ceilingCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(ceilingCheck.position, 0.2f);
        }
    }
}