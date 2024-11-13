public class TakeResourceCommand : ICommand
{
    private UnitResourcesHolder _resourcesHolder;
    private Resource _resource;

    private bool _isComplete;

    public bool IsComplete => _isComplete;

    public TakeResourceCommand(UnitResourcesHolder resourcesHolder, Resource resource)
    {
        _resourcesHolder = resourcesHolder;
        _resource = resource;
    }

    public void Execute()
    {
        if (_resourcesHolder.TakeResource(_resource))
        {
            _isComplete = true;
        }
    }
}
