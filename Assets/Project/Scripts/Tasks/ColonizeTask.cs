using UnityEngine;

public class ColonizeTask : Task
{
    private Transform _flagTransform;
    private Base _owner;
    private BuildingEventInvoker _buildingEventInvoker;

    public override void InitializeExecutor(Unit unit)
    {
        base.InitializeExecutor(unit);

        _buildingEventInvoker.InvokeBuldingPlanned();
        Commands.Enqueue(new MoveToTargetCommand(Unit, _flagTransform, _owner.Building.OccupiedZoneRadius));
        Commands.Enqueue(new ColonizeCommand(unit, _flagTransform, _buildingEventInvoker));
    }

    public ColonizeTask(Transform flagTransform, Base owner, BuildingEventInvoker buildingEventInvoker)
    {
        _flagTransform.position = flagTransform.position;
        _owner = owner;
        _buildingEventInvoker = buildingEventInvoker;
    }
}