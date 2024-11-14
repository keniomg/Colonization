using UnityEngine;

[RequireComponent(typeof(SphereCollider))]

public class Building : MonoBehaviour
{
    private SphereCollider _sphereCollider;

    public float OccupiedZoneRadius { get; private set; }

    protected virtual void Awake()
    {
        GetOccupiedZoneRadius();
    }

    private void GetOccupiedZoneRadius()
    {
        if (_sphereCollider.TryGetComponent(out SphereCollider capsuleCollider))
        {
            OccupiedZoneRadius = _sphereCollider.radius;
        }
    }
}
