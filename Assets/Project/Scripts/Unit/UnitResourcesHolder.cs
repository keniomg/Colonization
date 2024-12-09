using System;
using UnityEngine;

public class UnitResourcesHolder : MonoBehaviour
{
    [SerializeField] private float _actionDistanceOffset;
    [SerializeField] private Transform _holdingPoint;

    private Resource _holdingResource;

    public event Action<bool> HoldingStatusChanged;

    public bool TakeResource(Resource resource)
    {
        if (resource != null)
        {
            if (_holdingResource == null)
            {
                _holdingResource = resource;
                HoldingStatusChanged?.Invoke(true);
                PlaceHoldingResource(resource);

                return true;
            }
        }

        return false;
    }

    public bool GiveResource(Resource resource, ResourcesStorage storage)
    {
        if (_holdingResource != null)
        {
            DropResource(resource);
            storage.PlaceResource(resource);
            _holdingResource = null;
            HoldingStatusChanged?.Invoke(false);

            return true;
        }

        return false;
    }

    private void PlaceHoldingResource(Resource resource)
    {
        resource.transform.SetParent(_holdingPoint);
        resource.transform.localPosition = Vector3.zero;
        resource.transform.localRotation = Quaternion.identity;

        if (resource.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;
        }

        if (resource.TryGetComponent(out BoxCollider collider))
        {
            collider.enabled = false;
        }
    }

    private void DropResource(Resource resource)
    {
        resource.transform.SetParent(null);

        if (resource.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.isKinematic = false;
            rigidbody.useGravity = true;
        }
    }
}