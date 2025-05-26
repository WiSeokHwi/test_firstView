using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    
    private PlayerInputSystem inputActions;
    public Vector2 inputMove;

    void OnEnable()
    {
        inputActions = new PlayerInputSystem();
        inputActions.Enable();
        inputActions.Play.Move.performed += ctx => inputMove = ctx.ReadValue<Vector2>();
        inputActions.Play.Move.canceled += ctx => inputMove = Vector2.zero;
    }
}
