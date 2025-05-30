using UnityEngine;

public class PlayerRollState : PlayerState
{
    public PlayerRollState(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        animator.SetTrigger("IsRoll");
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        RotationToCamera();
    }
    

    void RotationToCamera()
    {
        Vector3 camForward = player.camPivot.forward;
        camForward.y = 0f;                     // 위아래 기울임 제거
        camForward.Normalize();               // 방향 벡터 정규화

        if (camForward.sqrMagnitude < 0.01f) return; // 너무 짧은 벡터면 회전 안 함

        Quaternion targetRotation = Quaternion.LookRotation(camForward, Vector3.up); // 카메라의 정면을 y축 기준으로 저장
        
        // 캐릭터 회전
        player.transform.rotation = Quaternion.Slerp(
            player.transform.rotation,
            targetRotation,
            Time.deltaTime * 5f // 회전 속도 조절 (값이 클수록 빨리 회전)
        );
    }

    public void RollEnd()
    {
        stateMachine.ChangeState(new PlayerIdleState(player, stateMachine));
    }
}
