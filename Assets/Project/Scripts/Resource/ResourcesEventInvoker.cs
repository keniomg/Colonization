using System;
using UnityEngine;

[CreateAssetMenu]
public class ResourcesEventInvoker : ScriptableObject
{
    public event Action<int, int> ResourceChoosed;
    public event Action<int, int> ResourceUnchoosed;
    public event Action<Resource> ResourceReturned;

    public void InvokeResourceChoosed(int baseID, int resourceID)
    {
        ResourceChoosed?.Invoke(baseID, resourceID);
    }

    public void InvokeResourceUnchoosed(int baseID, int resourceID)
    {
        ResourceUnchoosed?.Invoke(baseID, resourceID);
    }

    public void InvokeResourceReturned(Resource resource)
    {
        ResourceReturned?.Invoke(resource);
    }
}