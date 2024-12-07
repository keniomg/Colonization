﻿public class DeliverResourceCommand : ICommand
{
    private UnitResourcesHolder _resourcesHolder;
    private Resource _resource;
    private ResourcesStorage _storage;
    private UnitAnimationEventInvoker _unitAnimationEventInvoker;
    private ResourcesEventInvoker _resourceEventInvoker;

    private bool _isComplete;
    private bool _isInterrupted;

    public bool IsComplete => _isComplete;
    public bool IsInterrupted => _isInterrupted;

    public DeliverResourceCommand(Unit unit, Resource resource, Base owner)
    {
        if (unit.TryGetComponent(out UnitResourcesHolder unitResourcesHolder))
        {
            _resourcesHolder = unitResourcesHolder;
            _unitAnimationEventInvoker = unit.AnimationEventInvoker;
            _storage = owner.Storage;
            _resourceEventInvoker = owner.ResourcesEventInvoker;
            _resource = resource;
        }
    }

    public void Execute()
    {
        if (_resourcesHolder.GiveResource(_resource, _storage))
        {
            _isComplete = true;
            _resourceEventInvoker.InvokeResourceTaked(_resource);
        }
        else
        {
            _isInterrupted = true;
        }
            
        _unitAnimationEventInvoker.Invoke(AnimationsTypes.Hold, false);
    }
}