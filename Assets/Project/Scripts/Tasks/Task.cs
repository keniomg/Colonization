using System.Collections.Generic;

public class Task
{
    protected Unit Unit;

    public Queue<ICommand> Commands { get; protected set; } = new Queue<ICommand>();

    public virtual void InitializeExecutor(Unit unit)
    {
        Unit = unit;
    }
}