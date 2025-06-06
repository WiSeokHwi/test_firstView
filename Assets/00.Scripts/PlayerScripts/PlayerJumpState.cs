using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        animator.SetTrigger("IsJump");
        
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        RotationToCamera();
    }

    protected override void ChangeState()
    {
        
        if (!player.isGround && player.velocity.y > 1f)
        {
            stateMachine.ChangeState(new PlayerFallState(player, stateMachine));
        }
    }
    
    void RotationToCamera()
    {
        Vector3 camForward = player.camPivot.forward;
        camForward.y = 0f;                     // 위아래 기울임 제거
        camForward.Normalize();               // 방향 벡터 정규화

        if (camForward.sqrMagnitude < 0.01f) return; // 너무 짧은 벡터면 회전 안 함

        Quaternion targetRotation = Quaternion.LookRotation(camForward, Vector3.up);
        player.transform.rotation = Quaternion.Slerp(
            player.transform.rotation,
            targetRotation,
            Time.deltaTime * 5f // 회전 속도 조절 (값이 클수록 빨리 회전)
        );
    }

    public override void Exit()
    {
        base.Exit();
        animator.SetBool("IsJump", false);
    }
    
    public void Jump()
    {
        player.velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
    }
    
}
