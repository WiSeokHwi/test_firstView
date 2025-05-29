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
    
    PlayerStateMachine stateMachine;
    
    
    [ReadOnly]
    [SerializeField]
    public Vector3 velocity;
    
    // 값 저장하는 변수
    public bool isGround => controller.isGrounded;
    public bool jumping = false;
    
    // 플레이어 속도, 점프 등 수치
    [SerializeField] public float gravity = -9.81f; // CharacterController 중력 구현을 위한 중력값
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
    }

    private void Start()
    {
        stateMachine.Initialize(new PlayerIdleState(this, stateMachine));
    }

    void Update()
    {
        animator.SetBool("IsGround", isGround);
        YVelocity();
        stateMachine.CurrentState.HandleInput();
        stateMachine.CurrentState.UpdateLogic();
        Debug.Log("플레이어 상태" + stateMachine.CurrentState);

    }

    void FixedUpdate()
    {
        stateMachine.CurrentState.UpdatePhysics();
    }

    void YVelocity()
    {
        if (jumping && isGround)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumping = false;
        }
        else if (isGround) // 땅에 닿아있고 y의 속도가 가 0보다 작으면 
        {
            if (velocity.y <= 0)
            {
                velocity.y = -2f;
            }
        }
        
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
    }

    void JumpingAnimEvent()
    {
        jumping = true;
    }
}
