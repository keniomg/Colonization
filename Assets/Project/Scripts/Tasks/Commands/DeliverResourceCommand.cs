public class DeliverResourceCommand : ICommand
{
    private UnitResourcesHolder _resourcesHolder;
    private Resource _resource;
    private ResourcesStorage _storage;
    private UnitAnimationEventInvoker _unitAnimationEventInvoker;

    private bool _isComplete;

    public bool IsComplete => _isComplete;

    public DeliverResourceCommand(Unit unit, ResourcesStorage storage, Resource resource)
    {
        if (unit.TryGetComponent(out UnitResourcesHolder unitResourcesHolder))
        {
            _resourcesHolder = unitResourcesHolder;
            _unitAnimationEventInvoker = unit.AnimationEventInvoker;
            _storage = storage;
            _resource = resource;
        }
    }

    public void Execute()
    {
        if (_resourcesHolder.GiveResource(_resource, _storage))
        {
            _isComplete = true;
            _unitAnimationEventInvoker.Invoke(AnimationsTypes.Hold, false);
        }
    }
}