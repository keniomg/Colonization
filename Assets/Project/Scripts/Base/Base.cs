using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(ResourcesScanner), typeof(ResourcesStorage))]
[RequireComponent(typeof(CollectingResourcesRegister), typeof(UnitTasker), typeof(UnitSpawner))]
[RequireComponent(typeof(FlagSetter))]

public class Base : Building
{
    private UnitTasker _unitTasker;
    private UnitSpawner _unitSpawner;

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
        _unitSpawner = GetComponent<UnitSpawner>();
    }

    private void InitializeComponents()
    {
        _unitTasker.Initialize(this);
        _unitSpawner.Initialize(this, FlagSetter);
    }
}