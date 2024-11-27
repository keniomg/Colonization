public interface ICommand
{
    public bool IsComplete { get; }
    public bool IsInterrupted { get; }

    public void Execute();
}