using UnityEngine;

public class BuildingPreviewer : MonoBehaviour
{
    [SerializeField] private GameObject _buildingPrefab;

    private GameObject _buildingPreview;
    private BuildingEventInvoker _buildingEventInvoker;
    private Choosable _choosable;
    private float _transparencyValue;
    private MeshRenderer[] _previewMeshRenderers;
    private FlagSetter _flagSetter;
    private Vector3 _previewPosition;

    private void OnDisable()
    {
        _choosable.Choosed -= OnChoosed;
        _buildingEventInvoker.BuildingPlanned -= OnBuildingPlanned;
        _buildingEventInvoker.BuildingStarted -= OnBuildingStarted;
    }

    public void Initialize(FlagSetter flagSetter, BuildingEventInvoker buildingEventInvoker, Choosable choosable)
    {
        _flagSetter = flagSetter;
        _flagSetter.FlagStatusChanged += OnFlagStatusChanged;
        _previewMeshRenderers = GetComponentsInChildren<MeshRenderer>();
        _buildingPreview = Instantiate(_buildingPrefab);
        _buildingPreview.SetActive(false);
        _choosable = choosable;
        _choosable.Choosed += OnChoosed;
        _buildingEventInvoker = buildingEventInvoker;
        _buildingEventInvoker.BuildingPlanned += OnBuildingPlanned;
        _buildingEventInvoker.BuildingStarted += OnBuildingStarted;
        _transparencyValue = 0.5f;
        SetPrefabTransparency();
    }

    private void OnBuildingPlanned()
    {
        _buildingPreview.transform.position = _previewPosition;
        _buildingPreview.SetActive(true);
    }

    private void OnBuildingStarted()
    {
        _buildingPreview.SetActive(false);
    }

    private void SetPrefabTransparency()
    {
        foreach (MeshRenderer meshRenderer in _previewMeshRenderers)
        {
            Color color = meshRenderer.material.color;
            color.a = _transparencyValue;
        }
    }

    private void OnChoosed(bool isChosen)
    {
        if (isChosen)
        {
            SetPreviewVisibility(true);
        }
        else
        {
            SetPreviewVisibility(false);
        }
    }

    private void SetPreviewVisibility(bool isVisible)
    {
        foreach (MeshRenderer meshRenderer in _previewMeshRenderers)
        {
            meshRenderer.enabled = isVisible;
        }
    }

    private void OnFlagStatusChanged()
    {
        if (_flagSetter.Flag != null)
        {
            _previewPosition = _flagSetter.Flag.transform.position;
        }
    }
}