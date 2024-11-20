using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlagSetter : MonoBehaviour
{
    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private LayerMask _buildingsLayer;

    private float _requiredRadius;
    private Flag _flag;

    public event Action FlagStatusChanged;

    public Flag Flag { get; private set; }

    private void Awake()
    {
        _flag = Instantiate(_flagPrefab);
        _flag.gameObject.SetActive(false);
    }

    public void Initialize(Base owner)
    {
        _requiredRadius = owner.OccupiedZoneRadius;
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