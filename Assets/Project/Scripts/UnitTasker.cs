using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTasker : MonoBehaviour
{
    [SerializeField] private float _unitSearchingDelay;

    private Dictionary<int, Unit> _freeUnits;
    private WaitForSeconds _searchUnitDelay;
    private Queue<Task> _tasks;

    private CollectingResourcesRegister _collectingResourcesRegister;
    private UnitTaskEventInvoker _unitTaskEventInvoker;
    private ResourcesScanner _resourcesScanner;
    private ResourcesStorage _storage;

    private event Action _appearedNewTask;

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

    public void Initialize(ResourcesScanner resourcesScanner, CollectingResourcesRegister collectingResourcesRegister, UnitTaskEventInvoker unitTaskEventInvoker)
    {
        _resourcesScanner = resourcesScanner;
        _resourcesScanner.FoundAvailableResource += HandleAvailableResource;
        _collectingResourcesRegister = collectingResourcesRegister;
        _unitTaskEventInvoker = unitTaskEventInvoker;
        _unitTaskEventInvoker.UnitTaskStatusChanged += HandleUnitStatusChanged;
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
        }
    }

    private void AddFreeUnit(int id, Unit unit)
    {
        if (_freeUnits.ContainsKey(id) == false)
        {
            _freeUnits.Add(id, unit);
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
        StartCoroutine(FindTasksExecutors());
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