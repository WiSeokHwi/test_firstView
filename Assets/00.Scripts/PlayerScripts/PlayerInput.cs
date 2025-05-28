using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    
    private PlayerInputSystem inputActions;
    public Vector2 inputMove;
    public bool shiftPressed;

    void OnEnable()
    {
        inputActions = new PlayerInputSystem();
        inputActions.Enable();
        inputActions.Play.Move.performed += ctx => inputMove = ctx.ReadValue<Vector2>();
        inputActions.Play.Move.canceled += ctx => inputMove = Vector2.zero;
        inputActions.Play.ShiftPress.performed += ctx => shiftPressed = ctx.ReadValueAsButton();
        inputActions.Play.ShiftPress.canceled += ctx => shiftPressed = false;
    }
}
