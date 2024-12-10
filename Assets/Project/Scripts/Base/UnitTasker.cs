using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitTasker : MonoBehaviour
{
    private float _unitSearchingDelay;
    private WaitForSeconds _searchUnitDelay;
    private Dictionary<int, Unit> _freeUnits = new();
    private UnitTaskEventInvoker _unitTaskEventInvoker;
    private int _colonizationTaskCost;
    private int _minimumUnitsCountForColonization;
    private List<Task> _colonizationTasks = new();
    private List<Task> _collectTasks = new();
    private Base _owner;
    private FlagSetter _flagSetter;
    private ResourcesScanner _resourcesScanner;
    private CollectingResourceRegister _collectingResourceRegister;
    private ResourcesEventInvoker _resourcesEventInvoker;

    protected virtual void OnDestroy()
    {
        _unitTaskEventInvoker.UnitTaskStatusChanged -= HandleUnitStatusChanged;
        _resourcesScanner.FoundResource -= HandleAvailableResource;
        _flagSetter.FlagStatusChanged -= OnFlagStatusChanged;
    }

    public virtual void Initialize(Base owner)
    {
        _minimumUnitsCountForColonization = 1;
        _colonizationTaskCost = 5;
        _unitSearchingDelay = 1;
        _searchUnitDelay = new(_unitSearchingDelay);

        _owner = owner;
        _flagSetter = _owner.FlagSetter;
        _unitTaskEventInvoker = _owner.UnitTaskEventInvoker;
        _resourcesScanner = _owner.ResourcesScanner;
        _collectingResourceRegister = _owner.CollectingResourceRegister;

        _flagSetter.FlagStatusChanged += OnFlagStatusChanged;
        _unitTaskEventInvoker.UnitTaskStatusChanged += HandleUnitStatusChanged;
        _resourcesScanner.FoundResource += HandleAvailableResource;
        _resourcesEventInvoker = owner.ResourcesEventInvoker;
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

    private void HandleAvailableResource(Resource resource)
    {
        if (resource.transform.parent == null && 
            _collectingResourceRegister.GetResourceCollectingStatus(resource.gameObject.GetInstanceID()) == false)
        {
            _collectTasks.Add(new CollectResourceTask(resource, _owner));
            _resourcesEventInvoker.InvokeResourceChoosed(gameObject.GetInstanceID(), resource.gameObject.GetInstanceID());
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