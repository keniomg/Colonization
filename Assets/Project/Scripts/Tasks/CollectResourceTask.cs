public class CollectResourceTask : Task
{
    private Resource _resource;
    private ResourcesStorage _storage;
    private Base _owner;

    public override void InitializeExecutor(Unit unit)
    {
        base.InitializeExecutor(unit);

        if (_resource != null)
        {
            Commands.Enqueue(new MoveToResourceCommand(Unit, _resource.transform));
            Commands.Enqueue(new TakeResourceCommand(Unit, _resource));
            Commands.Enqueue(new MoveToTargetCommand(Unit, _storage.transform, _owner.Building.OccupiedZoneRadius));
            Commands.Enqueue(new DeliverResourceCommand(Unit, _resource, _owner));
        }
        else
        {
            Unit = null;
        }
    }

    public CollectResourceTask(Resource resource, Base owner)
    {
        _owner = owner;
        _storage = owner.Storage;
        _resource = resource;
    }
}