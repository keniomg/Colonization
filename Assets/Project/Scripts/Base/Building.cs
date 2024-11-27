using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Building : MonoBehaviour
{
    public float OccupiedZoneRadius { get; private set; }

    protected virtual void Awake()
    {
        GetOccupiedZoneRadius();
    }

    public Map GetMap()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, OccupiedZoneRadius);

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent(out Map map))
            {
                return map;
            }
        }

        return null;
    }

    private void GetOccupiedZoneRadius()
    {
        if (TryGetComponent(out SphereCollider sphereCollider))
        {
            OccupiedZoneRadius = sphereCollider.radius;
        }
    }
}