using System;
using UnityEngine;

[CreateAssetMenu]
public class ResourcesEventInvoker : ScriptableObject
{
    public event Action<Resource> ResourceReturned;
    public event Action<int, Resource> ResourceCollected;

    public void InvokeResourceReturn(Resource resource)
    {
        ResourceReturned?.Invoke(resource);
    }

    public void InvokeResourceCollected(int id, Resource resource)
    {
        ResourceCollected?.Invoke(id, resource);
    }
}