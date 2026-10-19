using UnityEngine;

public class PlayerJumpState : IState
{
    private Player player;
    private StateMachine stateMachine;
    private float moveInput;
    private float jumpDelayTimer;

    public PlayerJumpState(Player player, StateMachine stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
    }
    public void Enter()
    {
        player.rb.linearVelocity = new Vector2(player.rb.linearVelocity.x, player.jumpForce);
        jumpDelayTimer = 0.1f;
    }
    public void Tick()
    {
        moveInput = player.InputActions.Player.Move.ReadValue<float>();
        player.Flip();
        if (jumpDelayTimer > 0f)
        {
            jumpDelayTimer -= Time.deltaTime;
        }
        else
        {
            if (player.rb.linearVelocity.y <= 0.1f && player.IsGrounded())
            {
                if (Mathf.Abs(moveInput) > 0.05f)
                    stateMachine.ChangeState(player.MoveState);
                else
                    stateMachine.ChangeState(player.IdleState);
                return;
            }
        }

        player.rb.linearVelocity = new Vector2(moveInput * player.moveSpeed, player.rb.linearVelocity.y);
    }
    public void FixedTick() {}
    public void Exit() {}
}
