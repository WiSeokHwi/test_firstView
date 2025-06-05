using UnityEngine;

public class PlayerHitState : PlayerState
{
    public PlayerHitState(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine) {}

    public override void Enter()
    {
        base.Enter();
        player.velocity = Vector2.zero;
        player.animator.SetFloat("VelocityX", 0);
        player.animator.SetFloat("VelocityZ", 0);
        animator.SetTrigger("IsHit");
        
    }

    public void HitEnd()
    {
        stateMachine.ChangeState(new PlayerIdleState(player, stateMachine));
    }
}
