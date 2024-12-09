using System;
using UnityEngine;

public class BuildingEventInvoker : MonoBehaviour
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