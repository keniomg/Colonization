using UnityEngine;

public class ColonizeCommand : ICommand
{
    private UnitColonizer _unitColonizer;
    private Base _owner;
    private Vector3 _colonizePoint;
    private bool _isComplete;
    private BuildingEventInvoker _buildingEventInvoker;

    public bool IsComplete => _isComplete;

    public ColonizeCommand(Colonizator colonizator, Base owner, Vector3 colonizePoint, BuildingEventInvoker buildingEventInvoker)
    {
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
    }
}