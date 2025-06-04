using UnityEngine;

public class PlayerRollState : PlayerState
{
    public PlayerRollState(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    private float rollPower = 10f;
    private Vector3 rollDirection;

    public override void Enter()
    {
        base.Enter();
        animator.applyRootMotion = true;
        Vector3 camForward = player.camPivot.forward;
        Vector3 camRight = player.camPivot.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        rollDirection = camForward * input.inputMove.y + camRight * input.inputMove.x;
        rollDirection.Normalize();
        
        
        animator.SetTrigger("IsRoll");
        
    }

    public override void UpdateLogic()
    {

        Vector3 rollDirection = player.transform.forward;
        rollDirection.y = 0;
        
        base.UpdateLogic();
        
    }

    public override void UpdatePhysics()
    {
        RotationToCamera();
        base.UpdatePhysics();
    }


    void RotationToCamera()
    {
        

        // 목표 회전 계산
        Quaternion targetRotation = Quaternion.LookRotation(rollDirection, Vector3.up);

        // 캐릭터를 해당 방향으로 부드럽게 회전
        player.transform.rotation = Quaternion.Slerp(
            player.transform.rotation,
            targetRotation,
            Time.deltaTime * 10f);
    }

    public void RollEnd()
    {
        animator.applyRootMotion = false;
        stateMachine.ChangeState(new PlayerMoveState(player, stateMachine));
    }
}
