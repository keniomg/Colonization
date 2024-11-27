using UnityEngine;

public class MoveToPointCommand : ICommand
{
    private UnitMover _unitMover;
    private Vector3 _pointPosition;
    private Building _building;
    private UnitAnimationEventInvoker _unitAnimationEventInvoker;
    private bool _isComplete;
    private bool _isInterrupted;

    public bool IsComplete => _isComplete;
    public bool IsInterrupted => _isInterrupted;

    public MoveToPointCommand(Unit unit, Vector3 pointPosition, Building building = null)
    {
        _unitMover = unit.Mover;
        _unitAnimationEventInvoker = unit.AnimationEventInvoker;
        _pointPosition = pointPosition;
        _building = building;
    }

    public void Execute()
    {
        _unitAnimationEventInvoker.Invoke(AnimationsTypes.Walk, true);

        if (_unitMover.MoveToPoint(_pointPosition, _building))
        {
            _isComplete = true;
            _unitAnimationEventInvoker.Invoke(AnimationsTypes.Walk, false);
        }
    }
}