using System;
using UnityEngine;

public class UnitColonizer : MonoBehaviour
{
    [SerializeField] private Base _basePrefab;

    public event Action Colonized;

    public bool Colonize(Vector3 position, BuildingEventInvoker buildingEventInvoker)
    {
        Base newOwner = Instantiate(_basePrefab, position, Quaternion.identity);
        buildingEventInvoker.InvokeBuildingStarted();

        if (TryGetComponent(out Unit unit))
        {
            Colonized.Invoke();
            unit.UnitCommandController.Initialize(newOwner.UnitTaskEventInvoker, unit.AnimationEventInvoker);

            return true;
        }

        return false;
    }
}