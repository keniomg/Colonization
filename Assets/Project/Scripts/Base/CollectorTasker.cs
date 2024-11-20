public class CollectorTasker : UnitTasker<Collector>
{
    private CollectingResourcesRegister _collectingResourcesRegister;
    private ResourcesScanner _resourcesScanner;
    private ResourcesStorage _storage;

    public override void Initialize(Base owner)
    {
        base.Initialize(owner);
        _resourcesScanner = owner.ResourcesScanner;
        _resourcesScanner.FoundAvailableResource += HandleAvailableResource;
        _collectingResourcesRegister = owner.CollectingResourcesRegister;
        _storage = owner.Storage;
    }

    protected override void OnDisable()
    {
        _resourcesScanner.FoundAvailableResource -= HandleAvailableResource;
    }

    private void HandleAvailableResource(int id, Resource resource)
    {
        if (_collectingResourcesRegister.CollectingResources.ContainsKey(id) == false)
        {
            Tasks.Enqueue(new CollectResourceTask(resource, _storage, _collectingResourcesRegister));
            DelegateTask();
        }
    }
}