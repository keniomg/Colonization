using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitTasker<UnitType> : MonoBehaviour where UnitType : Unit
{
    [SerializeField] private float _unitSearchingDelay;

    protected UnitTaskEventInvoker<UnitType> UnitTaskEventInvoker;
    protected Queue<Task> Tasks = new Queue<Task>();
    protected Base Owner;

    private Dictionary<int, UnitType> _freeUnits = new Dictionary<int, UnitType>();
    private List<int> _unitsId = new List<int>();
    private WaitForSeconds _searchUnitDelay;
    private Coroutine _appointExecutors;

    protected virtual void OnDisable()
    {
        UnitTaskEventInvoker.UnitTaskStatusChanged -= HandleUnitStatusChanged;
    }

    private void Awake()
    {
        _searchUnitDelay = new(_unitSearchingDelay);
        _appointExecutors = null;
    }

    public virtual void Initialize(Base owner) 
    {
        Owner = owner;
    }

    protected void DelegateTasks()
    {
        _appointExecutors = StartCoroutine(AppointExecutors());
    }

    protected void HandleUnitStatusChanged(UnitType unit, UnitTaskStatusTypes statusType)
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

    protected abstract void GiveTask();

    protected UnitType GetFreeUnit()
    {
        int defailtUnitIndex = 0;

        if (_freeUnits.Count > 0)
        {
            return _freeUnits[_unitsId[defailtUnitIndex]];
        }

        return null;
    }

    private void RemoveFreeUnit(int id)
    {
        if (_freeUnits.ContainsKey(id))
        {
            _freeUnits.Remove(id);
            _unitsId.Remove(id);
        }
    }

    private void AddFreeUnit(int id, UnitType unit)
    {
        if (_freeUnits.ContainsKey(id) == false)
        {
            _freeUnits.Add(id, unit);
            _unitsId.Add(id);
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