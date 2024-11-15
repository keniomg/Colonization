using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTasker : MonoBehaviour
{
    [SerializeField] private float _unitSearchingDelay;

    private Dictionary<int, Unit> _freeUnits = new Dictionary<int, Unit>();
    private List<int> _unitsId = new List<int>();
    private Queue<Task> _tasks = new Queue<Task>();
    private WaitForSeconds _searchUnitDelay;
    private Coroutine _findTasksExecutors;

    private CollectingResourcesRegister _collectingResourcesRegister;
    private UnitTaskEventInvoker _unitTaskEventInvoker;
    private ResourcesScanner _resourcesScanner;
    private ResourcesStorage _storage;

    private event Action _appearedNewTask;

    private void Awake()
    {
        _searchUnitDelay = new(_unitSearchingDelay);
        _findTasksExecutors = null;
    }

    private void OnEnable()
    {
        _appearedNewTask += DelegateTask;
    }

    private void OnDisable()
    {
        _unitTaskEventInvoker.UnitTaskStatusChanged -= HandleUnitStatusChanged;
        _resourcesScanner.FoundAvailableResource -= HandleAvailableResource;
        _appearedNewTask -= DelegateTask;
    }

    public void Initialize(Base owner)
    {
        _resourcesScanner = owner.ResourcesScanner;
        _resourcesScanner.FoundAvailableResource += HandleAvailableResource;
        _collectingResourcesRegister = owner.CollectingResourcesRegister;
        _unitTaskEventInvoker = owner.UnitTaskEventInvoker;
        _unitTaskEventInvoker.UnitTaskStatusChanged += HandleUnitStatusChanged;
        _storage = owner.Storage;
    }

    private void HandleUnitStatusChanged(Unit unit, UnitTaskStatusTypes statusType)
    {
        switch (statusType)
        {
            case UnitTaskStatusTypes.Busy:
                RemoveFreeUnit(unit.gameObject.GetInstanceID());
                break;
            case UnitTaskStatusTypes.Free:
                AddFreeUnit(unit.GetInstanceID(), unit);
                break;
        }
    }

    private void RemoveFreeUnit(int id)
    {
        if (_freeUnits.ContainsKey(id))
        {
            _freeUnits.Remove(id);
            _unitsId.Remove(id);
        }
    }

    private void AddFreeUnit(int id, Unit unit)
    {
        if (_freeUnits.ContainsKey(id) == false)
        {
            _freeUnits.Add(id, unit);
            _unitsId.Add(id);
        }
    }

    private void HandleAvailableResource(int id, Resource resource)
    {
        if (_collectingResourcesRegister.CollectingResources.ContainsKey(id) == false)
        {
            _tasks.Enqueue(new CollectResourceTask(resource, _storage, _collectingResourcesRegister));
            _appearedNewTask?.Invoke();
        }
    }

    private void DelegateTask()
    {
        _findTasksExecutors = StartCoroutine(FindTasksExecutors());
    }

    private IEnumerator FindTasksExecutors()
    {
        while (_tasks.Count > 0)
        {
            while (GetFreeUnit() == null)
            {
                yield return _searchUnitDelay;
            }

            GetFreeUnit().CommandController.AddTask(_tasks.Dequeue());
        }

        _findTasksExecutors = null;
    }

    private Unit GetFreeUnit()
    {
        if (_freeUnits.Count > 0)
        {
            foreach (Unit unit in _freeUnits.Values)
            {
                return unit;
            }
        }

        return null;
    }
}