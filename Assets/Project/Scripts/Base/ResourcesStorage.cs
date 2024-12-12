using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourcesStorage : MonoBehaviour
{
    [SerializeField] private BoxCollider _storageZone;
    [SerializeField] private Transform _storagePlace;

    private ResourcesEventInvoker _resourceEventInvoker;
    private Dictionary<int, Resource> _resources = new();

    public event Action ValueChanged;

    public int Count => _resources.Count;

    public void Initialize(ResourcesEventInvoker resourcesEventInvoker)
    {
        _resourceEventInvoker = resourcesEventInvoker;
    }

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
        resource.transform.SetPositionAndRotation(GetPlacePosition(), Quaternion.identity);
        resource.transform.SetParent(transform);
        _resourceEventInvoker.InvokeResourceUnchoosed(gameObject.GetInstanceID(), resource.gameObject.GetInstanceID());

        if (resource.TryGetComponent(out BoxCollider collider))
        {
            collider.enabled = true;
        }
    }

    public void PayResource(int resourceCount)
    {
        for (int i = 0; i < resourceCount; i++)
        {
            Resource resource = _resources.ElementAt(UnityEngine.Random.Range(0, _resources.Count)).Value;
            UnregisterResource(resource.gameObject.GetInstanceID(), resource);
        }
    }

    private void RegisterResource(int id, Resource resource)
    {
        if (_resources.ContainsKey(id) == false)
        {
            _resources.Add(id, resource);
            ValueChanged?.Invoke();
        }
    }

    private void UnregisterResource(int id, Resource resource)
    {
        if (_resources.ContainsKey(id))
        {
            _resources.Remove(id);
            ValueChanged.Invoke();
            _resourceEventInvoker.InvokeResourceReturned(resource);
            resource.transform.SetParent(null);
        }
    }

    private Vector3 GetPlacePosition()
    {
        int numbersOfSide = 2;
        float minimumPositionX = _storagePlace.position.x - _storagePlace.localScale.x / numbersOfSide;
        float maximumPositionX = _storagePlace.position.x + _storagePlace.localScale.x / numbersOfSide;
        float minimumPositionZ = _storagePlace.position.z - _storagePlace.localScale.z / numbersOfSide;
        float maximumPositionZ = _storagePlace.position.z + _storagePlace.localScale.z / numbersOfSide;

        float positionX = UnityEngine.Random.Range(minimumPositionX, maximumPositionX);
        float positionY = _storagePlace.position.y;
        float positionZ = UnityEngine.Random.Range(minimumPositionZ, maximumPositionZ);
        Vector3 placePosition = new(positionX, positionY, positionZ);

        return placePosition;
    }
}