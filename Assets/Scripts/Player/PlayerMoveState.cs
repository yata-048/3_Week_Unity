using Unity.VisualScripting;
using UnityEngine;

public class PlayerMoveState : IState
{
    private Player player;
    private StateMachine stateMachine;
    private float moveInput;
    public PlayerMoveState(Player player, StateMachine stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
    }

    public void Enter() {}
    public void Tick()
    {
        moveInput = player.InputActions.Player.Move.ReadValue<float>();
        if (Mathf.Abs(moveInput) <= 0.05f)
        {
            stateMachine.ChangeState(player.IdleState);
            return;
        }

        if (moveInput > 0.05f)
            player.sr.flipX = false;
        else if (moveInput < -0.05f)
            player.sr.flipX = true;

        if (player.InputActions.Player.Jump.WasPressedThisFrame() && player.IsGrounded())
        {
            stateMachine.ChangeState(player.JumpState);
            return;
        }

        if (player.InputActions.Player.Attack.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.AttackState);
            return;
        }

        if (player.InputActions.Player.Parry.WasPressedThisFrame() && player.canParry)
        {
            stateMachine.ChangeState(player.ParryState);
            return;
        }

        player.rb.linearVelocity = new Vector2(moveInput * player.moveSpeed, player.rb.linearVelocity.y);
    }
    public void FixedTick() {}
    public void Exit() {}
}
