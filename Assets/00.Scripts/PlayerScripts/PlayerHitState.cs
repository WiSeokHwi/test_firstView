using UnityEngine;

public class PlayerHitState : PlayerState
{
    public PlayerHitState(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine) {}

    public override void Enter()
    {
        base.Enter();
        animator.SetTrigger("IsHit");
        
    }

    public override void UpdatePhysics()
    {
        
    }
}
