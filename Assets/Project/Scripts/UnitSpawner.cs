using UnityEngine;
using UnityEngine.Pool;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private Unit _prefab;
    [SerializeField] private int _poolCapacity;
    [SerializeField] private int _poolMaximumSize;
    [SerializeField] private int _startUnitsCount;
    [SerializeField] private float _spawnOffsetRadius;

    private Base _owner;

    private ObjectPool<Unit> _pool;

    private void Awake()
    {
    }

    public void Initialize(Base owner)
    {
        _pool = new ObjectPool<Unit>(
                createFunc: () => Instantiate(_prefab),
                actionOnGet: (unit) => AccompanyGet(unit),
                actionOnRelease: (unit) => AccompanyRelease(unit),
                actionOnDestroy: (unit) => Destroy(unit),
                collectionCheck: true,
                defaultCapacity: _poolCapacity,
                maxSize: _poolMaximumSize);
        
        _owner = owner;
        SpawnUnitsCount(_startUnitsCount);
    }

    private Vector3 GetSpawnPosition()
    {
        float minimumSpawnDistance = _owner.OccupiedZoneRadius;
        float maximumSpawnDistance = minimumSpawnDistance + _spawnOffsetRadius;
        float spawnDistance = Random.Range(minimumSpawnDistance, maximumSpawnDistance);

        float minimumSpawnAngle = 0;
        float maximumSpawnAngle = Mathf.PI * 2;
        float spawnAngle = Random.Range(minimumSpawnAngle, maximumSpawnAngle);

        Vector3 spawnPosition = _owner.transform.position + new Vector3(Mathf.Cos(spawnAngle), 0, Mathf.Sin(spawnAngle)) * spawnDistance;
        
        return spawnPosition;
    }

    private void AccompanyGet(Unit unit)
    {
        unit.gameObject.SetActive(true);
        unit.CommandController.Initialize(_owner);
        unit.transform.position = GetSpawnPosition();
    }

    private void AccompanyRelease(Unit unit)
    {
        unit.gameObject.SetActive(false);
    }

    private void SpawnUnit()
    {
        _pool.Get();
    }

    private void SpawnUnitsCount(int unitsCount)
    {
        for (int i = 0; i < _startUnitsCount; i++)
        {
            SpawnUnit();
        }
    }
}