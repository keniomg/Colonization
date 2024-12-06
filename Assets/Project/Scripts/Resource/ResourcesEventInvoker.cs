using System;
using UnityEngine;

[CreateAssetMenu]
public class ResourcesEventInvoker : ScriptableObject
{
    public event Action<int> ResourceTaked;
    public event Action<int> ResourceDropped;
    public event Action<int, int> ResourceChoosed;
    public event Action<int, int> ResourceUnchoosed;
    public event Action<int> ResourcePlacedOnBase;
    public event Action<Resource> ResourceReturned;

    public void InvokeResourceTaked(int resourceID)
    {
        ResourceTaked?.Invoke(resourceID);
    }

    public void InvokeResourceDropped(int resourceID)
    {
        ResourceDropped?.Invoke(resourceID);
    }

    public void InvokeResourceChoosed(int resourceID, int baseID)
    {
        ResourceChoosed?.Invoke(resourceID, baseID);
    }

    public void InvokeResourceUnchoosed(int resourceID, int baseID)
    {
        ResourceUnchoosed?.Invoke(resourceID, baseID);
    }

    public void InvokeResourcePlacedOnBase(int resourceID)
    {
        ResourcePlacedOnBase?.Invoke(resourceID);
    }

    public void InvokeResourceReturned(Resource resource)
    {
        ResourceReturned?.Invoke(resource);
    }
}