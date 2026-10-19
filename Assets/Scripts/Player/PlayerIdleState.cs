using UnityEngine;

public class PlayerIdleState : IState
{
    private Player player;
    private StateMachine stateMachine;

    public PlayerIdleState(Player player, StateMachine stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        player.rb.linearVelocity = new Vector2(0f, player.rb.linearVelocity.y);
    }

    public void Tick()
    {
        float moveInput = player.InputActions.Player.Move.ReadValue<float>();
        if (Mathf.Abs(moveInput) > 0.05f)
        {
            stateMachine.ChangeState(player.MoveState);
            return;
        }
        if (player.InputActions.Player.Jump.WasPressedThisFrame() && player.IsGrounded())
        {
            stateMachine.ChangeState(player.JumpState);
            return;
        }
        if (player.InputActions.Player.Parry.WasPressedThisFrame() && player.canParry)
        {
            stateMachine.ChangeState(player.ParryState);
            return;
        }
        if (player.InputActions.Player.Attack.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.AttackState);
            return;
        }
        if (player.InputActions.Player.Dodge.WasPressedThisFrame() && player.CanRoll())
        {
            stateMachine.ChangeState(player.RollState);
            return;
        }
    }

    public void FixedTick() { }

    public void Exit() { }
}