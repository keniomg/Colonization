public class CollectorTasker : UnitTasker<Collector>
{
    private CollectingResourcesRegister _collectingResourcesRegister;
    private ResourcesScanner _resourcesScanner;

    public override void Initialize(Base owner)
    {
        base.Initialize(owner);
        UnitTaskEventInvoker = Owner.CollectorTaskEventInvoker;
        UnitTaskEventInvoker.UnitTaskStatusChanged += HandleUnitStatusChanged;
        _resourcesScanner = Owner.ResourcesScanner;
        _resourcesScanner.FoundAvailableResource += HandleAvailableResource;
        _collectingResourcesRegister = Owner.CollectingResourcesRegister;
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
        if (_collectingResourcesRegister.GetResourceCollecting(id) == false)
        {
            Tasks.Add(new CollectResourceTask(resource, Owner));
            DelegateTasks();
        }
    }
}