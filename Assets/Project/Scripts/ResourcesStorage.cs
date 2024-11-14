using System.Collections.Generic;
using UnityEngine;

public class ResourcesStorage : MonoBehaviour
{
    [SerializeField] private ResourcesEventInvoker _resourceEventInvoker;
    [SerializeField] private BoxCollider _storageZone;
    [SerializeField] private Vector3 _storagePlace;

    private List<Resource> _resources;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Resource resource))
        {
            _resources.Add(resource);
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Resource resource))
        {
            _resources.Remove(resource);
        }
    }

    public void TakeResource(Resource resource)
    {
        PlaceResource(resource);
    }

    public void RemoveResource(Resource resource)
    {
        if (_resources.Count > 0)
        {
            _resources.Remove(resource);
            _resourceEventInvoker.Invoke(resource);
        }
    }

    private void PlaceResource(Resource resource)
    {
        resource.transform.position = _storagePlace;
        resource.transform.rotation = Quaternion.identity;
    }
}
