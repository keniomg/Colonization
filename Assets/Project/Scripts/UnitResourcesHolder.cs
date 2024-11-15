using UnityEngine;

public class UnitResourcesHolder : MonoBehaviour
{
    [SerializeField] private float _actionDistanceOffset;
    [SerializeField] private Transform _holdingPoint;

    private Resource _holdingResource;

    public bool TakeResource(Resource resource)
    {
        float distanceToTarget = Vector3.Distance(resource.transform.position, transform.position);

        if (_holdingResource == null && distanceToTarget < _actionDistanceOffset)
        {
            _holdingResource = resource;
            PlaceHoldingResource(resource);
            return true;
        }

        return false;
    }

    public bool GiveResource(Resource resource, ResourcesStorage storage)
    {
        float distanceToTarget = Vector3.Distance(storage.transform.position, transform.position);

        if (_holdingResource != null && distanceToTarget < _actionDistanceOffset)
        {
            storage.TakeResource(resource);
            _holdingResource = null;
            return true;
        }

        return false;
    }

    private void PlaceHoldingResource(Resource resource)
    {
        resource.transform.SetParent(_holdingPoint);
        resource.transform.localPosition = Vector3.zero;
        resource.transform.localRotation = Quaternion.identity;
    }
}