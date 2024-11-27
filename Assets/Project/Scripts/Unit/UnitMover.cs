using UnityEngine;
using UnityEngine.AI;

public class UnitMover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _offsetToPoint;

    private float _defaultOffsetDistance;
    private NavMeshAgent _navMeshAgent;

    private void Awake()
    {
        _defaultOffsetDistance = transform.localScale.y;
    }

    public bool MoveToTarget(Transform targetTransform, ref bool isInterrupted)
    {
        if (targetTransform != null)
        {
            float offset = _offsetToPoint;

            _navMeshAgent.SetDestination(targetTransform.position);
            _navMeshAgent.stoppingDistance = offset - _defaultOffsetDistance;

            if (transform.position.IsEnoughDistance(targetTransform.position, offset))
            {
                _navMeshAgent.ResetPath();

                return true;
            }
        }
        else
        {
            isInterrupted = true;
        }

        return false;
    }

    public bool MoveToPoint(Vector3 position, Building building = null)
    {
        float offset = _defaultOffsetDistance;

        if (building != null)
        {
            offset = building.OccupiedZoneRadius + _defaultOffsetDistance;
        }

        _navMeshAgent.SetDestination(position);
        _navMeshAgent.stoppingDistance = _defaultOffsetDistance;

        if (transform.position.IsEnoughDistance(position, offset))
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
}