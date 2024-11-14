using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(ResourcesScanner), typeof(ResourcesStorage))]
[RequireComponent(typeof(CollectingResourcesRegister), typeof(UnitTasker), typeof(UnitSpawner))]

public class Base : Building
{
    private UnitTasker _unitTasker;
    private ResourcesScanner _resourcesScanner;
    private UnitSpawner _unitSpawner;

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
        _resourcesScanner = TryGetComponent(out ResourcesScanner resourcesScanner) ? resourcesScanner : null;
        _unitSpawner = TryGetComponent(out UnitSpawner unitSpawner) ? unitSpawner : null;
    }

    private void InitializeComponents()
    {
        _unitTasker.Initialize(_resourcesScanner, CollectingResourcesRegister, UnitTaskEventInvoker);
        _unitSpawner.Initialize(this);
    }
}