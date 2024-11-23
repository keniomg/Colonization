using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(ResourcesScanner), typeof(ResourcesStorage))]
[RequireComponent(typeof(CollectingResourcesRegister), typeof(CollectorTasker), typeof(ColonizatorSpawner))]
[RequireComponent(typeof(FlagSetter), typeof(CollectorSpawner), typeof(ColonizatorTasker))]
[RequireComponent(typeof(Building), typeof(Choosable))]

public class Base : MonoBehaviour
{
    [field: SerializeField] public ResourcesEventInvoker ResourcesEventInvoker { get; private set; }

    private CollectorTasker _collectorTasker;
    private ColonizatorTasker _colonizatorTasker;
    private CollectorSpawner _collectorSpawner;
    private ColonizatorSpawner _colonizatorSpawner;
    private Choosable _choosable;

    public Building Building { get; private set; }
    public BuildingEventInvoker BuildingEventInvoker { get; private set; }
    public ResourcesScanner ResourcesScanner { get; private set; }
    public ResourcesStorage Storage { get; private set; }
    public CollectingResourcesRegister CollectingResourcesRegister { get; private set; }
    public CollectorTaskEventInvoker CollectorTaskEventInvoker { get; private set; }
    public ColonizatorTaskEventInvoker ColonizatorTaskEventInvoker { get; private set; }
    public FlagSetter FlagSetter { get; private set; }

    private void Awake()
    {
        GetComponents();
        InitializeComponents();
    }

    private void GetComponents()
    {
        CollectorTaskEventInvoker = ScriptableObject.CreateInstance<CollectorTaskEventInvoker>();
        ColonizatorTaskEventInvoker = ScriptableObject.CreateInstance<ColonizatorTaskEventInvoker>();
        BuildingEventInvoker = ScriptableObject.CreateInstance<BuildingEventInvoker>();

        _choosable = GetComponent<Choosable>();
        Building = GetComponent<Building>();
        FlagSetter = GetComponent<FlagSetter>();
        CollectingResourcesRegister = GetComponent<CollectingResourcesRegister>();
        Storage = GetComponent<ResourcesStorage>();
        _collectorTasker = GetComponent<CollectorTasker>();
        _colonizatorTasker = GetComponent<ColonizatorTasker>();
        ResourcesScanner = GetComponent<ResourcesScanner>();
        _collectorSpawner = GetComponent<CollectorSpawner>();
        _colonizatorSpawner = GetComponent<ColonizatorSpawner>();
    }

    private void InitializeComponents()
    {
        _collectorTasker.Initialize(this);
        _colonizatorTasker.Initialize(this);
        _collectorSpawner.Initialize(this);
        _colonizatorSpawner.Initialize(this);
        ResourcesScanner.Initialize(ResourcesEventInvoker, Building.GetMap());
        Storage.Initialize(ResourcesEventInvoker);
        FlagSetter.Initialize(this, _choosable);
    }
}