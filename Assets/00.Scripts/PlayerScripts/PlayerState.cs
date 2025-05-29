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
    
    protected Vector3 velocity;

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
    }
    public virtual void Exit() { }
    public virtual void HandleInput() { }

    public virtual void UpdateLogic()
    {
        isGrounded = player.isGround;
        this.velocity = player.velocity;
        ChangeState();
    }

    public virtual void UpdatePhysics()
    {
        controller.Move(velocity * Time.fixedDeltaTime);
    }

    protected virtual void ChangeState(){}
}

