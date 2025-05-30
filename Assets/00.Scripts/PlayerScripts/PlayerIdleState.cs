using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine) {}

    protected override void ChangeState()
    {
        base.ChangeState();
        
        // 방향키 입력이 있으면 Move로 전환
        if (input.inputMove.magnitude >= 0.01f)
        {
            stateMachine.ChangeState(new PlayerMoveState(player, stateMachine));
        }
        
        // 점프키 입력이 있으면 Jump로 전환
        if (input.jumpPressed && isGrounded)
        {
            stateMachine.ChangeState(new PlayerJumpState(player, stateMachine));
        }
    }
}
