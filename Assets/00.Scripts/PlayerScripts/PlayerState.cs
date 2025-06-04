using UnityEngine;

public abstract class PlayerState
{
    protected PlayerController player;
    protected PlayerStateMachine stateMachine;
    protected PlayerInput input;
    protected CharacterController controller;
    protected Animator animator;
    
    protected bool isGrounded;
    protected float walkSpeed;
    protected float runSpeed;
    protected float jumpHeight;
    
    protected float gravity;
    
    protected Vector2 savedVelocity;

    public PlayerState(PlayerController player, PlayerStateMachine stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
    }
    
    public virtual void Enter()
    {
        this.input = player.input;
        this.controller = player.controller;
        this.animator = player.animator;
        walkSpeed = player.walkSpeed;
        runSpeed = player.runSpeed;
        gravity = player.gravity;
        jumpHeight = player.jumpHeight;
    }
    public virtual void Exit() { }
    public virtual void HandleInput() { }

    public virtual void UpdateLogic()
    {
        isGrounded = player.isGround;
        ChangeState();
    }

    public virtual void UpdatePhysics()
    {
        YVelocity();
        controller.Move(player.velocity * Time.fixedDeltaTime);
    }

    protected virtual void ChangeState()
    {
        if (!isGrounded)
        {
            stateMachine.ChangeState(new PlayerFallState(player, stateMachine));
        }
    }
    
    // 중력 처리 메소드
    void YVelocity()
    {
        if (stateMachine.CurrentState is PlayerJumpState)
            return;

        if (isGrounded)
        {
            if (player.velocity.y <= 0)
            {
                player.velocity.y = -2f;
            }
        }
        else
        {
            player.velocity.y += gravity * Time.fixedDeltaTime;
        }
    }
}

