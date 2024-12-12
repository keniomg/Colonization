using System.Collections.Generic;
using UnityEngine;

public class CollectingResourceRegister : MonoBehaviour
{
    private List<int> _collectingResources = new();
    private ResourcesEventInvoker _resourcesEventInvoker;

    public void Initialize(ResourcesEventInvoker resourcesEventInvoker)
    {
        _resourcesEventInvoker = resourcesEventInvoker;
        _resourcesEventInvoker.ResourceChoosed += RegisterCollectingResource;
        _resourcesEventInvoker.ResourceUnchoosed += UnregisterCollectingResource;
    }

    private void OnDestroy()
    {
        _resourcesEventInvoker.ResourceChoosed -= RegisterCollectingResource;
        _resourcesEventInvoker.ResourceUnchoosed -= UnregisterCollectingResource;
    }

    public bool GetResourceCollectingStatus(int resourceID)
    {
        return _collectingResources.Contains(resourceID);
    }

    private void RegisterCollectingResource(int baseID, int resourceID)
    {
        if (baseID == gameObject.GetInstanceID())
        {
            if (_collectingResources.Contains(resourceID) == false)
            {
                _collectingResources.Add(resourceID);
            }
        }
    }

    private void UnregisterCollectingResource(int baseID, int resourceID)
    {
        if (baseID == gameObject.GetInstanceID())
        {
            if (_collectingResources.Contains(resourceID))
            {
                _collectingResources.Remove(resourceID);
            }
        }
    }
}