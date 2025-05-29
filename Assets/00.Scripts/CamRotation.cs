using System;
using UnityEngine;

public class CamRotation : MonoBehaviour
{
    public PlayerController target; // 플레이어 연결
    private PlayerInput input; // 인풋 가져오기
    
    public float mouseSensitivity = 2f; // 마우스 민감도
    
    private float yaw;
    private float pitch;

    private void Start()
    {
        input = target.input;
    }

    void Update()
    {
        Vector2 aimInput = input.inputAim;
        
        yaw += aimInput.x * mouseSensitivity;
        pitch += -aimInput.y * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -40f, 70f);
        
        transform.rotation = Quaternion.Euler(pitch, yaw, 0);
    }
}
