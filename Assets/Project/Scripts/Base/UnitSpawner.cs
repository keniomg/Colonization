using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private Unit _unitPrefab;
    [SerializeField] private int _poolCapacity;
    [SerializeField] private int _poolMaximumSize;
    [SerializeField] private float _spawnOffsetRadius;
    [SerializeField] private int _startCount;

    private int _unitCost;
    private ObjectPool<Unit> _pool;
    private Base _owner;
    private bool _isSpawning;
    private WaitForSeconds _spawnDelay;
    private float _delay;

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

        _delay = 1;
        _spawnDelay = new WaitForSeconds(_delay);
        UnitsCount = 0;
        _unitCost = 3;
        _owner = owner;
        _owner.Storage.ValueChanged += SpawnRes;
        _owner.FlagSetter.FlagStatusChanged += SpawnFlag;
        SpawnStartCount();
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

    private void SpawnStartCount()
    {
        for (int i = 0; i < _startCount; i++)
        {
            _pool.Get();
        }

        _startCount = 0;
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

    private void SpawnFlag()
    {
        StartCoroutine(SpawnUnits());
    }

    private void SpawnRes()
    {
        StartCoroutine(SpawnUnits());
    }

    private IEnumerator SpawnUnits()
    {
        if (_isSpawning == true)
        {
            yield break;
        }

        _isSpawning = true;

        if (_owner.FlagSetter.Flag == null || UnitsCount <= 1)
        {
            while (_owner.Storage.Count >= _unitCost)
            {
                _owner.Storage.PayResource(_unitCost);
                _pool.Get();

                yield return _spawnDelay;
            }
        }

        _isSpawning = false;
    }
}