public class CollectResourceTask : Task
{
    private Resource _resource;
    private ResourcesStorage _storage;
    private CollectingResourcesRegister _collectingResourcesRegister;

    public override void InitializeExecutor(Unit unit)
    {
        base.InitializeExecutor(unit);

        Commands.Enqueue(new MoveToTargetCommand(Unit, _resource.transform, MoveTypes.MoveToResource));
        Commands.Enqueue(new TakeResourceCommand(Unit, _resource));
        Commands.Enqueue(new MoveToTargetCommand(Unit, _storage.transform, MoveTypes.MoveToStorage));
        Commands.Enqueue(new DeliverResourceCommand(Unit, _storage, _resource));
    }

    public CollectResourceTask(Resource resource, ResourcesStorage storage, CollectingResourcesRegister collectingResourcesRegister)
    {
        _collectingResourcesRegister = collectingResourcesRegister;
        _collectingResourcesRegister.RegisterCollectingResource(resource.gameObject.GetInstanceID(), resource);
        _resource = resource;
        _storage = storage;
    }
}