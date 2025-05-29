using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    
    private PlayerInputSystem inputActions;
    public Vector2 inputMove;
    public bool shiftPressed;
    public Vector2 inputAim;
    public bool jumpPressed;

    void OnEnable()
    {
        inputActions = new PlayerInputSystem();
        inputActions.Enable();
        inputActions.Play.Move.performed += ctx => inputMove = ctx.ReadValue<Vector2>();
        inputActions.Play.Move.canceled += ctx => inputMove = Vector2.zero;
        inputActions.Play.ShiftPress.performed += ctx => shiftPressed = ctx.ReadValueAsButton();
        inputActions.Play.ShiftPress.canceled += ctx => shiftPressed = false;
        inputActions.Play.CamRotation.performed += ctx => inputAim = ctx.ReadValue<Vector2>();
        inputActions.Play.CamRotation.canceled += ctx => inputAim = Vector2.zero;
        inputActions.Play.JumpPress.performed += ctx => jumpPressed = ctx.ReadValueAsButton();
        inputActions.Play.JumpPress.canceled += ctx => jumpPressed = false;
    }
}
