public class ColonizatorTasker : UnitTasker<Colonizator>
{
    private FlagSetter _flagSetter;
    private BuildingEventInvoker _buildingEventInvoker;

    public override void Initialize(Base owner)
    {
        base.Initialize(owner);
        UnitTaskEventInvoker = Owner.ColonizatorTaskEventInvoker;
        UnitTaskEventInvoker.UnitTaskStatusChanged += HandleUnitStatusChanged;
        _flagSetter = Owner.FlagSetter;
        _flagSetter.FlagStatusChanged += HandleFlagSet;
        _buildingEventInvoker = owner.BuildingEventInvoker;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _flagSetter.FlagStatusChanged -= HandleFlagSet;
    }

    protected override void GiveTask()
    {
        Task givingTask = GetRandomTask();
        GetFreeUnit().ColonizatorCommandController.AddTask(givingTask);
        Tasks.Remove(givingTask);
    }

    private void HandleFlagSet()
    {
        Tasks.Clear();

        if (_flagSetter.Flag != null)
        {
            Tasks.Add(new ColonizeTask(_flagSetter.Flag.transform.position, Owner, _buildingEventInvoker));
            DelegateTasks();
        }
    }
}