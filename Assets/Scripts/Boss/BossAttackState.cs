using UnityEngine;

public class BossAttackState : IState
{
    private Boss boss;
    private StateMachine stateMachine;
    private float timer;
    private bool hasDealtDamage;
    private bool isHitboxActive;

    public BossAttackState(Boss boss, StateMachine stateMachine)
    {
        this.boss = boss;
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        timer = boss.attackDuration;
        hasDealtDamage = false;
        isHitboxActive = false;
        boss.rb.linearVelocity = new Vector2(0f, boss.rb.linearVelocity.y);
        Debug.Log("Boss: Vung kiem chem!");
        boss.Anim.SetTrigger("Attack");
    }

    public void Tick()
    {
        timer -= Time.deltaTime;
        if (!isHitboxActive && boss.player != null)
        {
            float dir = Mathf.Sign(boss.player.transform.position.x - boss.transform.position.x);
            boss.Flip(dir);
        }
        if (!hasDealtDamage && isHitboxActive)
        {
            DealDamage();
        }
        if (timer <= 0f)
        {
            boss.lastAttackTime = Time.time;
            stateMachine.ChangeState(boss.ChaseState);
        }
    }
    public void OpenAttackHitBox()
    {
        isHitboxActive = true;
    }
    public void CloseAttackHitBox()
    {
        isHitboxActive = false;
    }


    private void DealDamage()
    {
        Collider2D hit = Physics2D.OverlapBox(boss.GetAttackBoxCenter(), boss.attackBoxSize, 0f, boss.playerLayer);
        if (hit != null)
        {
            Player player = hit.GetComponent<Player>();
            if (player == null) return;
            if (player.isParry)
            {
                Debug.Log("Parry thành công !Boss bị ngắt chiêu!");
                stateMachine.ChangeState(boss.GetHitState);
                boss.TakeDamage(boss.bossDamageTaken);
                return;
            }
            if (player.isInvincible)
            {
                // Debug.Log("Player Roll né đòn thành công!");
                return;
            }
            hasDealtDamage = true;
            player.TakeDamage(boss.attackDamage);
    
        }
    }


    public void FixedTick() {}
    public void Exit()
    {
        hasDealtDamage = false;
        isHitboxActive = false;
    }

    
}