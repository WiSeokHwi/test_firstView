using UnityEngine;

public class PlayerMoveState : PlayerState
{
    private static readonly int VelocityX = Animator.StringToHash("VelocityX");
    private static readonly int VelocityZ = Animator.StringToHash("VelocityZ");

    public PlayerMoveState(PlayerController player, PlayerStateMachine stateMachine) :
        base(player, stateMachine) { }

    private bool isRun;
    private float speed; // 이동 속도
    private float targetSpeed; // 현재 타겟 스피드
    private float damping = 0.2f; // 속도 변화 부드럽게 하는 값
    private float targetSpeedDamping = 0.4f; // 걷기에서 뛰기 전환을 부드럽게 해주는 값
    
    Vector2 currentAnimVelocity; // 현재 애니매이션 속도
    Vector2 animVelocityVelocity; // SmoothDamp용 속도 버퍼 / 애니매이션용
    
    private float smoothTargetSpeed; // SmoothDamp용 속도 버퍼 / Walk : Run 전환용
    Vector2 smoothVelocity; // smoothDamp용 속도 버퍼 / 실제 이동속도 변환용
    
    Vector2 moveVelocity; // 현재 속도
    


    public override void Enter()
    {
        base.Enter();
        moveVelocity = Vector2.zero;
        
    }

    public override void HandleInput()
    {
        base.HandleInput();
        isRun = input.shiftPressed;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public override void UpdateLogic()
    {
        RotationToCamera();
        VelocityLogic();
        AnimationLogic();
        base.UpdateLogic();
        
    }

    void VelocityLogic()
    {
        targetSpeed = isRun ? runSpeed : walkSpeed;
        speed = Mathf.SmoothDamp(speed, targetSpeed, ref smoothTargetSpeed, targetSpeedDamping);
        
        // 카메라 기준 방향 구하기
        Vector3 camForward = player.camPivot.forward;
        Vector3 camRight = player.camPivot.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();
        

        // 입력 벡터를 카메라 방향 기준으로 변환
        Vector3 direction = camForward * input.inputMove.y + camRight * input.inputMove.x;
        Vector3 targetWorldVelocity = direction.normalized * speed;

        // moveVelocity = XZ 평면만 사용 (Vector2)
        Vector2 targetVelocity = new(targetWorldVelocity.x, targetWorldVelocity.z);
        moveVelocity = Vector2.SmoothDamp(moveVelocity, targetVelocity, ref smoothVelocity, damping);

        // 최종 월드 velocity 설정
        velocity.x = moveVelocity.x;
        velocity.z = moveVelocity.y;
        player.velocity = new Vector3(velocity.x, player.velocity.y, velocity.z);
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
            Time.deltaTime * 1f // 회전 속도 조절 (값이 클수록 빨리 회전)
        );
    }

    protected override void ChangeState()
    {
        if (input.inputMove.magnitude == 0f && moveVelocity.magnitude <= 0.01f && currentAnimVelocity.magnitude <= 0.01f)
        {
            stateMachine.ChangeState(new PlayerIdleState(player, stateMachine));
        }
        
        if (input.jumpPressed && isGrounded)
        {
            stateMachine.ChangeState(new PlayerJumpState(player, stateMachine));
        }
    }

    void AnimationLogic()
    {
        float targetAnimSpeed = isRun ? 1f : 0.5f; // 블렌드트리 걷기와 뛰기를 구현하기위한 목표값
        
        // 1. 수평 벡터만 추출
        Vector3 camForward = player.camPivot.forward;
        Vector3 camRight = player.camPivot.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        // 2. 방향 계산
        Vector3 inputWorldDir =
            (camForward * input.inputMove.y) +
            (camRight * input.inputMove.x);
        
        // 3. 월드 기준 입력을 플레이어의 로컬 기준으로 변환
        Vector3 inputLocalDir = player.transform.InverseTransformDirection(inputWorldDir);
        
        // 4. 애니메이션용 벡터 구성
        Vector2 targetAnimVelocity = new Vector2(inputLocalDir.x, inputLocalDir.z) * targetAnimSpeed;
        
        currentAnimVelocity = Vector2.SmoothDamp(
            currentAnimVelocity, 
            targetAnimVelocity, 
            ref animVelocityVelocity, 
            damping
        );
        
        player.animator.SetFloat(VelocityX, currentAnimVelocity.x);
        player.animator.SetFloat(VelocityZ, currentAnimVelocity.y);
    }

    public override void Exit()
    {
        base.Exit();
        input.inputMove= Vector2.zero;
        moveVelocity = Vector2.zero;
        currentAnimVelocity = Vector2.zero;
    }
}
