using UnityEngine;

public class UnitMover : MonoBehaviour
{
    [SerializeField] private float _defaultOffsetDistance;
    [SerializeField] private float _speed;
    [SerializeField] private float _offsetToResource;
    [SerializeField] private float _offsetToBase;

    public bool MoveToTarget(Vector3 targetPosition, MoveTypes moveType)
    {
        float offset = _defaultOffsetDistance;

        switch (moveType)
        {
            case MoveTypes.MoveToStorage:
                offset = _offsetToBase;
                break;
            case MoveTypes.MoveToResource:
                offset = _offsetToResource;
                break;
        }

        float distanceToTarget = Vector3.Distance(targetPosition, transform.position);
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;
        transform.Translate(directionToTarget * _speed * Time.deltaTime, Space.World);

        if (distanceToTarget <= offset)
        {
            return true;
        }

        return false;
    }
}

