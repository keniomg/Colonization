using UnityEngine;

public class CollectorTasker : UnitTasker<Collector>
{
    private CollectingResourcesRegister _collectingResourcesRegister;
    private ResourcesScanner _resourcesScanner;
    private ResourcesStorage _storage;

    public override void Initialize(Base owner)
    {
        base.Initialize(owner);
        UnitTaskEventInvoker = Owner.CollectorTaskEventInvoker;
        UnitTaskEventInvoker.UnitTaskStatusChanged += HandleUnitStatusChanged;
        _resourcesScanner = Owner.ResourcesScanner;
        _resourcesScanner.FoundAvailableResource += HandleAvailableResource;
        _collectingResourcesRegister = Owner.CollectingResourcesRegister;
        _storage = Owner.Storage;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _resourcesScanner.FoundAvailableResource -= HandleAvailableResource;
    }

    protected override void GiveTask()
    {
        if (Tasks.Count > 0)
        {
            GetFreeUnit().CollectorCommandController.AddTask(Tasks.Dequeue());
           Debug.Log(1);
        }
    }

    private void HandleAvailableResource(int id, Resource resource)
    {
        if (_collectingResourcesRegister.CollectingResources.ContainsKey(id) == false)
        {
            Tasks.Enqueue(new CollectResourceTask(resource, _storage, _collectingResourcesRegister, Owner));
            DelegateTasks();
        }
    }
}