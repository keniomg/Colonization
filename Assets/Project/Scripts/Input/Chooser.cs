using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Chooser : MonoBehaviour
{
    [SerializeField] private InputEventInvoker _inputEventInvoker;

    private Choosable _chosen;
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        _inputEventInvoker.Choosed += OnChoosed;
    }

    private void OnDisable()
    {
        _inputEventInvoker.Choosed -= OnChoosed;
    }

    public void OnChoosed()
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