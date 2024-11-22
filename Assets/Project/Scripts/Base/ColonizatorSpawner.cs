using System.Collections;

public class ColonizatorSpawner : UnitSpawner<Colonizator>
{
    protected override void AccompanyGet(Colonizator unit)
    {
        base.AccompanyGet(unit);
        unit.ColonizatorCommandController.Initialize(Owner);
        unit.Colonizer.Colonized += OnBaseBuilded;
    }

    protected override void AccompanyRelease(Colonizator unit)
    {
        unit.Colonizer.Colonized -= OnBaseBuilded;
        base.AccompanyRelease(unit);
    }

    protected override IEnumerator SpawnUnit()
    {
        while (Owner.Storage.Count >= UnitCost && Owner.FlagSetter.Flag != null)
        {
            base.SpawnUnit();
        }

        yield return SpawnDelaySeconds;
    }

    private void OnBaseBuilded(Colonizator unit)
    {
        Pool.Release(unit);
    }
}