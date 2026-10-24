using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody2D rb;
    public Player player;
    [Header("Stats")]
    public float maxHealth = 100;
    public float currentHealth;
    public float chaseSpeed = 3f;
    public float attackRange = 3.5f;     
    public float attackCooldown = 1.5f;
    public float attackDuration = 0.5f;  
    public float damageDelay = 0.4f;   
    public int attackDamage = 25;
    public float hurtDuration = 0.3f;
    public float bossDamageTaken = 10f;
    public float knockbackSpeed;
    public float knockbackDir;
    public float knockbackJumpSpeed;

    [Header("Hitbox")]
    public Vector2 attackBoxSize = new Vector2(3.5f, 1.5f); // Kích thước hộp sát thương
    public Vector2 attackOffset = new Vector2(2f, -0.5f);   // Đẩy lệch về phía trước mặt
    public LayerMask playerLayer;

    public float lastAttackTime { get; set; } = -10f;
    public StateMachine StateMachine { get; private set; }
    public BossMoveState ChaseState { get; private set; }
    public BossAttackState AttackState { get; private set; }
    public BossTakeHitState GetHitState { get; private set; }
    public BossDeadState DeadState { get; private set; }
    public Animator Anim { get; private set; }

    void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (Anim == null) Anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        if (player == null)
        {
            player = FindAnyObjectByType<Player>();
        }

        if (playerLayer.value == 0)
        {
            playerLayer = LayerMask.GetMask("Player");
        }
        StateMachine = new StateMachine();
        ChaseState = new BossMoveState(this, StateMachine);
        AttackState = new BossAttackState(this, StateMachine);
        GetHitState = new BossTakeHitState(this, StateMachine);
        DeadState = new BossDeadState(this, StateMachine);

        StateMachine.ForceSetState(ChaseState);
    }

    void Update()
    {
        StateMachine?.Tick();
        float speed = Mathf.Abs(rb.linearVelocityX);
        knockbackDir = Mathf.Sign(transform.localScale.x); // them dau - luc take hit
        Anim.SetFloat("xVelocity", speed);
    }

    void FixedUpdate()
    {
        StateMachine?.FixedTick();
    }

    public void Flip(float directionX)
    {
        if (directionX == 0) return;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(directionX);
        transform.localScale = scale;
    }

    public bool CanAttack()
    {
        return Time.time >= lastAttackTime + attackCooldown;
    }
    public Vector2 GetAttackBoxCenter()
    {
        float facingDir = Mathf.Sign(transform.localScale.x);
        return (Vector2)transform.position + new Vector2(attackOffset.x * facingDir, attackOffset.y);
    }
    public void TakeDamage(float damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        Debug.Log($"Boss ăn {damage} damage, máu còn: {currentHealth}");

        if (currentHealth <= 0)
        {
            StateMachine.ChangeState(DeadState);
        }
        else
        {
            StateMachine.ChangeState(GetHitState);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(GetAttackBoxCenter(), attackBoxSize);
    }
    
    public void OpenAttackHitBox()
    {
        if(StateMachine.CurrentState == AttackState)
        {
            AttackState.OpenAttackHitBox();
        }
    }
    public void CloseAttackHitBox()
    {
        if(StateMachine.CurrentState == AttackState)
        {
            AttackState.CloseAttackHitBox();
        }
    }
}