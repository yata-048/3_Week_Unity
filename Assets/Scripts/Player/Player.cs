using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    [field:SerializeField] public float maxHealth { get; private set; } = 100;
    [field:SerializeField] public float currentHealth { get; private set; }
    [field:SerializeField]public float parryDuration { get; private set; } = 0.2f;
    public bool isParry = false;
    public bool canParry = true;
    [field:SerializeField] public float attackDuration { get; private set; } = 0.25f;
    [field:SerializeField] public float groundCheckDistance { get; private set; } = 1.2f;
    public LayerMask groundLayer { get; private set; }
    [field:SerializeField]public float moveSpeed { get; private set; } = 4f;
    [field:SerializeField] public float jumpForce { get; private set; } = 6f;
    [field:SerializeField] public float rollSpeed { get; private set; } = 8f;
    [field:SerializeField] public  float rollCooldown { get; private set; } = 2f;
    [field:SerializeField] public  float rollDuration { get; private set; } = 1f;
    [field:SerializeField] public float invincibleDur { get; private set; }
    public bool isInvincible { get; set; } = false;
    public float knockbackDir { get; private set; }
    [field:SerializeField] public float knockbackSpeed { get; private set; }
    [field:SerializeField] public float knockbackJumpSpeed { get; private set; }
    
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
    public PlayerTakeHitState TakeHitState { get; private set; }
    public float hurtDuration = 0.3f;
    public float moveDirection { get; private set; }
    public Animator Anim;

    public Boss boss;
    public HealthBar healthBar;
    void Awake()
    {
        if(rb == null) rb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        if(boss == null) boss = FindAnyObjectByType<Boss>();
        if (healthBar != null) healthBar.SetMaxHealth(maxHealth);
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
        TakeHitState = new PlayerTakeHitState(this, StateMachine);
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
        if (Time.timeScale == 0f) return;
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
    public void TakeDamage(float damage, Boss attacker)
    {
        if (currentHealth <= 0 || isInvincible) return;
        if (isParry)
        {
            if (attacker != null)
            {
                attacker.TakeDamage(attacker.bossDamageTaken);
            }
            return;
        }
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);
        if (healthBar != null) healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StateMachine.ChangeState(TakeHitState);
        }
    }
    private void Die()
    {
        Debug.Log("Player đã chết!");
        Anim.SetTrigger("isDead"); 
        isDead = true;
        InputActions.Player.Disable(); 
        rb.linearVelocity = Vector2.zero; 
        if (GameManager.Instance != null) GameManager.Instance.GameOver(false);
    }

}
