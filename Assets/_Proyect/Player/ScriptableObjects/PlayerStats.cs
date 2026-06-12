using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Paradigm Shift/Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("Movimiento Base")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float crouchSpeed = 1.5f;
    public float crawlSpeed = 0.75f;
    
    [Header("Salto")]
    public float jumpForce = 8f;
    
    [Header("Sprint")]
    public float sprintDuration = 3f;
    public float sprintCooldown = 2f;
    public float sprintRecoveryRate = 2f;
    
    [Header("Ground Check")]
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    
    [Header("Parálisis")]
    public float paralysisThreshold = 5f;
}