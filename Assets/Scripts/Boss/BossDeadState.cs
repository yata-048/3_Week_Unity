using UnityEngine;

public class BossDeadState : IState
{
    private Boss boss;
    private StateMachine stateMachine;

    public BossDeadState(Boss boss, StateMachine stateMachine)
    {
        this.boss = boss;
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        Debug.Log("boss da chet");
        boss.rb.linearVelocity = Vector2.zero;
        Collider2D col = boss.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
        boss.rb.bodyType = RigidbodyType2D.Static;
    }

    public void Tick() {}
    public void FixedTick() {}
    public void Exit() {}
}