using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class PlayerInput : MonoBehaviour
{
    
    private PlayerInputSystem inputActions;
    public Vector2 inputMove => inputActions?.Play.Move.ReadValue<Vector2>() ?? Vector2.zero;
    public Vector2 inputAim => inputActions?.Play.CamRotation.ReadValue<Vector2>() ?? Vector2.zero;
    public bool shiftPressed => inputActions?.Play.ShiftPress.ReadValue<float>() > 0.5f;
    public bool jumpPressed { get; private set; }
    
    public bool shiftDoubleTapped { get; private set; }

    void OnEnable()
    {
        inputActions = new PlayerInputSystem();
        inputActions.Enable();
        inputActions.Play.JumpPress.performed += ctx => jumpPressed = true;
        inputActions.Play.JumpPress.canceled += ctx => jumpPressed = false;
        inputActions.Play.ShiftDoubleTap.performed += ctx =>
        {
            if (ctx.interaction is MultiTapInteraction)
            {
                shiftDoubleTapped = true;
                
                // 타이밍 맞춰서 다시 false로 리셋해줘야 함
                Invoke(nameof(ResetDoubleTap), 0.1f);
            }
        };
    }
    void ResetDoubleTap()
    {
        shiftDoubleTapped = false;
    }

    void OnDisable()
    {
        inputActions?.Disable();
    }
}
