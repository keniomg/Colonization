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
    private Dictionary<int, Resource> _availableResources = new();
    private Dictionary<int, Resource> _collectingResources = new();

    public event Action<Resource> FoundResource;

    public void Initialize(ResourcesEventInvoker resourcesEventInvoker)
    {
        _scanRepeatDelay = new(_delay);
        _resourcesEventInvoker = resourcesEventInvoker;

        StartCoroutine(Scanning());
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
                FoundResource?.Invoke(resource);
            }
        }
    }
}