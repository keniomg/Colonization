public class DeliverResourceCommand : ICommand
{
    private UnitResourcesHolder _resourcesHolder;
    private Resource _resource;
    private ResourcesStorage _storage;
    private UnitAnimationEventInvoker _unitAnimationEventInvoker;
    private CollectingResourcesRegister _collectingResourcesRegister;

    private bool _isComplete;

    public bool IsComplete => _isComplete;

    public DeliverResourceCommand(Unit unit, ResourcesStorage storage, Resource resource, CollectingResourcesRegister collectingResourcesRegister)
    {
        if (unit.TryGetComponent(out UnitResourcesHolder unitResourcesHolder))
        {
            _resourcesHolder = unitResourcesHolder;
            _unitAnimationEventInvoker = unit.AnimationEventInvoker;
            _storage = storage;
            _resource = resource;
            _collectingResourcesRegister = collectingResourcesRegister;
        }
    }

    public void Execute()
    {
        if (_resourcesHolder.GiveResource(_resource, _storage))
        {
            _isComplete = true;
            _collectingResourcesRegister.UnregisterCollectingResource(_resource.gameObject.GetInstanceID(), _resource);
            _unitAnimationEventInvoker.Invoke(AnimationsTypes.Hold, false);
        }
    }
}