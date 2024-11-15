using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesScanner : MonoBehaviour
{
    [SerializeField] private Map _map;

    public Dictionary<int, Resource> ResourcesOnMap { get; private set; } = new Dictionary<int, Resource>();

    public event Action<int, Resource> FoundAvailableResource;

    private void OnEnable()
    {
        _map.ResourceAppeared += RegisterResource;
        _map.ResourceDisappeared += UnregisterResource;
    }

    private void OnDisable()
    {
        _map.ResourceAppeared -= RegisterResource;
        _map.ResourceDisappeared -= UnregisterResource;
    }

    private void RegisterResource(int id, Resource resource)
    {
        if (ResourcesOnMap.ContainsKey(id) == false)
        {
            ResourcesOnMap.Add(id, resource);
            FoundAvailableResource?.Invoke(id, resource);
        }
    }

    private void UnregisterResource(int id, Resource resource)
    {
        if (ResourcesOnMap.ContainsKey(id))
        {
            ResourcesOnMap.Remove(id);
        }
    }
}
