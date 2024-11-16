using System.Collections.Generic;
using UnityEngine;

public class ResourcesStorage : MonoBehaviour
{
    [SerializeField] private ResourcesEventInvoker _resourceEventInvoker;
    [SerializeField] private BoxCollider _storageZone;
    [SerializeField] private Transform _storagePlace;

    private Dictionary<int, Resource> _resources = new Dictionary<int, Resource>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetType() == typeof(BoxCollider) && other.TryGetComponent(out Resource resource))
        {
            RegisterResource(resource.gameObject.GetInstanceID(), resource);
        }     
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetType() == typeof(BoxCollider) && other.TryGetComponent(out Resource resource))
        {
            UnregisterResource(resource.gameObject.GetInstanceID(), resource);
        }
    }

    public void PlaceResource(Resource resource)
    {
        resource.transform.position = GetPlacePosition();
        resource.transform.rotation = Quaternion.identity;

        if (resource.TryGetComponent(out BoxCollider collider))
        {
            collider.enabled = true;
        }
    }

    private void RegisterResource(int id, Resource resource)
    {
        if (_resources.ContainsKey(id) == false)
        {
            _resources.Add(id, resource);
            _resourceEventInvoker.InvokeResourceChanged(_resources.Count);
        }
    }

    private void UnregisterResource(int id, Resource resource)
    {
        if (_resources.ContainsKey(id))
        {
            _resources.Remove(id);
            _resourceEventInvoker.InvokeResourceChanged(_resources.Count);
            _resourceEventInvoker.InvokeResourceReturn(resource);
        }
    }

    private Vector3 GetPlacePosition()
    {
        int numbersOfSide = 2;
        float minimumPositionX = _storagePlace.position.x - _storagePlace.localScale.x / numbersOfSide;
        float maximumPositionX = _storagePlace.position.x + _storagePlace.localScale.x / numbersOfSide;
        float minimumPositionZ = _storagePlace.position.z - _storagePlace.localScale.z / numbersOfSide;
        float maximumPositionZ = _storagePlace.position.z + _storagePlace.localScale.z / numbersOfSide;

        float positionX = Random.Range(minimumPositionX, maximumPositionX);
        float positionY = _storagePlace.position.y;
        float positionZ = Random.Range(minimumPositionZ, maximumPositionZ);
        Vector3 placePosition = new(positionX, positionY, positionZ);

        return placePosition;
    }
}