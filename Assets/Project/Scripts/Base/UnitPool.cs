using UnityEngine;
using UnityEngine.Pool;

public class UnitPool : MonoBehaviour
{
    [SerializeField] private Unit _unitPrefab;
    [SerializeField] private int _poolCapacity;
    [SerializeField] private int _poolMaximumSize;
    [SerializeField] private float _spawnOffsetRadius;

    private ObjectPool<Unit> _pool;
    private Base _owner;

    public int UnitsCount { get; private set; }

    public void Initialize(Base owner)
    {
        _pool = new ObjectPool<Unit>(
                createFunc: () => Instantiate(_unitPrefab),
                actionOnGet: (unit) => AccompanyGet(unit),
                actionOnRelease: (unit) => AccompanyRelease(unit),
                actionOnDestroy: (unit) => Destroy(unit.gameObject),
                collectionCheck: true,
                defaultCapacity: _poolCapacity,
                maxSize: _poolMaximumSize);

        UnitsCount = 0;
        _owner = owner;
    }

    public void GetUint()
    {
        _pool.Get();
    }

    private void AccompanyGet(Unit unit)
    {
        UnitsCount++;
        unit.gameObject.SetActive(true);
        unit.transform.position = GetSpawnPosition();
        unit.UnitCommandController.Initialize(_owner.UnitTaskEventInvoker, unit.AnimationEventInvoker);
        unit.Colonizer.Colonized += OnColonized;
    }

    private void AccompanyRelease(Unit unit)
    {
        UnitsCount--;
        unit.gameObject.SetActive(false);
    }

    private void OnColonized()
    {
        UnitsCount--;
    }

    private Vector3 GetSpawnPosition()
    {
        float minimumSpawnDistance = _owner.Building.OccupiedZoneRadius;
        float maximumSpawnDistance = minimumSpawnDistance + _spawnOffsetRadius;
        float spawnDistance = Random.Range(minimumSpawnDistance, maximumSpawnDistance);

        float minimumSpawnAngle = 0;
        float maximumSpawnAngle = Mathf.PI * 2;
        float spawnAngle = Random.Range(minimumSpawnAngle, maximumSpawnAngle);

        Vector3 spawnPosition = _owner.transform.position + new Vector3(Mathf.Cos(spawnAngle), 0, Mathf.Sin(spawnAngle)) * spawnDistance;

        return spawnPosition;
    }
}