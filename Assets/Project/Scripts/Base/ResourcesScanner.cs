using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

public class ResourcesScanner : MonoBehaviour
{
    [SerializeField] private float _searchRadius;
    [SerializeField] private LayerMask _resourceLayer;
    [SerializeField] private float _delay;

    private WaitForSeconds _scanRepeatDelay;
    private ResourcesEventInvoker _resourcesEventInvoker;
    private Dictionary<int, Resource> _availableResources = new Dictionary<int, Resource>();
    private Dictionary<int, Resource> _collectingResources = new Dictionary<int, Resource>();

    public event Action<int, Resource> FoundAvailableResource;

    private void OnDestroy()
    {
        _resourcesEventInvoker.ResourceCollected -= UnregisterAvailableResource;
        _resourcesEventInvoker.ResourceChoosed -= RegisterCollectingResource;
        _resourcesEventInvoker.ResourceCollected -= UnregisterCollectingResource;
    }

    public void Initialize(ResourcesEventInvoker resourcesEventInvoker)
    {
        _scanRepeatDelay = new(_delay);
        _resourcesEventInvoker = resourcesEventInvoker;
        _resourcesEventInvoker.ResourceCollected += UnregisterAvailableResource;
        _resourcesEventInvoker.ResourceChoosed += RegisterCollectingResource;
        _resourcesEventInvoker.ResourceCollected += UnregisterCollectingResource;
        StartCoroutine(Scanning());
    }

    public bool GetResourceAvailableStatus(Resource resource)
    {
        return _availableResources.ContainsKey(resource.gameObject.GetInstanceID());
    }

    public bool GetResourceCollectingStatus(Resource resource)
    {
        return _collectingResources.ContainsKey(resource.gameObject.GetInstanceID());
    }

    private IEnumerator Scanning()
    {
        while (true)
        {
            SearchResources();

            yield return _scanRepeatDelay;
        }
    }

    private void SearchResources()
    {
        Collider[] resources = Physics.OverlapSphere(transform.position, _searchRadius, _resourceLayer);

        foreach (Collider collider in resources)
        {
            if (collider.TryGetComponent(out Resource resource))
            {
                if (GetResourceInStorageStatus(collider))
                {
                    continue;
                }

                RegisterAvailableResource(resource);
            }
        }
    }

    private bool GetResourceInStorageStatus(Collider resourceCollider)
    {
        float checkRadius = 0.1f;
        Collider[] colliders = Physics.OverlapSphere(resourceCollider.transform.position, checkRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out ResourcesStorage resourcesStorage))
            {
                return true;
            }
        }

        return false;
    }

    private void RegisterAvailableResource(Resource resource)
    {
        if (_availableResources.ContainsKey(resource.gameObject.GetInstanceID()) == false)
        {
            _availableResources.Add(resource.gameObject.GetInstanceID(), resource);
            FoundAvailableResource?.Invoke(resource.gameObject.GetInstanceID(), resource);
        }
    }

    private void UnregisterAvailableResource(Resource resource)
    {
        if (_availableResources.ContainsKey(resource.gameObject.GetInstanceID()))
        {
            _availableResources.Remove(resource.gameObject.GetInstanceID());
        }
    }

    private void RegisterCollectingResource(Resource resource)
    {
        if (_collectingResources.ContainsKey(resource.gameObject.GetInstanceID()) == false)
        {
            _collectingResources.Add(resource.gameObject.GetInstanceID(), resource);
        }
    }

    private void UnregisterCollectingResource(Resource resource)
    {
        if (_collectingResources.ContainsKey(resource.gameObject.GetInstanceID()))
        {
            _collectingResources.Remove(resource.gameObject.GetInstanceID());
        }
    }
}