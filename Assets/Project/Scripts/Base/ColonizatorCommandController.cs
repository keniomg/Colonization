public class ColonizatorCommandController : UnitCommandController<Colonizator> 
{
    public override void Initialize(Base ownBase)
    {
        Owner = ownBase;
        UnitTaskEventInvoker = Owner.ColonizatorTaskEventInvoker;
        StartCoroutine(HandleTask());
    }
}