using UnityEngine;

public class BossMoveState : IState
{
    private Boss boss;
    private StateMachine stateMachine;
    private float direction;

    public BossMoveState(Boss boss, StateMachine stateMachine)
    {
        this.boss = boss;
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        
    }

    public void Tick()
    {
        if (boss.player == null) return;
        float distanceToPlayer = Mathf.Abs(boss.player.position.x - boss.transform.position.x);
        direction = Mathf.Sign(boss.player.position.x - boss.transform.position.x);
        boss.Flip(direction);
        if (distanceToPlayer <= boss.attackRange && boss.CanAttack())
        {
            stateMachine.ChangeState(boss.AttackState);
            return;
        }
    }

    public void FixedTick()
    {
        if (boss.player == null) return;

        float distanceToPlayer = Mathf.Abs(boss.player.position.x - boss.transform.position.x);
        if (distanceToPlayer > boss.attackRange)
        {
            boss.rb.linearVelocity = new Vector2(direction * boss.chaseSpeed, boss.rb.linearVelocity.y);
        }
        else
        {
            boss.rb.linearVelocity = new Vector2(0f, boss.rb.linearVelocity.y);
        }
    }

    public void Exit()
    {
        boss.rb.linearVelocity = new Vector2(0f, boss.rb.linearVelocity.y);
    }
}