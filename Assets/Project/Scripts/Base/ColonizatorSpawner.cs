public class ColonizatorSpawner : UnitSpawner<Colonizator>
{
    protected override void AccompanyGet(Colonizator unit)
    {
        base.AccompanyGet(unit);
        unit.BaseBuilded += OnBaseBuilded;
    }

    protected override void AccompanyRelease(Colonizator unit)
    {
        unit.BaseBuilded -= OnBaseBuilded;
        base.AccompanyRelease(unit);
    }

    protected override void SpawnUnit()
    {
        if (Owner.FlagSetter.Flag != null)
        {
            base.SpawnUnit();
        }
    }

    private void OnBaseBuilded(Colonizator unit)
    {
        Pool.Release(unit);
    }
}