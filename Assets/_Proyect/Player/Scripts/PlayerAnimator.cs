using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    private Animator animator;
    private static readonly int IsWalkingHash = Animator.StringToHash("isWalking");
    private static readonly int IsRunningHash = Animator.StringToHash("isRunning");
    private static readonly int IsJumpingHash = Animator.StringToHash("isJumping");
    private static readonly int IsCrouchingHash = Animator.StringToHash("isCrouching");
    private static readonly int IsCrawlingHash = Animator.StringToHash("isCrawling");
    private static readonly int IsParalyzedHash = Animator.StringToHash("isParalyzed");
    private static readonly int SableEquippedHash = Animator.StringToHash("sableEquipped");
    private static readonly int TakeDamageHash = Animator.StringToHash("takeDamage");
    private static readonly int SableAttackHash = Animator.StringToHash("sableAttack");

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Leer estado del movimiento cada frame
        UpdateMovementAnimations();
    }

    private void UpdateMovementAnimations()
    {
        if (movement == null) return;
        
        // Usa getters públicos que añadiremos a PlayerMovement
        animator.SetBool(IsWalkingHash, movement.IsWalking && !movement.IsSprinting);
        animator.SetBool(IsRunningHash, movement.IsSprinting);
        animator.SetBool(IsJumpingHash, !movement.IsGrounded);
        animator.SetBool(IsCrouchingHash, movement.IsCrouching && !movement.IsCrawling);
        animator.SetBool(IsCrawlingHash, movement.IsCrawling);
    }

    public void SetParalyzed(bool value)
    {
        animator.SetBool(IsParalyzedHash, value);
    }

    public void SetSableEquipped(bool value)
    {
        animator.SetBool(SableEquippedHash, value);
    }

    public void TriggerTakeDamage()
    {
        animator.SetTrigger(TakeDamageHash);
    }

    public void TriggerSableAttack()
    {
        animator.SetTrigger(SableAttackHash);
    }

    public void FlipSprite(bool facingRight)
    {
        spriteRenderer.flipX = !facingRight;
    }
}