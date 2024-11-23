using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlagSetter : MonoBehaviour
{
    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private LayerMask _buildingsLayer;

    private Choosable _choosable;
    private PlayerInput _playerInput;
    private float _requiredRadius;
    private Flag _flag;

    public event Action FlagStatusChanged;

    public Flag Flag { get; private set; }

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();

        if (_playerInput != null)
        {
            _playerInput.Disable(); // Отключаем ввод по умолчанию
        }

        Flag = null;
        _flag = Instantiate(_flagPrefab);
        _flag.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _choosable.Choosed -= OnChoosed;
    }

    public void Initialize(Base owner, Choosable choosable)
    {
        _requiredRadius = owner.Building.OccupiedZoneRadius;
        _choosable = choosable;
        _choosable.Choosed += OnChoosed;
    }

    public void OnSetFlag(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out Map map) && GetFlagSetAvailable(hit.point))
                {
                    SetFlag(hit.point);
                }
                else if (hit.collider.TryGetComponent(out Flag flag) && flag.gameObject == _flag.gameObject)
                {
                    UnsetFlag();
                }
            }
        }
    }

    private void OnChoosed(bool isChosen)
    {
        if (isChosen)
        {
            _playerInput.Enable();
        }
        else
        {
            _playerInput.Disable();
        }
    }

    private bool GetFlagSetAvailable(Vector3 areaCenter)
    {
        if (Physics.OverlapSphere(areaCenter, _requiredRadius, _buildingsLayer) != null)
        {
            return true;
        }

        return false;
    }

    private void SetFlag(Vector3 flagPosition)
    {
        _flag.gameObject.SetActive(true);
        _flag.transform.position = flagPosition;
        FlagStatusChanged?.Invoke();
        Flag = _flag;
    }

    private void UnsetFlag()
    {
        _flag.gameObject.SetActive(false);
        FlagStatusChanged?.Invoke();
        Flag = null;
    }
}