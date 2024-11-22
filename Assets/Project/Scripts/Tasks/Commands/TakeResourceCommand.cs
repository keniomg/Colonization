﻿public class TakeResourceCommand : ICommand
{
    private UnitResourcesHolder _resourcesHolder;
    private Resource _resource;
    private UnitAnimationEventInvoker _unitAnimationEventInvoker;

    private bool _isComplete;

    public bool IsComplete => _isComplete;

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
    }
}