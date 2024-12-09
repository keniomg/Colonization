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
        _defaultOffsetDistance = transform.localScale.y;
    }

    public bool MoveToResource(Transform resourceTransform, ref bool isInterrupted)
    {
        if (resourceTransform.parent != null)
        {
            isInterrupted = true;
        }

        return MoveToTarget(resourceTransform);
    }

    public bool MoveToTarget(Transform targetTransform, float offset = 0)
    {
        if (targetTransform != null)
        {
            offset = offset == 0 ? _defaultOffsetDistance : (offset + _defaultOffsetDistance);

            return MoveToPoint(targetTransform.position, offset);
            //_navMeshAgent.SetDestination(targetTransform.position);
            //_navMeshAgent.stoppingDistance = _defaultOffsetDistance;

            //if (transform.position.IsEnoughDistance(targetTransform.position, offset))
            //{
            //    _navMeshAgent.ResetPath();

            //    return true;
            //}
        }

        return false;
    }

    public bool MoveToPoint(Vector3 position, float offset)
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