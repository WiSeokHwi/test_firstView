using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        animator.SetBool("IsJump", true);
    }

    protected override void ChangeState()
    {
        base.ChangeState();
        
        if (!player.isGround && player.velocity.y > 0f)
        {
            stateMachine.ChangeState(new PlayerFallState(player, stateMachine));
        }
    }

    public override void Exit()
    {
        base.Exit();
        animator.SetBool("IsJump", false);
    }
}
