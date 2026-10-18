using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public float parryDuration = 0.2f;
    public bool isParry = false;
    public bool canParry = true;
    public float attackDuration = 0.25f;
    public float groundCheckDistance = 1.2f;
    public LayerMask groundLayer;
    public float moveSpeed = 4f;
    public float jumpForce = 6f;

    [System.NonSerialized] public PlayerInputActions InputActions;
    [System.NonSerialized] public StateMachine StateMachine;
    [System.NonSerialized] public PlayerIdleState IdleState;
    [System.NonSerialized] public PlayerJumpState JumpState;
    [System.NonSerialized] public PlayerMoveState MoveState;
    [System.NonSerialized] public PlayerParryState ParryState;
    [System.NonSerialized] public PlayerAttackState AttackState;

    void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (sr == null) sr = GetComponent<SpriteRenderer>();

        if (groundLayer.value == 0)
        {
            groundLayer = LayerMask.GetMask("Ground");
            if (groundLayer.value == 0)
            {
                groundLayer = ~LayerMask.GetMask("UI", "Ignore Raycast");
            }
        }

        InputActions = new PlayerInputActions();
        StateMachine = new StateMachine();
        IdleState = new PlayerIdleState(this, StateMachine);
        JumpState = new PlayerJumpState(this, StateMachine);
        MoveState = new PlayerMoveState(this, StateMachine);
        ParryState = new PlayerParryState(this, StateMachine);
        AttackState = new PlayerAttackState(this, StateMachine);
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
        StateMachine?.Tick();
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
}
