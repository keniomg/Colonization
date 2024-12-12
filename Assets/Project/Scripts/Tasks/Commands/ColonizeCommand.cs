using UnityEngine;

public class ColonizeCommand : ICommand
{
    private UnitColonizer _unitColonizer;
    private Vector3 _colonizePosition;
    private bool _isComplete;
    private BuildingEventInvoker _buildingEventInvoker;
    private bool _isInterrupted;

    public bool IsComplete => _isComplete;
    public bool IsInterrupted => _isInterrupted;

    public ColonizeCommand(Unit unit, Vector3 colonizePosition, BuildingEventInvoker buildingEventInvoker)
    {
        _buildingEventInvoker = buildingEventInvoker;
        _unitColonizer = unit.Colonizer;
        _colonizePosition = colonizePosition;
    }

    public void Execute()
    {
        if (_unitColonizer.CanColonize(_colonizePosition, _buildingEventInvoker))
        {
            _isComplete = true;
        }
        else
        {
            _isInterrupted = true;
        }
    }
}