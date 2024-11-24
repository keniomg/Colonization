using System.Collections;
using UnityEngine;

public class CollectorSpawner : UnitSpawner<Collector>
{
    [SerializeField] private int _startCount;

    public override void Initialize(Base owner)
    {
        base.Initialize(owner);
        SpawnStartCount();
    }

    protected override void AccompanyGet(Collector unit)
    {
        base.AccompanyGet(unit);
        unit.CollectorCommandController.Initialize(Owner);
    }

    protected override IEnumerator SpawnUnit()
    {
        while (Owner.FlagSetter.Flag == null && Owner.Storage.Count >= UnitCost)
        {
            StartCoroutine(base.SpawnUnit());
        }

        yield return SpawnDelaySeconds;
    }

    private void SpawnStartCount()
    {
        for (int i = 0; i < _startCount; i++)
        {
            Pool.Get();
        }
    }
}