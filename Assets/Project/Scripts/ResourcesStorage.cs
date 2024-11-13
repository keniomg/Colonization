using System.Collections.Generic;
using UnityEngine;

public class ResourcesStorage : MonoBehaviour
{
    private List<Resource> _resources;

    public void AddResource(Resource resource)
    {
        _resources.Add(resource);
    }

    public void RemoveResource(Resource resource)
    {
        if (_resources.Count > 0)
        {
            _resources.Remove(resource);
        }
    }
}
