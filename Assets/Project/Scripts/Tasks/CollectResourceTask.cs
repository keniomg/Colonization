public class CollectResourceTask : Task
{
    private Resource _resource;
    private ResourcesStorage _storage;
    private CollectingResourcesRegister _collectingResourcesRegister;

    public override void InitializeExecutor(Unit unit)
    {
        base.InitializeExecutor(unit);

        Commands.Enqueue(new MoveToTargetCommand(Unit.Mover, _resource.transform.position, MoveTypes.MoveToResource));
        Commands.Enqueue(new TakeResourceCommand(Unit.ResourcesHolder, _resource));
        Commands.Enqueue(new MoveToTargetCommand(Unit.Mover, _storage.transform.position, MoveTypes.MoveToStorage));
        Commands.Enqueue(new DeliverResourceCommand(Unit.ResourcesHolder, _storage, _resource));
    }

    public CollectResourceTask(Resource resource, ResourcesStorage storage, CollectingResourcesRegister collectingResourcesRegister)
    {
        _collectingResourcesRegister = collectingResourcesRegister;
        _collectingResourcesRegister.RegisterCollectingResource(resource.GetInstanceID(), resource);
        _resource = resource;
        _storage = storage;
    }
}