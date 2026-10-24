using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public float maxHealth = 100;
    public float currentHealth;
    public float parryDuration = 0.2f;
    public bool isParry = false;
    public bool canParry = true;
    public float attackDuration = 0.25f;
    public float groundCheckDistance = 1.2f;
    public LayerMask groundLayer;
    public float moveSpeed = 4f;
    public float jumpForce = 6f;
    public float rollSpeed = 8f;
    public float rollCooldown = 2f;
    public float rollDuration = 1f;
    public float invincibleDur;
    public bool isInvincible = false;
    public float knockbackDir;
    public float knockbackSpeed;
    public float knockbackJumpSpeed;
    
    public float lastRollTime { get; set; } = -10f;  // đánh dấu lần cuối nhảy với -10f nhỏ hơn thời điểm time.time khi bấm play là 0;
    public bool isDead { get; private set; }
    public PlayerInputActions InputActions { get; private set; }
    public StateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerParryState ParryState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerRollingState RollState { get; private set; }
    public float moveDirection { get; private set; }
    public Animator Anim { get; private set; }

    public Boss boss;
    void Awake()
    {
        if(rb == null) rb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        if(boss == null) boss = FindAnyObjectByType<Boss>();
        currentHealth = maxHealth;
        if (groundLayer.value == 0)
        {
            groundLayer = LayerMask.GetMask("Ground");
            if (groundLayer.value == 0)
            {
                groundLayer = ~LayerMask.GetMask("UI", "Ignore Raycast");
            }
        }
        isDead = false;
        InputActions = new PlayerInputActions();
        StateMachine = new StateMachine();
        IdleState = new PlayerIdleState(this, StateMachine);
        JumpState = new PlayerJumpState(this, StateMachine);
        MoveState = new PlayerMoveState(this, StateMachine);
        ParryState = new PlayerParryState(this, StateMachine);
        AttackState = new PlayerAttackState(this, StateMachine);
        RollState = new PlayerRollingState(this, StateMachine);
        StateMachine.ForceSetState(IdleState);
    }
    void OnEnable()
    {
        InputActions?.Player.Enable();
    }

    void OnDisable()
    {
        InputActions?.Player.Disable();
    }

    void Update()
    {
        moveDirection = InputActions.Player.Move.ReadValue<float>();
        StateMachine?.Tick();
        Anim.SetFloat("yVelocity", rb.linearVelocity.y);
        Anim.SetBool("isGrounded", IsGrounded());
        knockbackDir = Mathf.Sign(transform.localScale.x);
    }

    void FixedUpdate()
    {
        StateMachine?.FixedTick();
    }
    public bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            Vector2.down,
            groundCheckDistance,
            groundLayer
        );
        return hit.collider != null && hit.collider.gameObject != gameObject;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            transform.position,
            transform.position + Vector3.down * groundCheckDistance
        );
    }
    public bool CanRoll()
    {
        return Time.time >= lastRollTime + rollCooldown; 
    }
    public void Flip() 
    {
        if (moveDirection == 0) return;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(moveDirection);
        transform.localScale = scale;
    }
    public void TakeDamage(float damage)
    {
        if (currentHealth <= 0 || isInvincible) return; 
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth); 
        Debug.Log($"Player dính đòn! Mất {damage} máu. Máu còn: {currentHealth}/{maxHealth}");
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            if(knockbackDir != boss.knockbackDir)
            {
                rb.linearVelocity = new Vector2(-knockbackDir * knockbackSpeed, knockbackJumpSpeed);
            }
            else
            {
                rb.linearVelocity = new Vector2(knockbackDir*knockbackSpeed, knockbackJumpSpeed);
            }
            Anim.SetTrigger("TakeHit");
        }
    }
     private void Die()
    {
        Debug.Log("Player đã chết!");
        Anim.SetTrigger("isDead"); 
        isDead = true;
        InputActions.Player.Disable(); 
        rb.linearVelocity = Vector2.zero; 
    }

}
