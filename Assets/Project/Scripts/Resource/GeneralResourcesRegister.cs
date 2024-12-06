using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GeneralResourcesRegister : ScriptableObject
{
    [SerializeField] private ResourcesEventInvoker _resourcesEventInvoker;

    private List<int> _takedResources = new();
    private List<int> _onBaseResources = new();
    private Dictionary<int, List<int>> _collectingResources = new();

    private void OnEnable()
    {
        _resourcesEventInvoker.ResourceTaked += RegisterTakedResouce;
        _resourcesEventInvoker.ResourceDropped += UnregisterTakedResouce;
        _resourcesEventInvoker.ResourceChoosed += RegisterCollectingResource;
        _resourcesEventInvoker.ResourceUnchoosed += UnregisterCollectingResource;
        _resourcesEventInvoker.ResourcePlacedOnBase += RegisterOnBaseResource;
        _resourcesEventInvoker.ResourceReturned += UnregisterOnBaseResource;
    }

    private void OnDisable()
    {
        _resourcesEventInvoker.ResourceTaked -= RegisterTakedResouce;
        _resourcesEventInvoker.ResourceDropped -= UnregisterTakedResouce;
        _resourcesEventInvoker.ResourceChoosed -= RegisterCollectingResource;
        _resourcesEventInvoker.ResourceUnchoosed -= UnregisterCollectingResource;
        _resourcesEventInvoker.ResourcePlacedOnBase -= RegisterOnBaseResource;
        _resourcesEventInvoker.ResourceReturned -= UnregisterOnBaseResource;
    }

    public bool GetResourceTakedStatus(int resourceID)
    {
        return _takedResources.Contains(resourceID);
    }

    public bool GetResourceOnBaseStatus(int resourceID)
    {
        return _onBaseResources.Contains(resourceID);
    }

    public bool GetResourceCollectingStatus(int baseID, int resourceID)
    {
        if (_collectingResources.ContainsKey(baseID))
        {
            return _collectingResources[baseID].Contains(resourceID);
        }

        return false;
    }

    private void RegisterTakedResouce(int resourceID)
    {
        if (_takedResources.Contains(resourceID) == false)
        {
            _takedResources.Add(resourceID);
        }
    }

    private void RegisterOnBaseResource(int resourceID)
    {
        if (_onBaseResources.Contains(resourceID) == false)
        {
            _onBaseResources.Add(resourceID);
        }
    }

    private void RegisterCollectingResource(int baseID, int resourceID)
    {
        if (_collectingResources.ContainsKey(baseID) == false)
        {
            _collectingResources.Add(baseID, new List<int>());
            _collectingResources[baseID].Add(resourceID);
        }
        else
        {
            if (_collectingResources[baseID].Contains(resourceID) == false)
            {
                _collectingResources[baseID].Add(resourceID);
            }
        }
    }

    private void UnregisterTakedResouce(int resourceID)
    {
        if (_takedResources.Contains(resourceID))
        {
            _takedResources.Remove(resourceID);
        }
    }

    private void UnregisterOnBaseResource(Resource resource)
    {
        if (_onBaseResources.Contains(resource.gameObject.GetInstanceID()))
        {
            _onBaseResources.Remove(resource.gameObject.GetInstanceID());
        }
    }

    private void UnregisterCollectingResource(int baseID, int resourceID)
    {
        if (_collectingResources.ContainsKey(baseID))
        {
            if (_collectingResources[baseID].Contains(resourceID) == false)
            {
                _collectingResources[baseID].Remove(resourceID);
            }
        }
    }
}