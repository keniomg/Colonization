using UnityEngine;
using UnityEngine.InputSystem;

public class Chooser : MonoBehaviour
{
    private Choosable _chosen;
    private Camera _mainCamera;
    private PlayerInputHandler _playerInputHandler;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _playerInputHandler = new();
    }

    private void OnEnable()
    {
        _playerInputHandler.Enable();
        _playerInputHandler.Player.Choosed.performed += OnChoosed;
    }

    private void OnDisable()
    {
        _playerInputHandler.Disable();
        _playerInputHandler.Player.Choosed.performed -= OnChoosed;
    }

    public void OnChoosed(InputAction.CallbackContext context)
    {
        Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

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

    private void Choose(Choosable choosable)
    {
        Unchoose();

        _chosen = choosable;
        _chosen.Choose();
    }

    private void Unchoose()
    {
        if (_chosen != null)
        {
            _chosen.Unchoose();
            _chosen = null;
        }
    }
}