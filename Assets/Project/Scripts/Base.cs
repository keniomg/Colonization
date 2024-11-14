using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class Base : MonoBehaviour
{
    public ResourcesStorage Storage { get; private set; }
    public CollectingResourcesRegister CollectingResourcesRegister { get; private set; }

    private void Awake()
    {

    }

    private void GetComponents()
    {
        CollectingResourcesRegister = TryGetComponent(out CollectingResourcesRegister collectingResourcesRegister) ? collectingResourcesRegister : null;
        Storage = TryGetComponent(out ResourcesStorage storage) ? storage : null;
    }

    private void InitializeComponents()
    {

    }
}

public class ResourcesSpawner : MonoBehaviour
{
    [SerializeField] private ResourcesEventInvoker _resourcesEventInvoker;
    [SerializeField] private Resource _resourcePrefab;
    [SerializeField] private Transform[] _spawnPositions;
    [SerializeField] private int _poolCapacity;
    [SerializeField] private int _poolMaximumSize;
    [SerializeField] private float _resourcesSpawnDelay;

    private WaitForSeconds _spawnDelay;
    private ObjectPool<Resource> _pool;

    private void Awake()
    {
        _spawnDelay = new(_resourcesSpawnDelay);

        _pool = new ObjectPool<Resource>(
              createFunc: () => Instantiate(_resourcePrefab),
              actionOnGet: (resource) => AccompanyGet(resource),
              actionOnRelease: (resource) => AccompanyRelease(resource),
              actionOnDestroy: (resource) => Destroy(resource),
              collectionCheck: true,
              defaultCapacity: _poolCapacity,
              maxSize: _poolMaximumSize);
    }

    private void OnEnable()
    {
        _resourcesEventInvoker.ResourceReturned += _pool.Release;
    }

    private void OnDisable()
    {
        _resourcesEventInvoker.ResourceReturned -= _pool.Release;
    }

    private void AccompanyGet(Resource resource)
    {
        resource.transform.position = GetSpawnPosition();
        resource.transform.rotation = Quaternion.identity;
    }

    private void AccompanyRelease(Resource resource)
    {

    }

    private IEnumerator SpawnResources()
    {
        yield return _spawnDelay;

        _pool.Get();
    }

    private Vector3 GetSpawnPosition()
    {
        int minimumSpawnPositionsIndex = 0;
        int maximumSpawnPositionsIndex = _spawnPositions.Length;
        int spawnPositionIndex = UnityEngine.Random.Range(minimumSpawnPositionsIndex, maximumSpawnPositionsIndex);
        return _spawnPositions[spawnPositionIndex].position;
    }
}

public class UnitTaskEventInvoker : MonoBehaviour
{
    public event Action<Unit, UnitStatusTypes> UnitTaskStatusChanged;

    public void Invoke(Unit unit, UnitStatusTypes statusType)
    {
        UnitTaskStatusChanged?.Invoke(unit, statusType);
    }
}

public enum UnitStatusTypes
{
    Busy,
    Free
}

public class UnitTasker : MonoBehaviour
{
    [SerializeField] private float _unitSearchingDelay;

    [field: SerializeField] public UnitTaskEventInvoker UnitTaskEventInvoker { get; private set; }

    private Dictionary<int, Unit> _freeUnits;
    private WaitForSeconds _searchUnitDelay;
    private ResourcesScanner _resourcesScanner;
    private CollectingResourcesRegister _collectingResourcesRegister;
    private ResourcesStorage _storage;
    private Queue<Task> _tasks;

    private void OnEnable()
    {
        UnitTaskEventInvoker.UnitTaskStatusChanged += HandleUnitStatusChanged;
        _resourcesScanner.FoundAvailableResource += RequireCollector;
    }

    private void OnDisable()
    {
        UnitTaskEventInvoker.UnitTaskStatusChanged -= HandleUnitStatusChanged;
        _resourcesScanner.FoundAvailableResource -= RequireCollector;
    }

    public void Initialize(ResourcesScanner resourcesScanner, CollectingResourcesRegister collectingResourcesRegister)
    {
        _resourcesScanner = resourcesScanner;
        _collectingResourcesRegister = collectingResourcesRegister;
    }

    private void HandleUnitStatusChanged(Unit unit, UnitStatusTypes statusType)
    {
        switch (statusType)
        {
            case UnitStatusTypes.Busy:
                RemoveFreeUnit(unit.gameObject.GetInstanceID());
                break;
            case UnitStatusTypes.Free:
                AddFreeUnit(unit.GetInstanceID(), unit);
                break;
        }
    }

    private void RemoveFreeUnit(int id)
    {
        if (_freeUnits.ContainsKey(id))
        {
            _freeUnits.Remove(id);
        }
    }

    private void AddFreeUnit(int id, Unit unit)
    {
        if (_freeUnits.ContainsKey(id) == false)
        {
            _freeUnits.Add(id, unit);
        }
    }

    private void RequireCollector(int id, Resource resource)
    {
        if (_collectingResourcesRegister.CollectingResources.ContainsKey(id) == false)
        {
            StartCoroutine(SearchCollector(resource));
        }
    }

    private IEnumerator SearchCollector(Resource resource)
    {
        while (GetFreeUnit() == null)
        {
            yield return _searchUnitDelay;
        }

        Unit collector = GetFreeUnit();
        CollectResourceTask collectTask = new CollectResourceTask();
        collectTask.Initialize(collector, resource, _storage, _collectingResourcesRegister);
        GiveTask(collector, collectTask);
    }

    private void GiveTask(Unit unit, Task task)
    {
        unit.CommandController.AddTask(task);
    }

    private Unit GetFreeUnit()
    {
        int defaultUnitIndex = 0;

        if (_freeUnits.Count > 0)
        {
            return _freeUnits[defaultUnitIndex];
        }

        return null;
    }
}

public class Unit : MonoBehaviour
{
    public UnitCommandController CommandController { get; private set; }
    public UnitResourcesHolder ResourcesHolder { get; private set; }
    public UnitMover Mover { get; private set; }
    public CollectingResourcesRegister CollectingResourcesRegister { get; private set; }

}

public class UnitResourcesHolder : MonoBehaviour
{
    [field: SerializeField] public float ActionDistanceOffset { get; private set; }

    [SerializeField] private Vector3 _holdingPosition;

    public Resource HoldingResource { get; private set; }

    public bool TakeResource(Resource resource)
    {
        float distanceToTarget = Vector3.Distance(resource.transform.position, transform.position);

        if (HoldingResource == null && distanceToTarget < ActionDistanceOffset)
        {
            HoldingResource = resource;
            PlaceHoldingResource(resource);
            return true;
        }

        return false;
    }

    public bool GiveResource(Resource resource, ResourcesStorage storage)
    {
        float distanceToTarget = Vector3.Distance(storage.transform.position, transform.position);

        if (HoldingResource != null && distanceToTarget < ActionDistanceOffset)
        {
            storage.TakeResource(resource);
            HoldingResource = null;
            return true;
        }

        return false;
    }

    private void PlaceHoldingResource(Resource resource)
    {
        resource.transform.position = _holdingPosition;
        resource.transform.rotation = Quaternion.identity;
    }
}

public class Map : MonoBehaviour
{
    public event Action<int, Resource> ResourceAppeared;
    public event Action<int, Resource> ResourceDisappeared;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Resource resource))
        {
            ResourceAppeared?.Invoke(resource.gameObject.GetInstanceID(), resource);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Resource resource))
        {
            ResourceDisappeared?.Invoke(resource.gameObject.GetInstanceID(), resource);
        }
    }
}

public class Resource : MonoBehaviour
{

}

