using System;
using UnityEngine;

[CreateAssetMenu]
public class BuildingEventInvoker : ScriptableObject
{
    public event Action<Vector3> BuildingPlanned;
    public event Action<Vector3> BuildingStarted;

    public void InvokeBuldingPlanned(Vector3 position)
    {
        BuildingPlanned?.Invoke(position);
    }

    public void InvokeBuildingStarted(Vector3 position)
    {
        BuildingStarted?.Invoke(position);
    }
}