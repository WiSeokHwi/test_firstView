using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

// 캐릭터 
public class PlayerController : MonoBehaviour
{
    // 필요한 컴포넌트 변수
    public CharacterController controller;
    public PlayerInput input;
    public Animator animator;
    public Transform camPivot;
    private GroundChecker groundChecker;
    
    PlayerStateMachine stateMachine;
    
    // 값 저장하는 변수
    public bool isGround;
    public Vector3 velocity;
    
    // 플레이어 속도, 점프 등 수치
    [SerializeField] public float gravity = -20f; // CharacterController 중력 구현을 위한 중력값
    [SerializeField] public float walkSpeed = 3f;
    [SerializeField] public float runSpeed = 6f;
    [SerializeField] public float jumpHeight = 2f; // 점프 힘

    private void Awake()
    {
        
        // 캐릭터 컨트롤러 컴포넌트 가져오기
        controller = GetComponent<CharacterController>();
        input = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        stateMachine = new PlayerStateMachine();
        groundChecker = GetComponentInChildren<GroundChecker>();
    }

    private void Start()
    {
        stateMachine.Initialize(new PlayerIdleState(this, stateMachine));
    }

    void Update()
    {
        isGround = groundChecker.IsGrounded();
        animator.SetBool("IsGround", isGround);
        
        stateMachine.CurrentState.HandleInput();
        stateMachine.CurrentState.UpdateLogic();
        Debug.Log("플레이어 상태" + stateMachine.CurrentState);
        Debug.Log("그라운드" + isGround);
        

    }

    void FixedUpdate()
    {
        stateMachine.CurrentState.UpdatePhysics();
    }
    
    public void ApplyJumpForce()
    {
        if (stateMachine.CurrentState is PlayerJumpState jumpState)
        {
            jumpState.Jump();
        }
    }

    public void EndRoll()
    {
        if (stateMachine.CurrentState is PlayerRollState rollState)
        {
            rollState.RollEnd();
        }
    }
}
