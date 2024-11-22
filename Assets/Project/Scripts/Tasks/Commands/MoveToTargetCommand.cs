﻿using UnityEngine;

public class MoveToTargetCommand : ICommand
{
    private UnitMover _unitMover;
    private Transform _targetTransform;
    private bool _isComplete;
    private UnitAnimationEventInvoker _unitAnimationEventInvoker;

    public bool IsComplete => _isComplete;

    public MoveToTargetCommand(Unit unit, Transform targetTransform)
    {
        _unitMover = unit.Mover;
        _unitAnimationEventInvoker = unit.AnimationEventInvoker;
        _targetTransform = targetTransform;
    }

    public void Execute()
    {
        _unitAnimationEventInvoker.Invoke(AnimationsTypes.Walk, true);

        if (_unitMover.MoveToTarget(_targetTransform))
        {
            _isComplete = true;
            _unitAnimationEventInvoker.Invoke(AnimationsTypes.Walk, false);
        }
    }
}