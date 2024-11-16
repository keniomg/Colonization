using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(ResourcesScanner), typeof(ResourcesStorage))]
[RequireComponent(typeof(CollectingResourcesRegister), typeof(UnitTasker), typeof(UnitSpawner))]

public class Base : Building
{
    private UnitTasker _unitTasker;
    private UnitSpawner _unitSpawner;

    public ResourcesScanner ResourcesScanner { get; private set; } 
    public ResourcesStorage Storage { get; private set; }
    public CollectingResourcesRegister CollectingResourcesRegister { get; private set; }
    public UnitTaskEventInvoker UnitTaskEventInvoker { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        GetComponents();
        InitializeComponents();
    }

    private void GetComponents()
    {
        UnitTaskEventInvoker = ScriptableObject.CreateInstance<UnitTaskEventInvoker>();

        CollectingResourcesRegister = TryGetComponent(out CollectingResourcesRegister collectingResourcesRegister) ? collectingResourcesRegister : null;
        Storage = TryGetComponent(out ResourcesStorage storage) ? storage : null;
        _unitTasker = TryGetComponent(out UnitTasker unitTasker) ? unitTasker : null;
        ResourcesScanner = TryGetComponent(out ResourcesScanner resourcesScanner) ? resourcesScanner : null;
        _unitSpawner = TryGetComponent(out UnitSpawner unitSpawner) ? unitSpawner : null;
    }

    private void InitializeComponents()
    {
        _unitTasker.Initialize(this);
        _unitSpawner.Initialize(this);
    }
}