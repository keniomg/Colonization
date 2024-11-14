using System;
using UnityEngine;

public class Map : MonoBehaviour
{
    public event Action<int, Resource> ResourceAppeared;
    public event Action<int, Resource> ResourceDisappeared;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Resource resource))
        {
            ResourceAppeared?.Invoke(resource.gameObject.GetInstanceID(), resource);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Resource resource))
        {
            ResourceDisappeared?.Invoke(resource.gameObject.GetInstanceID(), resource);
        }
    }
}
