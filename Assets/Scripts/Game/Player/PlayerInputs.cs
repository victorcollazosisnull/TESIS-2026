using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInputs : MonoBehaviour
{
    public static event Action<Vector2> moveInput;
    public static event Action<Vector2> mouseInputAction;
    public static event Action interactInput;
    public static event Action pauseInput;
    public static event Action activatePanelInput;

    public void ReadMovement(InputAction.CallbackContext context)
    {
        moveInput?.Invoke(context.ReadValue<Vector2>());
    }

    public void ReadCameraMovement(InputAction.CallbackContext context)
    {
        mouseInputAction?.Invoke(context.ReadValue<Vector2>());
    }

    public void ReadInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interactInput?.Invoke();
        }
    }

    public void ReadPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            pauseInput?.Invoke();
        }
    }
    public void ReadActivatePanel(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            activatePanelInput?.Invoke();
        }
    }
}