using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public abstract class UnitSpawner<UnitType> : MonoBehaviour where UnitType : Unit
{
    [SerializeField] protected int UnitCost;

    [SerializeField] private UnitType _unitPrefab;
    [SerializeField] private int _poolCapacity;
    [SerializeField] private int _poolMaximumSize;
    [SerializeField] private float _spawnOffsetRadius;

    private float _spawnDelay;

    protected WaitForSeconds SpawnDelaySeconds;
    protected ObjectPool<UnitType> Pool;
    protected Base Owner;

    public virtual void Initialize(Base owner)
    {
        Pool = new ObjectPool<UnitType>(
                createFunc: () => Instantiate(_unitPrefab),
                actionOnGet: (unit) => AccompanyGet(unit),
                actionOnRelease: (unit) => AccompanyRelease(unit),
                actionOnDestroy: (unit) => Destroy(unit),
                collectionCheck: true,
                defaultCapacity: _poolCapacity,
                maxSize: _poolMaximumSize);

        SpawnDelaySeconds = new(_spawnDelay);
        Owner = owner;
        Owner.Storage.ValueChanged += OnResourcesChanged;
        Owner.FlagSetter.FlagStatusChanged += OnFlagStatusChanged;
    }

    protected virtual void AccompanyGet(UnitType unit)
    {
        unit.gameObject.SetActive(true);
        unit.transform.position = GetSpawnPosition();
    }

    protected virtual IEnumerator SpawnUnit()
    {
        Owner.Storage.PayResource(UnitCost);
        Pool.Get();

        yield return null;
    }

    protected virtual void AccompanyRelease(UnitType unit)
    {
        unit.gameObject.SetActive(false);
    }

    private Vector3 GetSpawnPosition()
    {
        float minimumSpawnDistance = Owner.Building.OccupiedZoneRadius;
        float maximumSpawnDistance = minimumSpawnDistance + _spawnOffsetRadius;
        float spawnDistance = Random.Range(minimumSpawnDistance, maximumSpawnDistance);

        float minimumSpawnAngle = 0;
        float maximumSpawnAngle = Mathf.PI * 2;
        float spawnAngle = Random.Range(minimumSpawnAngle, maximumSpawnAngle);

        Vector3 spawnPosition = Owner.transform.position + new Vector3(Mathf.Cos(spawnAngle), 0, Mathf.Sin(spawnAngle)) * spawnDistance;

        return spawnPosition;
    }

    private void OnFlagStatusChanged()
    {
        StartCoroutine(SpawnUnit());
    }

    private void OnResourcesChanged()
    {
        StartCoroutine(SpawnUnit());
    }
}