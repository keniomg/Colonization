public class CollectResourceTask : Task
{
    private Resource _resource;
    private ResourcesStorage _storage;
    private ResourcesScanner _resourceScanner;
    private Base _owner;

    public override void InitializeExecutor(Unit unit)
    {
        base.InitializeExecutor(unit);

        if (_resource != null)
        {
            Commands.Enqueue(new MoveToTargetCommand(Unit, _resource.transform));
            Commands.Enqueue(new TakeResourceCommand(Unit, _resource, _owner));
            Commands.Enqueue(new MoveToPointCommand(Unit, _storage.transform.position, _owner.Building));
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
        _resourceScanner = owner.ResourcesScanner;
        _resource = resource;
        owner.ResourcesEventInvoker.InvokeResourceChoosed(resource);
    }
}