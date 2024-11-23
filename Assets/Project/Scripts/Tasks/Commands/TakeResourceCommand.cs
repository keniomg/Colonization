using UnityEngine;

public class TakeResourceCommand : ICommand
{
    private UnitResourcesHolder _resourcesHolder;
    private Resource _resource;
    private UnitAnimationEventInvoker _unitAnimationEventInvoker;
    private ResourcesEventInvoker _resourcesEventInvoker;

    private bool _isComplete;

    public bool IsComplete => _isComplete;

    public TakeResourceCommand(Unit unit, Resource resource, ResourcesEventInvoker resourcesEventInvoker)
    {
        if (unit.TryGetComponent(out UnitResourcesHolder unitResourcesHolder))
        {
            _resourcesHolder = unitResourcesHolder;
            _unitAnimationEventInvoker = unit.AnimationEventInvoker;
            _resource = resource;
            _resourcesEventInvoker = resourcesEventInvoker;
        }
    }

    public void Execute()
    {
        if (_resourcesHolder.TakeResource(_resource, _resourcesEventInvoker))
        {
            _isComplete = true;
            _unitAnimationEventInvoker.Invoke(AnimationsTypes.Hold, true);
        }
    }
}