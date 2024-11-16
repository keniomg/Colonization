using UnityEngine;

public class MoveToTargetCommand : ICommand
{
    private UnitMover _unitMover;
    private Transform _targetTransform;
    private bool _isComplete;
    private MoveTypes _moveType;
    private UnitAnimationEventInvoker _unitAnimationEventInvoker;

    public bool IsComplete => _isComplete;

    public MoveToTargetCommand(Unit unit, Transform targetTransform, MoveTypes moveType)
    {
        _unitMover = unit.Mover;
        _unitAnimationEventInvoker = unit.AnimationEventInvoker;
        _targetTransform = targetTransform;
        _moveType = moveType;
    }

    public void Execute()
    {
        _unitAnimationEventInvoker.Invoke(AnimationsTypes.Walk, true);

        if (_unitMover.MoveToTarget(_targetTransform, _moveType))
        {
            _isComplete = true;
            _unitAnimationEventInvoker.Invoke(AnimationsTypes.Walk, false);
        }
    }
}