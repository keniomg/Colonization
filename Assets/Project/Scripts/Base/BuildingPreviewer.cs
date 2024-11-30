using UnityEngine;

public class BuildingPreviewer : MonoBehaviour
{
    [SerializeField] private BuildingPreview _buildingPrefab;

    private BuildingPreview _buildingPreview;
    private BuildingEventInvoker _buildingEventInvoker;
    private Choosable _choosable;
    private FlagSetter _flagSetter;
    private Vector3 _previewPosition;

    private void OnDestroy()
    {
        _choosable.Choosed -= OnChoosed;
        _buildingEventInvoker.BuildingPlanned -= OnBuildingPlanned;
        _buildingEventInvoker.BuildingStarted -= OnBuildingStarted;
    }

    public void Initialize(FlagSetter flagSetter, BuildingEventInvoker buildingEventInvoker, Choosable choosable)
    {
        _flagSetter = flagSetter;
        _flagSetter.FlagStatusChanged += OnFlagStatusChanged;
        _buildingPreview = Instantiate(_buildingPrefab);
        _buildingPreview.gameObject.SetActive(false);
        _choosable = choosable;
        _choosable.Choosed += OnChoosed;
        _buildingEventInvoker = buildingEventInvoker;
        _buildingEventInvoker.BuildingPlanned += OnBuildingPlanned;
        _buildingEventInvoker.BuildingStarted += OnBuildingStarted;
    }

    private void OnBuildingPlanned()
    {
        _buildingPreview.transform.position = _previewPosition;
        _buildingPreview.gameObject.SetActive(true);
    }

    private void OnBuildingStarted()
    {
        _buildingPreview.gameObject.SetActive(false);
    }

    private void OnChoosed(bool isChosen)
    {
        _buildingPreview.SetPreviewVisibility(isChosen);
    }

    private void OnFlagStatusChanged()
    {
        if (_flagSetter.Flag != null)
        {
            _previewPosition = _flagSetter.Flag.transform.position;
        }
    }
}