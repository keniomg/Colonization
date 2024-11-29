using System;
using UnityEngine;

public class UnitColonizer : MonoBehaviour
{
    public event Action Colonized;

    public bool Colonize(Vector3 position, Base owner, BuildingEventInvoker buildingEventInvoker)
    {
        Base newOwner = Instantiate(owner, position, Quaternion.identity);
        buildingEventInvoker.InvokeBuildingStarted();

        if (TryGetComponent(out Unit unit))
        {
            Colonized.Invoke();
            unit.UnitCommandController.Initialize(newOwner.UnitTaskEventInvoker);

            return true;
        }

        return false;
    }
}