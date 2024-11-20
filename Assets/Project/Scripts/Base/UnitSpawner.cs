using UnityEngine;
using UnityEngine.Pool;

public abstract class UnitSpawner<UnitType> : MonoBehaviour where UnitType : Unit
{
    [SerializeField] private UnitType _unitPrefab;
    [SerializeField] private int _poolCapacity;
    [SerializeField] private int _poolMaximumSize;
    [SerializeField] private int _unitCost;
    [SerializeField] private float _spawnOffsetRadius;

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
        
        Owner = owner;
        Owner.Storage.ValueChanged += OnResourcesChanged;
        Owner.FlagSetter.FlagStatusChanged += OnFlagStatusChanged;
    }

    private Vector3 GetSpawnPosition()
    {
        float minimumSpawnDistance = Owner.OccupiedZoneRadius;
        float maximumSpawnDistance = minimumSpawnDistance + _spawnOffsetRadius;
        float spawnDistance = Random.Range(minimumSpawnDistance, maximumSpawnDistance);

        float minimumSpawnAngle = 0;
        float maximumSpawnAngle = Mathf.PI * 2;
        float spawnAngle = Random.Range(minimumSpawnAngle, maximumSpawnAngle);

        Vector3 spawnPosition = Owner.transform.position + new Vector3(Mathf.Cos(spawnAngle), 0, Mathf.Sin(spawnAngle)) * spawnDistance;
        
        return spawnPosition;
    }

    protected virtual void AccompanyGet(UnitType unit)
    {
        unit.gameObject.SetActive(true);
        unit.CommandController.Initialize(Owner);
        unit.transform.position = GetSpawnPosition();
    }

    protected virtual void SpawnUnit()
    {
        if (Owner.Storage.Count >= _unitCost)
        {
            Pool.Get();
        }
    }

    protected virtual void AccompanyRelease(UnitType unit)
    {
        unit.gameObject.SetActive(false);
    }

    private void OnFlagStatusChanged()
    {
        SpawnUnit();
    }

    private void OnResourcesChanged()
    {
        SpawnUnit();
    }
}