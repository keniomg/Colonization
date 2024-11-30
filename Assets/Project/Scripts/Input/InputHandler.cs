using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private InputEventInvoker _invoker;

    public void OnChoosed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _invoker.InvokeChoosed();
        }
    }

    public void OnFlagSetted(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _invoker.InvokeFlagSetted();
        }
    }
}