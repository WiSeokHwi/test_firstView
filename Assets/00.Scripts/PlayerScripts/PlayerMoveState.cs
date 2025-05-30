using UnityEngine;

public class PlayerMoveState : PlayerState
{
    private static readonly int VelocityX = Animator.StringToHash("VelocityX");
    private static readonly int VelocityZ = Animator.StringToHash("VelocityZ");

    public PlayerMoveState(PlayerController player, PlayerStateMachine stateMachine) :
        base(player, stateMachine) { }

    private bool isRun; // 걷기 : 뛰기 체크
    
    private float damping = 0.2f; // 속도 변화 부드럽게 하는 값
    private float animDamiping = 0.2f; // 애니메이션 댐핑
    private float targetSpeedDamping = 0.4f; // 걷기에서 뛰기 전환을 부드럽게 해주는 값
    
    // SmoothDamp사용을 위한 현재 값 저장하는 변수
    Vector2 currentAnimVelocity; // 현재 애니매이션 속도
    private float curruntAnimTarget; // 현재 타겟 Anim
    private float currentTargetSpeed; // 현재 이동 속도
    private float targetSpeed; // 현재 타겟 스피드
    Vector2 moveVelocity; // 현재 속도 
    
    
    // SmoothDamp용 버퍼 변수
    Vector2 animVelocityVelocity; // SmoothDamp용 속도 버퍼 / 애니매이션용
    private float smoothAnimTargetSpeed; // SmoothDamp용 속도 버퍼 / 애니매이션 타겟 스피드 전환용
    private float smoothTargetSpeed; // SmoothDamp용 속도 버퍼 / Walk : Run 전환용
    Vector2 smoothVelocity; // smoothDamp용 속도 버퍼 / 실제 이동속도 변환용
    
    
    


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
        targetSpeed = isRun ? runSpeed : walkSpeed; // 쉬프트키 입력에 따른 타겟 스피드 값 변경
        
        // SmoothDamp로 부드럽게 보간하여 타겟스피드 변환시 부드럽게 만듬
        currentTargetSpeed = Mathf.SmoothDamp(currentTargetSpeed, targetSpeed, ref smoothTargetSpeed, targetSpeedDamping);
        
        // 카메라 기준 방향 구하기
        Vector3 camForward = player.camPivot.forward;
        Vector3 camRight = player.camPivot.right;
        
        // y값이 반영되면 안되기에 제로화 및 정규화
        camForward.y = 0f; 
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();
        

        // 입력 벡터를 카메라 방향 기준으로 변환
        Vector3 direction = camForward * input.inputMove.y + camRight * input.inputMove.x;
        Vector3 targetWorldVelocity = direction.normalized * currentTargetSpeed;

        // moveVelocity = XZ 평면만 사용 (Vector2)
        Vector2 targetVelocity = new(targetWorldVelocity.x, targetWorldVelocity.z);
        moveVelocity = Vector2.SmoothDamp(moveVelocity, targetVelocity, ref smoothVelocity, damping);

        // 최종 월드 velocity 설정
        player.velocity.x = moveVelocity.x;
        player.velocity.z = moveVelocity.y;
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
            Time.deltaTime * 2f // 회전 속도 조절 (값이 클수록 빨리 회전)
        );
    }

    protected override void ChangeState()
    {
        
        // 키 입력이 없고 이동값이 없고 애니매이션파라매터 값이 없으면
        if (input.inputMove.magnitude == 0f && moveVelocity.magnitude <= 0.01f && currentAnimVelocity.magnitude <= 0.01f)
        {
            stateMachine.ChangeState(new PlayerIdleState(player, stateMachine));
        }
        
        // 점프키 입력, 지면에 닿아있으면
        if (input.jumpPressed && isGrounded)
        {
            stateMachine.ChangeState(new PlayerJumpState(player, stateMachine));
        }

        if (input.shiftDoubleTapped)
        {
            stateMachine.ChangeState(new PlayerRollState(player, stateMachine));
        }
    }

    void AnimationLogic()
    {
        float targetAnimSpeed = isRun ? 1f : 0.5f; // 블렌드트리 걷기와 뛰기를 구현하기위한 목표값
        
        curruntAnimTarget = Mathf.SmoothDamp(curruntAnimTarget, targetAnimSpeed, ref smoothAnimTargetSpeed, targetSpeedDamping);
        
        //============= 블랜드트리 방향을 맞추기 위해 입력값을 local방향으로 전환
        // 카메라 기준 방향 저장
        Vector3 camForward = player.camPivot.forward;
        Vector3 camRight = player.camPivot.right;

        // y값 초기화 및 정규화
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        // 방향 계산  *인풋값은 그냥 키입력에 따른 앞 뒤 옆이기때문에 기준이 없음
        Vector3 inputCamDir =
            (camForward * input.inputMove.y) + //인풋 일렵값을 카매라 방향으로 전환
            (camRight * input.inputMove.x);
        
        // 월드 기준 입력을 플레이어의 로컬 기준으로 변환
        Vector3 inputLocalDir = player.transform.InverseTransformDirection(inputCamDir);
        
        // 애니메이션용 벡터 구성
        Vector2 targetAnimVelocity = new Vector2(inputLocalDir.x, inputLocalDir.z) * curruntAnimTarget;
        
        currentAnimVelocity = Vector2.SmoothDamp(
            currentAnimVelocity, 
            targetAnimVelocity, 
            ref animVelocityVelocity, 
            animDamiping
        );
        
        player.animator.SetFloat(VelocityX, currentAnimVelocity.x);
        player.animator.SetFloat(VelocityZ, currentAnimVelocity.y);
    }

    public override void Exit()
    {
        base.Exit();
        moveVelocity = Vector2.zero;
        currentAnimVelocity = Vector2.zero;
    }
}
