using UnityEngine;

public class MoveToPointCommand : ICommand
{
    private UnitMover _unitMover;
    private bool _isComplete;
    private Vector3 _pointPosition;
    private Building _building;
    private UnitAnimationEventInvoker _unitAnimationEventInvoker;

    public bool IsComplete => _isComplete;

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