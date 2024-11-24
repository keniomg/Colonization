using System;
using UnityEngine;

public class UnitColonizer : MonoBehaviour
{
    public event Action<Colonizator> Colonized;

    public bool Colonize(Vector3 position, Base owner, BuildingEventInvoker buildingEventInvoker)
    {
        Instantiate(owner, position, Quaternion.identity);
        buildingEventInvoker.InvokeBuildingStarted();

        if (TryGetComponent(out Colonizator colonizator))
        {
            Colonized.Invoke(colonizator);
        }

        return true;
    }
}