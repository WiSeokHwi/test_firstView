using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(PlayerController player, PlayerStateMachine stateMachine) :
        base(player, stateMachine) { }

    private bool isRun;
    private float speed; // 실제 속도
    private float damping = 0.2f; // 속도 변화 부드럽게 하는 값
    private float targetSpeedDamping = 0.4f; // 걷기에서 뛰기 전환을 부드럽게 해주는 값
    private float smoothTargetSpeed; // SmoothDamp를 위한 currentVelocity값
    Vector2 smoothVelocity; // SmoothDamp를 위한 currentVelocity값
    Vector2 moveVelocity;


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
        VelocityLogic();
        base.UpdateLogic();
        
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        controller.Move(velocity * Time.fixedDeltaTime);
    }

    void VelocityLogic()
    {
        float targetSpeed = isRun ? runSpeed : walkSpeed;
        
        speed = Mathf.SmoothDamp(speed, targetSpeed, ref smoothTargetSpeed, targetSpeedDamping);

        Vector2 targetVelocity = input.inputMove.normalized * speed;
        moveVelocity = Vector2.SmoothDamp(moveVelocity, targetVelocity, ref smoothVelocity, damping);
        
        velocity.x = moveVelocity.x;
        velocity.z = moveVelocity.y;
        player.velocity = new(velocity.x,player.velocity.y ,velocity.z);
    }

    protected override void ChangeState()
    {
        if (input.inputMove.magnitude == 0f && moveVelocity.magnitude <= 0.01f)
        {
            stateMachine.ChangeState(new PlayerIdleState(player, stateMachine));
        }
    }
}
