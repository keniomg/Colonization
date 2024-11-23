public class CollectorTasker : UnitTasker<Collector>
{
    private CollectingResourcesRegister _collectingResourcesRegister;
    private ResourcesScanner _resourcesScanner;
    private ResourcesStorage _storage;
    private ResourcesEventInvoker _resourcesEventInvoker;

    public override void Initialize(Base owner)
    {
        base.Initialize(owner);
        UnitTaskEventInvoker = Owner.CollectorTaskEventInvoker;
        UnitTaskEventInvoker.UnitTaskStatusChanged += HandleUnitStatusChanged;
        _resourcesScanner = Owner.ResourcesScanner;
        _resourcesScanner.FoundAvailableResource += HandleAvailableResource;
        _collectingResourcesRegister = Owner.CollectingResourcesRegister;
        _storage = Owner.Storage;
        _resourcesEventInvoker = Owner.ResourcesEventInvoker;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _resourcesScanner.FoundAvailableResource -= HandleAvailableResource;
    }

    protected override void GiveTask()
    {
        GetFreeUnit().CollectorCommandController.AddTask(Tasks.Dequeue());
    }

    private void HandleAvailableResource(int id, Resource resource)
    {
        if (_collectingResourcesRegister.CollectingResources.ContainsKey(id) == false)
        {
            Tasks.Enqueue(new CollectResourceTask(resource, Owner, _resourcesEventInvoker));
            DelegateTasks();
        }
    }
}