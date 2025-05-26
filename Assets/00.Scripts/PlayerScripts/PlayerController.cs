using System;
using UnityEngine;
using UnityEngine.InputSystem;

// 캐릭터 
public class PlayerController : MonoBehaviour
{
    // 필요한 컴포넌트 변수
    private CharacterController controller;
    private PlayerInput input;
    
    // 값 저장하는 변수
    private bool isGrounded;
    private Vector3 velocity;
    private Vector2 moveVelocity;
    private bool isRun;
    private float speed; // 플레이어 이동속도
    
    // 플레이어 속도, 점프 등 수치
    [SerializeField] private float gravity = -9.81f; // CharacterController 중력 구현을 위한 중력값
    [SerializeField] private float workSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float damping = 1f; // 속도 변화 부드럽게 하는 값
    [SerializeField] private float jumpHeight = 3f; // 점프 힘

    private void Awake()
    {
        // 캐릭터 컨트롤러 컴포넌트 가져오기
        controller = GetComponent<CharacterController>();
        input = GetComponent<PlayerInput>();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRun = true;
        }
        else
        {
            isRun = false;
        }
        Velocity();
        
        controller.Move(velocity * (speed * Time.deltaTime));
    }

    void Velocity()
    {
        YVelocity();
        speed = isRun ? runSpeed : workSpeed;
        moveVelocity = Vector2.Lerp(moveVelocity, input.inputMove, damping * Time.deltaTime);
        velocity = new (moveVelocity.x, velocity.y, moveVelocity.y);
    }

    void YVelocity()
    {
        if (isGrounded && velocity.y <= 0) // 땅에 닿아있고 y의 속도가 가 0보다 작으면 
        {
            velocity.y = -2f;
        }
        
        velocity.y += gravity * Time.deltaTime;
    }
}
