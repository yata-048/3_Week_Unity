using UnityEngine;

public class PlayerTakeHitState : IState
{
    private Player player;
    private StateMachine stateMachine;
    private float timer;

    public PlayerTakeHitState(Player player, StateMachine stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        timer = player.hurtDuration;
        player.Anim.SetTrigger("TakeHit");

        if (player.knockbackDir != player.boss.knockbackDir)
        {
            player.rb.linearVelocity = new Vector2(-player.knockbackDir * player.knockbackSpeed, player.knockbackJumpSpeed);
        }
        else
        {
            player.rb.linearVelocity = new Vector2(player.knockbackDir * player.knockbackSpeed, player.knockbackJumpSpeed);
        }
    }

    public void Tick()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }

    public void FixedTick() { }

    public void Exit()
    {
        player.rb.linearVelocity = new Vector2(0f, player.rb.linearVelocity.y);
    }
}
