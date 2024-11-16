using UnityEngine;
using UnityEngine.AI;

public class UnitMover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _offsetToResource;
    
    private float _defaultOffsetDistance;
    private NavMeshAgent _navMeshAgent;

    private void Awake()
    {
        _defaultOffsetDistance = transform.localScale.y;
    }

    public bool MoveToTarget(Transform targetTransform, MoveTypes moveType)
    {
        float offset = _defaultOffsetDistance;

        switch (moveType)
        {
            case MoveTypes.MoveToStorage:
                offset = GetOffsetToBuilding(targetTransform);
                break;
            case MoveTypes.MoveToResource:
                offset = _offsetToResource;
                break;
        }

        _navMeshAgent.SetDestination(targetTransform.position);
        _navMeshAgent.stoppingDistance = offset - _defaultOffsetDistance;
        float distanceToTarget = Vector3.Distance(targetTransform.position, transform.position);

        if (distanceToTarget <= offset)
        {
            _navMeshAgent.ResetPath();
            return true;
        }

        return false;
    }

    public void Initialize(NavMeshAgent navMeshAgent)
    {
        _navMeshAgent = navMeshAgent;
        _navMeshAgent.speed = _speed;
    }

    private float GetOffsetToBuilding(Transform transform)
    {
        if (transform.gameObject.TryGetComponent(out Building building))
        {
            return building.OccupiedZoneRadius;
        }

        return _defaultOffsetDistance;
    }
}