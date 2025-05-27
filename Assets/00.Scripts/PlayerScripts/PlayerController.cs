using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

// 캐릭터 
public class PlayerController : MonoBehaviour
{
    // 필요한 컴포넌트 변수
    private CharacterController controller;
    private PlayerInput input;
    
    [ReadOnly]
    [SerializeField]
    private Vector3 velocity;
    
    // 값 저장하는 변수
    private bool isGrounded;
    private Vector2 moveVelocity;
    private bool isRun;
    private float speed; // 플레이어 이동속도

    private float smoothTargetSpeed;
    Vector2 smoothVelocity;
    
    // 플레이어 속도, 점프 등 수치
    [SerializeField] private float gravity = -9.81f; // CharacterController 중력 구현을 위한 중력값
    [SerializeField] private float workSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float damping = 0.25f; // 속도 변화 부드럽게 하는 값
    [SerializeField] private float targetSpeedDamping = 0.8f;
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
        float targetSpeed = isRun ? runSpeed : workSpeed;
        
        speed = Mathf.SmoothDamp(speed, targetSpeed, ref smoothTargetSpeed, targetSpeedDamping);

        Vector2 targetVelocity = input.inputMove;
        moveVelocity = Vector2.SmoothDamp(moveVelocity, targetVelocity, ref smoothVelocity, damping);
        
        velocity.x = moveVelocity.x;
        velocity.z = moveVelocity.y;
        
        Debug.Log(moveVelocity.magnitude * speed);
        
    }

    void YVelocity()
    {
        if (isGrounded) // 땅에 닿아있고 y의 속도가 가 0보다 작으면 
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
}
