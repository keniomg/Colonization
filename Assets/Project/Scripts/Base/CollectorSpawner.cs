using UnityEngine;

public class CollectorSpawner : UnitSpawner<Collector>
{
    [SerializeField] private int _startCount;

    public override void Initialize(Base owner)
    {
        base.Initialize(owner);
        SpawnStartCount();
    }

    protected override void SpawnUnit()
    {
        if (Owner.FlagSetter.Flag == null)
        {
            base.SpawnUnit();
        }
    }

    private void SpawnStartCount()
    {
        for (int i = 0; i < _startCount; i++)
        {
            Pool.Get();
        }
    }
}