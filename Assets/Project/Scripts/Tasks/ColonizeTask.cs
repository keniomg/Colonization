using UnityEngine;

public class ColonizeTask : Task
{
    private Vector3 _flagPosition;
    private Base _owner;
    private BuildingEventInvoker _buildingEventInvoker;

    public override void InitializeExecutor(Unit unit)
    {
        base.InitializeExecutor(unit);

        _buildingEventInvoker.InvokeBuldingPlanned();
        Commands.Enqueue(new MoveToPointCommand(Unit, _flagPosition, _owner.Building.OccupiedZoneRadius));
        Commands.Enqueue(new ColonizeCommand(unit, _flagPosition, _buildingEventInvoker));
    }

    public ColonizeTask(Vector3 flagPosition, Base owner, BuildingEventInvoker buildingEventInvoker)
    {
        _flagPosition = flagPosition;
        _owner = owner;
        _buildingEventInvoker = buildingEventInvoker;
    }
}