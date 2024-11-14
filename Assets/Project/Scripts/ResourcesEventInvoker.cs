using System;
using UnityEngine;

public class ResourcesEventInvoker : MonoBehaviour
{
    public event Action<Resource> ResourceReturned;

    public void Invoke(Resource resource)
    {
        ResourceReturned?.Invoke(resource);
    }
}