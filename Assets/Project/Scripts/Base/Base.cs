using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(ResourcesScanner), typeof(ResourcesStorage))]
[RequireComponent(typeof(CollectingResourcesRegister), typeof(UnitTasker), typeof(ColonizatorSpawner))]
[RequireComponent(typeof(FlagSetter), typeof(CollectorSpawner))]

public class Base : Building
{
    private UnitTasker _unitTasker;
    private CollectorSpawner _collectorSpawner;
    private ColonizatorSpawner _colonizatorSpawner;

    public ResourcesScanner ResourcesScanner { get; private set; } 
    public ResourcesStorage Storage { get; private set; }
    public CollectingResourcesRegister CollectingResourcesRegister { get; private set; }
    public UnitTaskEventInvoker UnitTaskEventInvoker { get; private set; }
    public FlagSetter FlagSetter { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        GetComponents();
        InitializeComponents();
    }

    private void GetComponents()
    {
        UnitTaskEventInvoker = ScriptableObject.CreateInstance<UnitTaskEventInvoker>();

        FlagSetter = GetComponent<FlagSetter>();
        CollectingResourcesRegister = GetComponent<CollectingResourcesRegister>();
        Storage = GetComponent<ResourcesStorage>();
        _unitTasker = GetComponent<UnitTasker>();
        ResourcesScanner = GetComponent<ResourcesScanner>();
        _collectorSpawner = GetComponent<CollectorSpawner>();
        _colonizatorSpawner = GetComponent<ColonizatorSpawner>();
    }

    private void InitializeComponents()
    {
        _unitTasker.Initialize(this);
        _collectorSpawner.Initialize(this);
        _colonizatorSpawner.Initialize(this);
    }
}