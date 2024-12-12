using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class ResourcesSpawner : MonoBehaviour
{
    [SerializeField] private ResourcesEventInvoker _resourcesEventInvoker;
    [SerializeField] private Resource _resourcePrefab;
    [SerializeField] private Transform[] _spawnPositions;
    [SerializeField] private int _poolCapacity;
    [SerializeField] private int _poolMaximumSize;
    [SerializeField] private float _resourcesSpawnDelay;

    private WaitForSeconds _spawnDelay;
    private ObjectPool<Resource> _pool;
    private bool _isSpawnEnabled;

    private void Awake()
    {
        _spawnDelay = new(_resourcesSpawnDelay);
        _isSpawnEnabled = true;

        _pool = new ObjectPool<Resource>(
              createFunc: () => Instantiate(_resourcePrefab),
              actionOnGet: (resource) => AccompanyGet(resource),
              actionOnRelease: (resource) => AccompanyRelease(resource),
              actionOnDestroy: (resource) => Destroy(resource.gameObject),
              collectionCheck: true,
              defaultCapacity: _poolCapacity,
              maxSize: _poolMaximumSize);
    }

    private void OnEnable()
    {
        _resourcesEventInvoker.ResourceReturned += _pool.Release;
    }

    private void OnDisable()
    {
        _resourcesEventInvoker.ResourceReturned -= _pool.Release;
    }

    private void Start()
    {
        StartCoroutine(SpawnResources());
    }

    private void AccompanyGet(Resource resource)
    {
        resource.gameObject.SetActive(true);
        resource.transform.position = GetSpawnPosition();
        resource.transform.rotation = Quaternion.identity;
    }

    private void AccompanyRelease(Resource resource)
    {
        resource.gameObject.SetActive(false);
    }

    private IEnumerator SpawnResources()
    {
        while (_isSpawnEnabled)
        {
            yield return _spawnDelay;

            _pool.Get();
        }
    }

    private Vector3 GetSpawnPosition()
    {
        int minimumSpawnPositionsIndex = 0;
        int maximumSpawnPositionsIndex = _spawnPositions.Length;
        int spawnPositionIndex = UnityEngine.Random.Range(minimumSpawnPositionsIndex, maximumSpawnPositionsIndex);
        return _spawnPositions[spawnPositionIndex].position;
    }
}