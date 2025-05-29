using UnityEngine;

public class PlayerFallState : PlayerState
{
    public PlayerFallState(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    protected override void ChangeState()
    {
        base.ChangeState();
        if (isGrounded)
        {
            stateMachine.ChangeState(new PlayerIdleState(player, stateMachine));
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.velocity = Vector2.zero;
        player.animator.SetFloat("VelocityX", 0);
        player.animator.SetFloat("VelocityZ", 0);
    }
}
