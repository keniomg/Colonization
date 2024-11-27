using UnityEngine;

public class MoveToTargetCommand : ICommand
{
    private UnitMover _unitMover;
    private Transform _targetTransform;
    private UnitAnimationEventInvoker _unitAnimationEventInvoker;
    private bool _isComplete;
    private bool _isInterrupted;

    public bool IsComplete => _isComplete;
    public bool IsInterrupted => _isInterrupted;

    public MoveToTargetCommand(Unit unit, Transform targetTransform)
    {
        _unitMover = unit.Mover;
        _unitAnimationEventInvoker = unit.AnimationEventInvoker;
        _targetTransform = targetTransform;
    }

    public void Execute()
    {
        _unitAnimationEventInvoker.Invoke(AnimationsTypes.Walk, true);

        if (_unitMover.MoveToTarget(_targetTransform, ref _isInterrupted))
        {
            _isComplete = true;
            _unitAnimationEventInvoker.Invoke(AnimationsTypes.Walk, false);
        }
    }
}