using UnityEngine;

public class MoveToTargetCommand : ICommand
{
    private UnitMover _unitMover;
    private Vector3 _targetPosition;
    private bool _isComplete;
    private MoveTypes _moveType;

    public bool IsComplete => _isComplete;

    public MoveToTargetCommand(UnitMover unitMover, Vector3 targetPosition, MoveTypes moveType)
    {
        _unitMover = unitMover;
        _targetPosition = targetPosition;
        _moveType = moveType;
    }

    public void Execute()
    {
        if (_unitMover.MoveToTarget(_targetPosition, _moveType))
        {
            _isComplete = true;
        }
    }
}
