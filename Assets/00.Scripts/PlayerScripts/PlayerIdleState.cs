using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine) {}
    

    protected override void ChangeState()
    {
        base.ChangeState();
        if (input.inputMove.magnitude >= 0.01f)
        {
            stateMachine.ChangeState(new PlayerMoveState(player, stateMachine));
        }

        if (input.jumpPressed && isGrounded)
        {
            stateMachine.ChangeState(new PlayerJumpState(player, stateMachine));
        }
    }
}
