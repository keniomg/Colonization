using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(ResourcesScanner), typeof(ResourcesStorage))]
[RequireComponent(typeof(CollectingResourcesRegister), typeof(ColonizatorSpawner))]
[RequireComponent(typeof(FlagSetter), typeof(CollectorSpawner), typeof(ColonizatorTasker))]
[RequireComponent(typeof(Building), typeof(Choosable), typeof(BuildingPreviewer))]
[RequireComponent(typeof(ResourcesCounterView))]
public class Base : MonoBehaviour
{
    [field: SerializeField] public ResourcesEventInvoker ResourcesEventInvoker { get; private set; }

    private BuildingPreviewer _buildingPreviewer;
    private UnitTasker _unitTasker;
    private UnitSpawner _unitSpawner;
    private Choosable _choosable;
    private ResourcesCounterView _resourcesCounterView;

    public Building Building { get; private set; }
    public BuildingEventInvoker BuildingEventInvoker { get; private set; }
    public ResourcesScanner ResourcesScanner { get; private set; }
    public ResourcesStorage Storage { get; private set; }
    public CollectingResourcesRegister CollectingResourcesRegister { get; private set; }
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
        CollectingResourcesRegister = GetComponent<CollectingResourcesRegister>();
        Storage = GetComponent<ResourcesStorage>();
        ResourcesScanner = GetComponent<ResourcesScanner>();
        
        _buildingPreviewer = GetComponent<BuildingPreviewer>();
        _choosable = GetComponent<Choosable>();
        _unitTasker = GetComponent<UnitTasker>();
        _unitSpawner = GetComponent<UnitSpawner>();
        _resourcesCounterView = GetComponent<ResourcesCounterView>();
    }

    private void InitializeComponents()
    {
        _unitTasker.Initialize(this);
        _unitSpawner.Initialize(this);
        ResourcesScanner.Initialize(ResourcesEventInvoker, Building.GetMap());
        Storage.Initialize(ResourcesEventInvoker);
        FlagSetter.Initialize(this, _choosable);
        _buildingPreviewer.Initialize(FlagSetter, BuildingEventInvoker, _choosable);
        _resourcesCounterView.Initialize(Storage);
    }
}