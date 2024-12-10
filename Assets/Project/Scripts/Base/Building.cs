using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Building : MonoBehaviour
{
    public float OccupiedZoneRadius { get; private set; }

    private void Awake()
    {
        GetOccupiedZoneRadius();
    }

    private void GetOccupiedZoneRadius()
    {
        if (TryGetComponent(out SphereCollider sphereCollider))
        {
            OccupiedZoneRadius = sphereCollider.radius;
        }
    }
}