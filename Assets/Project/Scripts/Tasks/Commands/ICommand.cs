public interface ICommand
{
    public bool IsComplete { get; }

    public void Execute();
}
