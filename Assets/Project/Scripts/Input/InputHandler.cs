using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private InputEventInvoker _invoker;

    public void OnLeftMouseClicked(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _invoker.InvokeLeftMouseClicked();
        }
    }

    public void OnRightMouseClicked(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _invoker.InvokeRightMouseClicked();
        }
    }
}