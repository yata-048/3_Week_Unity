using UnityEngine;

public class BossTakeHitState : IState
{
    private Boss boss;
    private StateMachine stateMachine;
    private float timer;

    public BossTakeHitState(Boss boss, StateMachine stateMachine)
    {
        this.boss = boss;
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        timer = boss.hurtDuration;
        float knockbackDir = -Mathf.Sign(boss.transform.localScale.x);
        boss.Anim.SetTrigger("TakeHit");
        boss.rb.linearVelocity = new Vector2(knockbackDir * 2f, boss.rb.linearVelocity.y);
        Debug.Log("Boss trung don!");
    }

    public void Tick()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            stateMachine.ChangeState(boss.ChaseState);
        }
    }

    public void FixedTick() {}

    public void Exit()
    {
        boss.rb.linearVelocity = new Vector2(0f, boss.rb.linearVelocity.y);
    }
}