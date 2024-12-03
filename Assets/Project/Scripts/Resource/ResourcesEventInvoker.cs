using System;
using UnityEngine;

[CreateAssetMenu]
public class ResourcesEventInvoker : ScriptableObject
{
    public event Action<Resource> ResourceReturned;
    public event Action<Resource> ResourceCollected;
    public event Action<Resource> ResourceChoosed;

    public void InvokeResourceReturn(Resource resource)
    {
        ResourceReturned?.Invoke(resource);
    }

    public void InvokeResourceCollected(Resource resource)
    {
        ResourceCollected?.Invoke(resource);
    }

    public void InvokeResourceChoosed(Resource resource)
    {
        ResourceChoosed?.Invoke(resource);
    }
}