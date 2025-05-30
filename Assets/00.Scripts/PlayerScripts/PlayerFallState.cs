using UnityEngine;

public class PlayerFallState : PlayerState
{
    public PlayerFallState(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    protected override void ChangeState()
    {
        if (isGrounded && player.velocity.y <= -0.1f)
        {
            stateMachine.ChangeState(new PlayerIdleState(player, stateMachine));
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.velocity = Vector3.zero;
        player.controller.Move(Vector3.zero);
        player.animator.SetFloat("VelocityX", 0);
        player.animator.SetFloat("VelocityZ", 0);
    }
}
