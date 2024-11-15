using System;
using UnityEngine;

public class ResourcesEventInvoker : MonoBehaviour
{
    public event Action<Resource> ResourceReturned;
    public event Action<int> ResourceChanged;

    public void InvokeResourceReturn(Resource resource)
    {
        ResourceReturned?.Invoke(resource);
    }

    public void InvokeResourceChanged(int currentResourceStorageCount)
    {
        ResourceChanged?.Invoke(currentResourceStorageCount);
    }
}