using UnityEngine;

public class PlayerParryState : IState
{
    private Player player;
    private StateMachine stateMachine;
    private float timer;
    public PlayerParryState(Player player, StateMachine stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        player.rb.linearVelocity = new Vector2(0f, player.rb.linearVelocity.y);
        player.isParry = true;
        Debug.Log("parry");
        timer = player.parryDuration;
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
        player.isParry = false;
        Debug.Log("het parry");
    }
}
