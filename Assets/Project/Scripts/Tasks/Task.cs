using System.Collections.Generic;
using System.Linq;

public class Task
{
    protected Unit Unit;

    protected Queue<ICommand> Commands = new Queue<ICommand>();

    public Queue<ICommand> GetCommands()
    {
        Queue<ICommand> commands = new Queue<ICommand>();

        for (int i = 0; i < Commands.Count; i++)
        {
            commands.Enqueue(Commands.ElementAt(i));
        }

        return commands;
    }

    public virtual void InitializeExecutor(Unit unit)
    {
        Unit = unit;
    }

    public virtual void Interrupt()
    {
        Commands.Clear();
    }
}