using System;
using UnityEngine;

public abstract class UnitTaskEventInvoker<UnitType> : ScriptableObject where UnitType : Unit
{
    public event Action<UnitType, UnitTaskStatusTypes> UnitTaskStatusChanged;

    public void Invoke(UnitType unit, UnitTaskStatusTypes statusType)
    {
        UnitTaskStatusChanged?.Invoke(unit, statusType);
    }
}