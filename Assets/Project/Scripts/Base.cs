using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Base : MonoBehaviour
{
    public ResourcesStorage Storage {get; private set; }
    public CollectingResourcesRegister CollectingResourcesRegister {get; private set; }

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

}

public class UnitStatusEventInvoker : MonoBehaviour
{
    public event Action<Unit, UnitStatusTypes> UnitStatusChanged;

    public void Invoke(Unit unit, UnitStatusTypes statusType)
    {
        UnitStatusChanged?.Invoke(unit, statusType);
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

    [field: SerializeField] public UnitStatusEventInvoker UnitStatusEventInvoker { get; private set; }

    private Dictionary<int, Unit> _freeUnits;
    private WaitForSeconds _searchUnitDelay;
    private ResourcesScanner _resourcesScanner;
    private CollectingResourcesRegister _collectingResourcesRegister;
    private ResourcesStorage _storage;
    private Queue<Task> _tasks;

    private void OnEnable()
    {
        UnitStatusEventInvoker.UnitStatusChanged += HandleUnitStatusChanged;
        _resourcesScanner.FoundAvailableResource += RequireCollector;
    }

    private void OnDisable()
    {
        UnitStatusEventInvoker.UnitStatusChanged -= HandleUnitStatusChanged;
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
        //foreach(Unit unit in _freeUnits)
        //{
        //    if (unit.CommandController.Commands.Count == 0)
        //    {
        //        return unit;
        //    }
        //}

        return null;
    }
}

public class CollectResourceTask : Task
{
    private CollectingResourcesRegister _collectingResourcesRegister;

    public void Initialize(Unit unit, Resource resource, ResourcesStorage storage, CollectingResourcesRegister collectingResourcesRegister)
    {
        _collectingResourcesRegister = collectingResourcesRegister;
        _collectingResourcesRegister.RegisterCollectingResource(resource.GetInstanceID(), resource);

        Commands.Enqueue(new MoveToTargetCommand(unit.Mover, resource.transform.position, MoveTypes.MoveToResource));
        Commands.Enqueue(new TakeResourceCommand(unit.ResourcesHolder, resource));
        Commands.Enqueue(new MoveToTargetCommand(unit.Mover, storage.transform.position, MoveTypes.MoveToStorage));
        Commands.Enqueue(new DeliverResourceCommand(unit.ResourcesHolder, storage, resource));
    }
}

public class Unit : MonoBehaviour
{
    public UnitCommandController CommandController {get; private set; }
    public UnitResourcesHolder ResourcesHolder {get; private set; }
    public UnitMover Mover {get; private set; }
    public CollectingResourcesRegister CollectingResourcesRegister {get; private set; }

}

public class UnitMover : MonoBehaviour
{
    [SerializeField] private float _defaultOffsetDistance;
    [SerializeField] private float _speed;
    [SerializeField] private float _offsetToResource;
    [SerializeField] private float _offsetToBase;

    public bool MoveToTarget(Vector3 targetPosition, MoveTypes moveType)
    {
        float offset = _defaultOffsetDistance;

        switch (moveType)
        {
            case MoveTypes.MoveToStorage:
                offset = _offsetToBase;
                break;
            case MoveTypes.MoveToResource:
                offset = _offsetToResource;
                break;
        }

        float distanceToTarget = Vector3.Distance(targetPosition, transform.position);
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;
        transform.Translate(directionToTarget * _speed * Time.deltaTime, Space.World);

        if (distanceToTarget <= offset)
        {
            return true;
        }

        return false;
    }
}

public class UnitResourcesHolder : MonoBehaviour
{
    [field: SerializeField] public float ActionDistanceOffset { get; private set; }

    public Resource HoldingResource { get; private set; }

    public bool TakeResource(Resource resource)
    {
        float distanceToTarget = Vector3.Distance(resource.transform.position, transform.position);

        if (HoldingResource == null && distanceToTarget < ActionDistanceOffset)
        {
            HoldingResource = resource;
            return true;
        }

        return false;
    }

    public bool GiveResource(Resource resource, ResourcesStorage storage)
    {
        float distanceToTarget = Vector3.Distance(storage.transform.position, transform.position);

        if (HoldingResource != null && distanceToTarget < ActionDistanceOffset)
        {
            storage.AddResource(resource);
            HoldingResource = null;
            return true;
        }

        return false;
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

