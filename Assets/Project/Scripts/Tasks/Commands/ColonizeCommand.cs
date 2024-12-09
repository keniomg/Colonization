using UnityEngine;

public class ColonizeCommand : ICommand
{
    private UnitColonizer _unitColonizer;
    private Transform _colonizeTransform;
    private bool _isComplete;
    private BuildingEventInvoker _buildingEventInvoker;
    private bool _isInterrupted;

    public bool IsComplete => _isComplete;
    public bool IsInterrupted => _isInterrupted;

    public ColonizeCommand(Unit unit, Transform colonizeTransform, BuildingEventInvoker buildingEventInvoker)
    {
        _buildingEventInvoker = buildingEventInvoker;
        _unitColonizer = unit.Colonizer;
        _colonizeTransform = colonizeTransform;
    }

    public void Execute()
    {
        if (_unitColonizer.Colonize(_colonizeTransform.position, _buildingEventInvoker))
        {
            _isComplete = true;
        }
        else
        {
            _isInterrupted = true;
        }
    }
}