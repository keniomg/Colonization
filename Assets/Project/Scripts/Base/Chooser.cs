using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Chooser : MonoBehaviour
{
    private Choosable _chosen;

    public void OnChoose(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out Choosable choosable))
                {
                    Choose(choosable);
                }
                else
                {
                    Unchoose();
                }
            }
        }
    }

    private void Choose(Choosable choosable)
    {
        Unchoose();

        _chosen = choosable;
        _chosen.ChangeChosenStatus(true);
    }

    private void Unchoose()
    {
        if (_chosen != null)
        {
            _chosen.ChangeChosenStatus(false);
            _chosen = null;
        }
    }
}