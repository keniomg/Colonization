using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesScanner : MonoBehaviour
{
    private Map _map;
    private ResourcesEventInvoker _resourcesEventInvoker;
    private Dictionary<int, Resource> _resourcesOnMap = new Dictionary<int, Resource>();

    public event Action<int, Resource> FoundAvailableResource;

    private void OnDisable()
    {
        _map.ResourceAppeared -= RegisterResource;
        _map.ResourceDisappeared -= UnregisterResource;
    }

    public void Initialize(ResourcesEventInvoker resourcesEventInvoker, Map map)
    {
        _map = map;
        _map.ResourceAppeared += RegisterResource;
        _map.ResourceDisappeared += UnregisterResource;
        _resourcesEventInvoker = resourcesEventInvoker;
        _resourcesEventInvoker.ResourceCollected += UnregisterResource;
    }

    public bool GetResourceOnMap(Resource resource)
    {
        return _resourcesOnMap.ContainsKey(resource.gameObject.GetInstanceID());
    }

    private void RegisterResource(int id, Resource resource)
    {
        if (_resourcesOnMap.ContainsKey(id) == false)
        {
            _resourcesOnMap.Add(id, resource);
            FoundAvailableResource?.Invoke(id, resource);
        }
    }

    private void UnregisterResource(int id, Resource resource)
    {
        if (_resourcesOnMap.ContainsKey(id))
        {
            _resourcesOnMap.Remove(id);
        }
    }
}