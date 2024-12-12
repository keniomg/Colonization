using UnityEngine;
using UnityEngine.AI;

public class UnitMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private float _defaultOffsetDistance;
    private NavMeshAgent _navMeshAgent;

    public void Initialize(NavMeshAgent navMeshAgent)
    {
        _navMeshAgent = navMeshAgent;
        _navMeshAgent.speed = _speed;
        _defaultOffsetDistance = 5;
    }

    public bool CanMoveToResource(Transform resourceTransform, ref bool isInterrupted)
    {
        if (resourceTransform.parent != null)
        {
            isInterrupted = true;
            return false;
        }

        return CanMoveToTarget(resourceTransform);
    }

    public bool CanMoveToTarget(Transform targetTransform, float offset = 0)
    {
        if (targetTransform != null)
        {
            offset += _defaultOffsetDistance;

            return CanMoveToPoint(targetTransform.position, offset);
        }

        return false;
    }

    public bool CanMoveToPoint(Vector3 position, float offset)
    {
        _navMeshAgent.SetDestination(position);
        _navMeshAgent.stoppingDistance = _defaultOffsetDistance;

        if (transform.position.IsEnoughDistance(position, offset))
        {
            _navMeshAgent.ResetPath();

            return true;
        }

        return false;
    }
}