using System;
using UnityEngine;

[CreateAssetMenu]
public class BuildingEventInvoker : ScriptableObject
{
    public event Action BuildingPlanned;
    public event Action BuildingStarted;

    public void InvokeBuldingPlanned()
    {
        BuildingPlanned?.Invoke();
    }

    public void InvokeBuildingStarted()
    {
        BuildingStarted?.Invoke();
    }
}