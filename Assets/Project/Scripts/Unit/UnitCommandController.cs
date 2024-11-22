using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitCommandController<UnitType> : MonoBehaviour where UnitType : Unit
{
    protected Base Owner;
    protected UnitTaskEventInvoker<UnitType> UnitTaskEventInvoker;
    
    private UnitType _selfUnit;

    public Queue<ICommand> Commands { get; private set; }
    public ICommand CurrentCommand { get; private set; }

    private void Awake()
    {
        _selfUnit = TryGetComponent(out UnitType unit) ? unit : null;
        Commands = new Queue<ICommand>();
    }

    public abstract void Initialize(Base ownBase);

    public void AddCommand(ICommand command)
    {
        Commands.Enqueue(command);
    }

    public void AddTask(Task task)
    {
        task.InitializeExecutor(_selfUnit);

        foreach (ICommand command in task.Commands)
        {
            AddCommand(command);
        }

        UnitTaskEventInvoker.Invoke(_selfUnit, UnitTaskStatusTypes.Busy);
        StartCoroutine(HandleTask());
    }

    protected IEnumerator HandleTask()
    {
        while (Commands.Count > 0)
        {
            if (CurrentCommand == null)
            {
                CurrentCommand = Commands.Dequeue();
            }

            if (CurrentCommand != null)
            {
                CurrentCommand.Execute();

                if (CurrentCommand.IsComplete)
                {
                    CurrentCommand = null;
                }

                yield return null;
            }
        }

        UnitTaskEventInvoker.Invoke(_selfUnit, UnitTaskStatusTypes.Free);
    }
}