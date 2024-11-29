using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlagSetter : MonoBehaviour
{
    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private LayerMask _buildingsLayer;
    [SerializeField] private LayerMask _wallsLayer;
    [SerializeField] private InputEventInvoker _inputEventInvoker;

    private Choosable _choosable;
    private float _requiredArea;
    private Flag _flag;
    private BuildingEventInvoker _buildingEventInvoker;
    private MeshRenderer[] _flagMeshRenderers;

    public event Action FlagStatusChanged;

    public Flag Flag { get; private set; }

    private void OnDisable()
    {
        _choosable.Choosed -= OnChoosed;
        _flag.CollidedWithPreview -= UnsetFlag;
        _buildingEventInvoker.BuildingPlanned -= UnsetFlag;
    }

    public void Initialize(Base owner, Choosable choosable)
    {
        Flag = null;
        _flag = Instantiate(_flagPrefab);
        _flag.CollidedWithPreview += UnsetFlag;
        _flag.gameObject.SetActive(false);
        _flagMeshRenderers = _flag.GetComponentsInChildren<MeshRenderer>();
        _requiredArea = owner.Building.OccupiedZoneRadius;
        _choosable = choosable;
        _choosable.Choosed += OnChoosed;
        _buildingEventInvoker = owner.BuildingEventInvoker;
        _buildingEventInvoker.BuildingPlanned += UnsetFlag;
    }

    public void OnRightMouseClicked()
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

    private void OnChoosed(bool isChosen)
    {
        if (isChosen)
        {
            _inputEventInvoker.RightMouseClicked += OnRightMouseClicked;
            SetFlagVisibility(true);
        }
        else
        {
            _inputEventInvoker.RightMouseClicked -= OnRightMouseClicked;
            SetFlagVisibility(false);
        }
    }

    private void SetFlagVisibility(bool isVisible) 
    {
        foreach (MeshRenderer meshRenderer in _flagMeshRenderers) 
        {
            meshRenderer.enabled = isVisible; 
        }
    }

    private bool GetFlagSetAvailable(Vector3 areaCenter)
    {
        return (Physics.OverlapSphere(areaCenter, _requiredArea, _buildingsLayer).Length == 0 
            && Physics.OverlapSphere(areaCenter, _requiredArea, _wallsLayer).Length == 0);
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