using Unity.Mathematics;
using UnityEditor.Callbacks;
using UnityEngine;

public class PlayerRollingState : IState
{
    private Player player;
    private StateMachine stateMachine;
    private float timer;
    private float rollDirection;
    private int playerLayer;
    private int bossLayer;
    public PlayerRollingState(Player player, StateMachine stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        playerLayer = LayerMask.NameToLayer("Player");
        bossLayer = LayerMask.NameToLayer("Boss");
    }
    
    public void Enter()
    {
        player.isInvincible = true;
        Debug.Log("dang bat I-Frame!");
        timer = player.rollDuration;
        Physics2D.IgnoreLayerCollision(playerLayer, bossLayer, true);
        // if(Mathf.Abs(player.moveDirection) > 0)
        // {
        //     rollDirection = Mathf.Sign(player.moveDirection);
        //     player.Flip();
        // }
        // else
        // {
        //     rollDirection = -Mathf.Sign(player.transform.localScale.x);
        // }
        player.rb.linearVelocity = new Vector2(player.rollSpeed * player.moveDirection, 0f);
        player.Anim.SetTrigger("Roll");
    }
    public void Tick()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            if(Mathf.Abs(player.moveDirection) > 0)
            {
                stateMachine.ChangeState(player.MoveState);
            }
            else
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
    }
    public void FixedTick() {}
    public void Exit()
    {
        player.isInvincible = false;
        player.lastRollTime = Time.time;
        Debug.Log("het I-frame!");
        Physics2D.IgnoreLayerCollision(playerLayer, bossLayer, false);
        player.rb.linearVelocity = new Vector2(0f, player.rb.linearVelocity.y);
    }

}
