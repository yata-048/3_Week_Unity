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

    public void Enter()
    {
        player.Anim.SetBool("isMoving", true);
    }
    public void Tick()
    {
        moveInput = player.InputActions.Player.Move.ReadValue<float>();
        if (Mathf.Abs(moveInput) <= 0.05f)
        {
            stateMachine.ChangeState(player.IdleState);
            return;
        }

        player.Flip();

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
        if (player.InputActions.Player.Dodge.WasPressedThisFrame() && player.CanRoll())
        {
            stateMachine.ChangeState(player.RollState);
            return;
        }

        player.rb.linearVelocity = new Vector2(moveInput * player.moveSpeed, player.rb.linearVelocity.y);
    }
    public void FixedTick() {}
    public void Exit()
    {
        player.Anim.SetBool("isMoving", false);
    }
}
