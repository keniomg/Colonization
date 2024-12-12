using UnityEngine;

public class MoveToPointCommand : ICommand
{
    private UnitMover _unitMover;
    private Vector3 _pointPosition;
    private float _offset;
    private UnitAnimationEventInvoker _unitAnimationEventInvoker;
    private bool _isComplete;
    private bool _isInterrupted;

    public bool IsComplete => _isComplete;
    public bool IsInterrupted => _isInterrupted;

    public MoveToPointCommand(Unit unit, Vector3 position, float offset = 0)
    {
        _unitMover = unit.Mover;
        _unitAnimationEventInvoker = unit.AnimationEventInvoker;
        _pointPosition = position;
        _offset = offset;
    }

    public void Execute()
    {
        _unitAnimationEventInvoker.Invoke(AnimationsTypes.Walk, true);

        if (_unitMover.CanMoveToPoint(_pointPosition, _offset))
        {
            _isComplete = true;
            _unitAnimationEventInvoker.Invoke(AnimationsTypes.Walk, false);
        }
    }
}