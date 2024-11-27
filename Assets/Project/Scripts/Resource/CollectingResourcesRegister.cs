using System.Collections.Generic;
using UnityEngine;

public class CollectingResourcesRegister : MonoBehaviour
{
    private Dictionary<int, Resource> _collectingResources = new Dictionary<int, Resource>();

    public void RegisterCollectingResource(int id, Resource resource)
    {
        if (_collectingResources.ContainsKey(id) == false)
        {
            _collectingResources.Add(id, resource);
        }
    }

    public void UnregisterCollectingResource(int id, Resource resource)
    {
        if (_collectingResources.ContainsKey(id))
        {
            _collectingResources.Remove(id);
        }
    }

    public bool GetResourceCollecting(int id)
    {
        return _collectingResources.ContainsKey(id);
    }
}