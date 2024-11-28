using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class UnitTasker : MonoBehaviour
{
    [SerializeField] private float _unitSearchingDelay;

    protected UnitTaskEventInvoker UnitTaskEventInvoker;
    protected List<Task> Tasks = new List<Task>();
    protected Base Owner;

    private Dictionary<int, Unit> _freeUnits = new Dictionary<int, Unit>();
    private WaitForSeconds _searchUnitDelay;
    private Coroutine _appointExecutors;
    private FlagSetter _flagSetter;
    private CollectingResourcesRegister _collectingResourcesRegister;
    private ResourcesScanner _resourcesScanner;

    protected virtual void OnDisable()
    {
        UnitTaskEventInvoker.UnitTaskStatusChanged -= HandleUnitStatusChanged;
        _resourcesScanner.FoundAvailableResource -= HandleAvailableResource;
    }

    public virtual void Initialize(Base owner) 
    {
        _searchUnitDelay = new(_unitSearchingDelay);
        _appointExecutors = null;
        Owner = owner;
        _flagSetter = Owner.FlagSetter;
        UnitTaskEventInvoker = Owner.UnitTaskEventInvoker;
        UnitTaskEventInvoker.UnitTaskStatusChanged += HandleUnitStatusChanged;
        _resourcesScanner = Owner.ResourcesScanner;
        _resourcesScanner.FoundAvailableResource += HandleAvailableResource;
        _collectingResourcesRegister = Owner.CollectingResourcesRegister;
    }

    protected void DelegateTasks()
    {
        _appointExecutors ??= StartCoroutine(AppointExecutors());
    }

    protected void HandleUnitStatusChanged(Unit unit, UnitTaskStatusTypes statusType)
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

    protected void GiveTask()
    {
        Task givingTask = GetRandomTask();
        GetFreeUnit().UnitCommandController.AddTask(givingTask);
        Tasks.Remove(givingTask);
    }

    protected Unit GetFreeUnit()
    {
        if (_freeUnits.Count > 0)
        {
            return _freeUnits.ElementAt(Random.Range(0, _freeUnits.Count)).Value;
        }

        return null;
    }

    protected Task GetRandomTask()
    {
        return Tasks[Random.Range(0, Tasks.Count)];
    }

    private void HandleAvailableResource(int id, Resource resource)
    {
        if (_collectingResourcesRegister.GetResourceCollecting(id) == false)
        {
            Tasks.Add(new CollectResourceTask(resource, Owner));
            DelegateTasks();
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
        while (Tasks.Count > 0)
        {
            while (_freeUnits.Count == 0)
            {
                yield return _searchUnitDelay;
            }

            GiveTask();
        }

        _appointExecutors = null;
    }
}