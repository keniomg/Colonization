using System.Collections.Generic;
using UnityEngine;

public class ResourcesStorage : MonoBehaviour
{
    [SerializeField] private ResourcesEventInvoker _resourceEventInvoker;
    [SerializeField] private BoxCollider _storageZone;
    [SerializeField] private Transform _storagePlace;

    private List<Resource> _resources;

    private void OnTriggerEnter(Collider other)
    {
        if (other == _storageZone && other.TryGetComponent(out Resource resource))
        {
            _resources.Add(resource);
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == _storageZone && other.TryGetComponent(out Resource resource))
        {
            _resources.Remove(resource);
        }
    }

    public void TakeResource(Resource resource)
    {
        PlaceResource(resource);
        _resourceEventInvoker.InvokeResourceChanged(_resources.Count);
    }

    public void RemoveResource(Resource resource)
    {
        if (_resources.Count > 0)
        {
            _resources.Remove(resource);
            _resourceEventInvoker.InvokeResourceReturn(resource);
            _resourceEventInvoker.InvokeResourceChanged(_resources.Count);
        }
    }

    private void PlaceResource(Resource resource)
    {
        resource.transform.position = _storagePlace.position;
        resource.transform.rotation = Quaternion.identity;
    }
}