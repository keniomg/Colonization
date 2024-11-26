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
        _resourcesEventInvoker.ResourceCollected += HandleResourceUnavailable;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _resourcesScanner.FoundAvailableResource -= HandleAvailableResource;
    }

    protected override void GiveTask()
    {
        Task givingTask = GetRandomTask();
        GetFreeUnit().CollectorCommandController.AddTask(givingTask);
        Tasks.Remove(givingTask);
    }

    private void HandleAvailableResource(int id, Resource resource)
    {
        if (_collectingResourcesRegister.CollectingResources.ContainsKey(id) == false)
        {
            Tasks.Add(new CollectResourceTask(resource, Owner, _resourcesEventInvoker));
            DelegateTasks();
        }
    }

    private void HandleResourceUnavailable(int id, Resource resource)
    {
        Task task = new CollectResourceTask(resource, Owner, _resourcesEventInvoker);

        if (Tasks.Contains(task))
        {
            Tasks.Remove(task);
        }
    }
}