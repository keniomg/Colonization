public class CollectResourceTask : Task
{
    private Resource _resource;
    private ResourcesStorage _storage;
    private CollectingResourcesRegister _collectingResourcesRegister;
    private Base _owner;

    public override void InitializeExecutor(Unit unit)
    {
        base.InitializeExecutor(unit);

        Commands.Enqueue(new MoveToTargetCommand(Unit, _resource.transform));
        Commands.Enqueue(new TakeResourceCommand(Unit, _resource, _owner));
        Commands.Enqueue(new MoveToPointCommand(Unit, _storage.transform.position, _owner.Building));
        Commands.Enqueue(new DeliverResourceCommand(Unit, _storage, _resource, _collectingResourcesRegister));
    }

    public CollectResourceTask(Resource resource, Base owner)
    {
        _owner = owner;
        _storage = owner.Storage;
        _collectingResourcesRegister = owner.CollectingResourcesRegister;
        _resource = resource;
        _collectingResourcesRegister.RegisterCollectingResource(_resource.gameObject.GetInstanceID(), _resource);
    }
}