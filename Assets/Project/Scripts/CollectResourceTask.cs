public class CollectResourceTask : Task
{
    private CollectingResourcesRegister _collectingResourcesRegister;

    public void Initialize(Unit unit, Resource resource, ResourcesStorage storage, CollectingResourcesRegister collectingResourcesRegister)
    {
        _collectingResourcesRegister = collectingResourcesRegister;
        _collectingResourcesRegister.RegisterCollectingResource(resource.GetInstanceID(), resource);

        Commands.Enqueue(new MoveToTargetCommand(unit.Mover, resource.transform.position, MoveTypes.MoveToResource));
        Commands.Enqueue(new TakeResourceCommand(unit.ResourcesHolder, resource));
        Commands.Enqueue(new MoveToTargetCommand(unit.Mover, storage.transform.position, MoveTypes.MoveToStorage));
        Commands.Enqueue(new DeliverResourceCommand(unit.ResourcesHolder, storage, resource));
    }
}

