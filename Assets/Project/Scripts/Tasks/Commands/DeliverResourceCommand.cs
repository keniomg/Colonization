public class DeliverResourceCommand : ICommand
{
    private UnitResourcesHolder _resourcesHolder;
    private Resource _resource;
    private ResourcesStorage _storage;

    private bool _isComplete;

    public bool IsComplete => _isComplete;

    public DeliverResourceCommand(UnitResourcesHolder resourcesHolder, ResourcesStorage storage, Resource resource)
    {
        _resourcesHolder = resourcesHolder;
        _storage = storage;
        _resource = resource;
    }

    public void Execute()
    {
        if (_resourcesHolder.GiveResource(_resource, _storage))
        {
            _isComplete = true;
        }
    }
}
