using System.Collections;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private int _startCount;

    private UnitPool _unitPool;
    private int _unitCost;
    private Base _owner;
    private bool _isSpawning;
    private WaitForSeconds _spawnDelay;
    private float _delay;

    public void Initialize(Base owner, UnitPool unitPool)
    {
        _unitPool = unitPool;
        _delay = 1;
        _spawnDelay = new(_delay);
        _unitCost = 3;
        _owner = owner;
        _owner.Storage.ValueChanged += OnResourceValueChanged;
        _owner.FlagSetter.FlagStatusChanged += OnFlagStatusChanged;
        SpawnStartCount();
    }

    private void OnDestroy()
    {
        _owner.Storage.ValueChanged -= OnResourceValueChanged;
        _owner.FlagSetter.FlagStatusChanged -= OnFlagStatusChanged;
    }

    private void SpawnStartCount()
    {
        for (int i = 0; i < _startCount; i++)
        {
            _unitPool.GetUint();
        }

        _startCount = 0;
    }

    private void OnFlagStatusChanged()
    {
        StartCoroutine(SpawnUnits());
    }

    private void OnResourceValueChanged()
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

        if (_owner.FlagSetter.Flag == null || _unitPool.UnitsCount <= 1)
        {
            while (_owner.Storage.Count >= _unitCost)
            {
                _owner.Storage.PayResource(_unitCost);
                _unitPool.GetUint();

                yield return _spawnDelay;
            }
        }

        _isSpawning = false;
    }
}