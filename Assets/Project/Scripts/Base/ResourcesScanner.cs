using UnityEngine;
using System;
using System.Collections;

public class ResourcesScanner : MonoBehaviour
{
    [SerializeField] private float _searchRadius;
    [SerializeField] private LayerMask _resourceLayer;
    [SerializeField] private float _delay;

    private WaitForSeconds _scanRepeatDelay;
    private bool _isScanEnable;

    public event Action<Resource> FoundResource;

    public void Initialize()
    {
        _scanRepeatDelay = new(_delay);
        _isScanEnable = true;
        StartCoroutine(Scanning());
    }

    private IEnumerator Scanning()
    {
        while (_isScanEnable)
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