using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitTasker : MonoBehaviour
{
    private float _unitSearchingDelay;
    private int _colonizationTaskCost;
    private UnitTaskEventInvoker _unitTaskEventInvoker;
    private List<Task> _collectTasks = new();
    private List<Task> _colonizationTasks = new();
    private Base _owner;
    private Dictionary<int, Unit> _freeUnits = new();
    private WaitForSeconds _searchUnitDelay;
    private FlagSetter _flagSetter;
    private ResourcesScanner _resourcesScanner;
    private int _minimumUnitsCountForColonization;

    protected virtual void OnDestroy()
    {
        _unitTaskEventInvoker.UnitTaskStatusChanged -= HandleUnitStatusChanged;
        _resourcesScanner.FoundAvailableResource -= HandleAvailableResource;
        _flagSetter.FlagStatusChanged -= OnFlagStatusChanged;
    }

    public virtual void Initialize(Base owner)
    {
        _minimumUnitsCountForColonization = 1;
        _unitSearchingDelay = 1;
        _colonizationTaskCost = 5;
        _searchUnitDelay = new(_unitSearchingDelay);
        _owner = owner;
        _flagSetter = _owner.FlagSetter;
        _flagSetter.FlagStatusChanged += OnFlagStatusChanged;
        _unitTaskEventInvoker = _owner.UnitTaskEventInvoker;
        _unitTaskEventInvoker.UnitTaskStatusChanged += HandleUnitStatusChanged;
        _resourcesScanner = _owner.ResourcesScanner;
        _resourcesScanner.FoundAvailableResource += HandleAvailableResource;
        StartCoroutine(AppointExecutors());
    }

    private void OnFlagStatusChanged()
    {
        _colonizationTasks.Clear();

        if (_flagSetter.Flag != null)
        {
            _colonizationTasks.Add(new ColonizeTask(_flagSetter.Flag.transform.position, _owner, _owner.BuildingEventInvoker));
        }
    }

    private void HandleUnitStatusChanged(Unit unit, UnitTaskStatusTypes statusType)
    {
        switch (statusType)
        {
            case UnitTaskStatusTypes.Busy:
                RemoveFreeUnit(unit.gameObject.GetInstanceID());
                break;
            case UnitTaskStatusTypes.Free:
                AddFreeUnit(unit.gameObject.GetInstanceID(), unit);
                break;
        }
    }

    private void GiveTask(List<Task> tasks)
    {
        Task givingTask = tasks[Random.Range(0, tasks.Count)];
        GetFreeUnit().UnitCommandController.AddTask(givingTask);
        tasks.Remove(givingTask);
    }

    private Unit GetFreeUnit()
    {
        if (_freeUnits.Count > 0)
        {
            return _freeUnits.ElementAt(Random.Range(0, _freeUnits.Count)).Value;
        }

        return null;
    }

    private void HandleAvailableResource(int id, Resource resource)
    {
        if (_resourcesScanner.GetResourceCollectingStatus(resource) == false)
        {
            _collectTasks.Add(new CollectResourceTask(resource, _owner));
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

    private IEnumerator AppointExecutors()
    {
        while (true)
        {
            if (_freeUnits.Count == 0)
            {
                yield return _searchUnitDelay;
            }
            else
            {
                if (_flagSetter.Flag != null && _owner.Storage.Count >= _colonizationTaskCost 
                    && _owner.UnitSpawner.UnitsCount > _minimumUnitsCountForColonization)
                {
                    if (_colonizationTasks.Count > 0)
                    {
                        _owner.Storage.PayResource(_colonizationTaskCost);
                        GiveTask(_colonizationTasks);
                    }
                }
                else
                {
                    if (_collectTasks.Count > 0)
                    {
                        GiveTask(_collectTasks);
                    }
                }
            }

            yield return null;
        }
    }
}