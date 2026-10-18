using UnityEngine;

public class PlayerAttackState : IState
{
    private Player player;
    private StateMachine stateMachine;
    private float timer;

    public PlayerAttackState(Player player, StateMachine stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        player.rb.linearVelocity = new Vector2(0f, player.rb.linearVelocity.y);
        Debug.Log("attack start");
        timer = player.attackDuration;
    }

    public void Tick()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            float moveInput = player.InputActions.Player.Move.ReadValue<float>();
            if (Mathf.Abs(moveInput) > 0.05f)
                stateMachine.ChangeState(player.MoveState);
            else
                stateMachine.ChangeState(player.IdleState);
        }
    }

    public void FixedTick() { }

    public void Exit()
    {
        Debug.Log("attack end");
    }
}