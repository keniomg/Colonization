using UnityEngine;

public class ColonizeCommand : ICommand
{
    private UnitColonizer _unitColonizer;
    private Base _owner;
    private Vector3 _colonizePoint;
    private bool _isComplete;
    private BuildingEventInvoker _buildingEventInvoker;
    private bool _isInterrupted;

    public bool IsComplete => _isComplete;
    public bool IsInterrupted => _isInterrupted;

    public ColonizeCommand(Colonizator colonizator, Base owner, Vector3 colonizePoint, BuildingEventInvoker buildingEventInvoker)
    {
        _buildingEventInvoker = buildingEventInvoker;
        _unitColonizer = colonizator.Colonizer;
        _owner = owner;
        _colonizePoint = colonizePoint;
    }

    public void Execute()
    {
        if (_unitColonizer.Colonize(_colonizePoint, _owner, _buildingEventInvoker))
        {
            _isComplete = true;
        }
        else
        {
            _isInterrupted = true;
        }
    }
}