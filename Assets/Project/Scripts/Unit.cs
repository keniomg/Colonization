using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(UnitCommandController), typeof(UnitResourcesHolder))]
[RequireComponent(typeof(UnitMover), typeof(CapsuleCollider))]

public class Unit : MonoBehaviour
{
    public UnitCommandController CommandController { get; private set; }
    public UnitResourcesHolder ResourcesHolder { get; private set; }
    public UnitMover Mover { get; private set; }

    private void Awake()
    {
        GetComponents();
    }

    private void GetComponents()
    {
        CommandController = TryGetComponent(out UnitCommandController commandController) ? commandController : null;
        ResourcesHolder = TryGetComponent(out UnitResourcesHolder unitResourcesHolder) ? unitResourcesHolder : null;
        Mover = TryGetComponent(out UnitMover mover) ? mover : null;
    }
}