using UnityEngine;
using UnityEngine.Pool;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private Unit _collectorPrefab;
    [SerializeField] private Unit _coloniztorPrefab;
    [SerializeField] private int _poolCapacity;
    [SerializeField] private int _poolMaximumSize;
    [SerializeField] private int _startUnitsCount;
    [SerializeField] private float _spawnOffsetRadius;

    private ResourcesStorage _resourcesStorage;
    private Base _owner;
    private FlagSetter _flagSetter;

    private ObjectPool<Unit> _collectorPool;
    private ObjectPool<Unit> _colonizatorPool;

    public void Initialize(Base owner, FlagSetter flagSetter)
    {
        _collectorPool = new ObjectPool<Unit>(
                createFunc: () => Instantiate(_collectorPrefab),
                actionOnGet: (unit) => AccompanyGet(unit),
                actionOnRelease: (unit) => AccompanyRelease(unit),
                actionOnDestroy: (unit) => Destroy(unit),
                collectionCheck: true,
                defaultCapacity: _poolCapacity,
                maxSize: _poolMaximumSize);
        
        _colonizatorPool = new ObjectPool<Unit>(
                createFunc: () => Instantiate(_coloniztorPrefab),
                actionOnGet: (unit) => AccompanyGet(unit),
                actionOnRelease: (unit) => AccompanyRelease(unit),
                actionOnDestroy: (unit) => Destroy(unit),
                collectionCheck: true,
                defaultCapacity: _poolCapacity,
                maxSize: _poolMaximumSize);

        _owner = owner;
        _resourcesStorage = _owner.Storage;
        _flagSetter = _owner.FlagSetter;
        //_flagSetter.FlagStatusChanged
        SpawnCollectorsCount(_startUnitsCount);
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

    private void OnFlagStatusChanged(bool isSet)
    {

    }

    private void SpawnColonizator()
    {
        _colonizatorPool.Get();
    }

    private void SpawnCollector()
    {
        _collectorPool.Get();
    }

    private void SpawnCollectorsCount(int unitsCount)
    {
        for (int i = 0; i < _startUnitsCount; i++)
        {
            SpawnCollector();
        }
    }
}