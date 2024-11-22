public class CollectorCommandController : UnitCommandController<Collector> 
{
    public override void Initialize(Base ownBase)
    {
        Owner = ownBase;
        UnitTaskEventInvoker = Owner.CollectorTaskEventInvoker;
        StartCoroutine(HandleTask());
    }
}