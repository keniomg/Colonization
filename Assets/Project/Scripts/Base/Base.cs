using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(UnitSpawner), typeof(ResourcesStorage))]
[RequireComponent(typeof(UnitTasker), typeof(ResourcesCounterView), typeof(FlagSetter))]
[RequireComponent(typeof(Building), typeof(Choosable), typeof(BuildingPreviewer))]
public class Base : MonoBehaviour
{
    private BuildingPreviewer _buildingPreviewer;
    private UnitTasker _unitTasker;
    private Choosable _choosable;
    private ResourcesCounterView _resourcesCounterView;

    [field: SerializeField] public ResourcesEventInvoker ResourcesEventInvoker { get; private set; }
    [field: SerializeField] public ResourcesScanner ResourcesScanner { get; private set; }
    [field: SerializeField] public GeneralResourcesRegister GeneralResourcesRegister {get; private set; }

    public UnitSpawner UnitSpawner { get; private set; }
    public Building Building { get; private set; }
    public BuildingEventInvoker BuildingEventInvoker { get; private set; }
    public ResourcesStorage Storage { get; private set; }
    public UnitTaskEventInvoker UnitTaskEventInvoker { get; private set; }
    public FlagSetter FlagSetter { get; private set; }

    private void Awake()
    {
        GetComponents();
        InitializeComponents();
    }

    private void GetComponents()
    {
        UnitTaskEventInvoker = ScriptableObject.CreateInstance<UnitTaskEventInvoker>();
        BuildingEventInvoker = ScriptableObject.CreateInstance<BuildingEventInvoker>();

        Building = GetComponent<Building>();
        FlagSetter = GetComponent<FlagSetter>();
        Storage = GetComponent<ResourcesStorage>();

        _buildingPreviewer = GetComponent<BuildingPreviewer>();
        _choosable = GetComponent<Choosable>();
        _unitTasker = GetComponent<UnitTasker>();
        UnitSpawner = GetComponent<UnitSpawner>();
        _resourcesCounterView = GetComponent<ResourcesCounterView>();
    }

    private void InitializeComponents()
    {
        _unitTasker.Initialize(this);
        UnitSpawner.Initialize(this);
        ResourcesScanner.Initialize(ResourcesEventInvoker);
        Storage.Initialize(ResourcesEventInvoker);
        FlagSetter.Initialize(this, _choosable);
        _buildingPreviewer.Initialize(FlagSetter, BuildingEventInvoker, _choosable);
        _resourcesCounterView.Initialize(Storage);
    }
}