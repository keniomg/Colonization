using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlagSetter : MonoBehaviour
{
    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private LayerMask _buildingsLayer;
    [SerializeField] private LayerMask _wallsLayer;

    private Choosable _choosable;
    private float _requiredArea;
    private Flag _flag;
    private BuildingEventInvoker _buildingEventInvoker;
    private MeshRenderer[] _flagMeshRenderers;
    private Camera _mainCamera;
    private PlayerInputHandler _playerInputHandler;

    public event Action FlagStatusChanged;

    public Flag Flag { get; private set; }

    public void Initialize(Base owner, Choosable choosable)
    {
        _playerInputHandler = new();
        _mainCamera = Camera.main;
        _playerInputHandler.Player.FlagSetted.performed += OnFlagSetted;
        Flag = null;
        _flag = Instantiate(_flagPrefab);
        _flag.CollidedWithPreview += UnsetFlag;
        _flag.gameObject.SetActive(false);
        _flagMeshRenderers = _flag.GetComponentsInChildren<MeshRenderer>();
        _requiredArea = owner.Building.OccupiedZoneRadius;
        _choosable = choosable;
        _choosable.Choosed += OnChoosed;
        _choosable.Unchoosed += OnUnchoosed;
        _buildingEventInvoker = owner.BuildingEventInvoker;
        _buildingEventInvoker.BuildingPlanned += UnsetFlag;
    }

    private void OnDestroy()
    {
        _playerInputHandler.Player.FlagSetted.performed -= OnFlagSetted;
        _choosable.Choosed -= OnChoosed;
        _choosable.Unchoosed -= OnUnchoosed;
        _flag.CollidedWithPreview -= UnsetFlag;
        _buildingEventInvoker.BuildingPlanned -= UnsetFlag;
    }

    public void OnFlagSetted(InputAction.CallbackContext context)
    {
        Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Physics.DefaultRaycastLayers))
        {
            if (hit.collider.TryGetComponent(out Flag flag) && flag.gameObject == _flag.gameObject)
            {
                UnsetFlag();
            }
            else if (GetFlagSetAvailable(hit.point))
            {
                SetFlag(hit.point);
            }
        }
    }

    private void OnChoosed()
    {
        _playerInputHandler.Enable();
        SetFlagVisible();
    }

    private void OnUnchoosed()
    {
        _playerInputHandler.Disable();
        SetFlagInvisible();
    }

    private void SetFlagVisible()
    {
        foreach (MeshRenderer meshRenderer in _flagMeshRenderers)
        {
            meshRenderer.enabled = true;
        }
    }

    private void SetFlagInvisible()
    {
        foreach (MeshRenderer meshRenderer in _flagMeshRenderers)
        {
            meshRenderer.enabled = false;
        }
    }

    private bool GetFlagSetAvailable(Vector3 areaCenter)
    {
        return Physics.OverlapSphere(areaCenter, _requiredArea, _buildingsLayer).Length == 0
            && Physics.OverlapSphere(areaCenter, _requiredArea, _wallsLayer).Length == 0;
    }

    private void SetFlag(Vector3 flagPosition)
    {
        _flag.gameObject.SetActive(true);
        _flag.transform.position = flagPosition;
        Flag = _flag;
        FlagStatusChanged?.Invoke();
    }

    private void UnsetFlag()
    {
        _flag.gameObject.SetActive(false);
        Flag = null;
        FlagStatusChanged?.Invoke();
    }
}