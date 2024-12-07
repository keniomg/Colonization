﻿using System;
using UnityEngine;

public class UnitTaskEventInvoker : ScriptableObject
{
    public event Action<Unit, UnitTaskStatusTypes> UnitTaskStatusChanged;

    public void Invoke(Unit unit, UnitTaskStatusTypes statusType)
    {
        UnitTaskStatusChanged?.Invoke(unit, statusType);
    }
}