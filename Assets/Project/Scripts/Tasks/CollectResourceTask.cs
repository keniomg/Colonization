public class CollectResourceTask : Task
{
    private CollectingResourcesRegister _collectingResourcesRegister;

    public CollectResourceTask(Resource resource, ResourcesStorage storage, CollectingResourcesRegister collectingResourcesRegister)
    {
        _collectingResourcesRegister = collectingResourcesRegister;
        _collectingResourcesRegister.RegisterCollectingResource(resource.GetInstanceID(), resource);

        Commands.Enqueue(new MoveToTargetCommand(Unit.Mover, resource.transform.position, MoveTypes.MoveToResource));
        Commands.Enqueue(new TakeResourceCommand(Unit.ResourcesHolder, resource));
        Commands.Enqueue(new MoveToTargetCommand(Unit.Mover, storage.transform.position, MoveTypes.MoveToStorage));
        Commands.Enqueue(new DeliverResourceCommand(Unit.ResourcesHolder, storage, resource));
    }
}