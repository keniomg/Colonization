using System.Collections;

public class ColonizatorSpawner : UnitSpawner<Colonizator>
{
    protected override void AccompanyGet(Colonizator unit)
    {
        base.AccompanyGet(unit);
        unit.ColonizatorCommandController.Initialize(Owner);
        unit.Colonizer.Colonized += OnColonized;
    }

    protected override void AccompanyRelease(Colonizator unit)
    {
        unit.Colonizer.Colonized -= OnColonized;
        base.AccompanyRelease(unit);
    }

    protected override IEnumerator SpawnUnit()
    {
        if (Owner.Storage.Count >= UnitCost && Owner.FlagSetter.Flag != null)
        {
            StartCoroutine(base.SpawnUnit());
        }

        yield return SpawnDelaySeconds;
    }

    private void OnColonized(Colonizator unit)
    {
        Pool.Release(unit);
    }
}