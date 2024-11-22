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
        Commands.Enqueue(new TakeResourceCommand(Unit, _resource));
        Commands.Enqueue(new MoveToPointCommand(Unit, _storage.transform.position, _owner.Building));
        Commands.Enqueue(new DeliverResourceCommand(Unit, _storage, _resource));
    }

    public CollectResourceTask(Resource resource, ResourcesStorage storage, CollectingResourcesRegister collectingResourcesRegister, Base owner = null)
    {
        _collectingResourcesRegister = collectingResourcesRegister;
        _collectingResourcesRegister.RegisterCollectingResource(resource.gameObject.GetInstanceID(), resource);
        _resource = resource;
        _storage = storage;
        _owner = owner;
    }
}