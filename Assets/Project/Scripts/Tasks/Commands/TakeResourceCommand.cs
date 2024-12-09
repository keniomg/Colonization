public class TakeResourceCommand : ICommand
{
    private UnitResourcesHolder _resourcesHolder;
    private Resource _resource;
    private UnitAnimationEventInvoker _unitAnimationEventInvoker;
    private bool _isComplete;
    private bool _isInterrupted;

    public bool IsComplete => _isComplete;
    public bool IsInterrupted => _isInterrupted;

    public TakeResourceCommand(Unit unit, Resource resource)
    {
        if (unit.TryGetComponent(out UnitResourcesHolder unitResourcesHolder))
        {
            _resourcesHolder = unitResourcesHolder;
            _unitAnimationEventInvoker = unit.AnimationEventInvoker;
            _resource = resource;
        }
    }

    public void Execute()
    {
        if (_resourcesHolder.TakeResource(_resource))
        {
            _isComplete = true;
            _unitAnimationEventInvoker.Invoke(AnimationsTypes.Hold, true);
        }
        else
        {
            _isInterrupted = true;
        }
    }
}