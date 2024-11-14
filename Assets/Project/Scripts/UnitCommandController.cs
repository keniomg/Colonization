using System.Collections.Generic;
using UnityEngine;

public class UnitCommandController : MonoBehaviour
{
    private UnitTasker _unitTasker;

    private Unit _selfUnit;
    private UnitTaskEventInvoker _unitTaskEventInvoker;

    public Queue<ICommand> Commands { get; private set; }
    public ICommand CurrentCommand { get; private set; }

    private void Awake()
    {
        _selfUnit = TryGetComponent(out Unit unit) ? unit : null;
        Commands = new Queue<ICommand>();
    }


    public void Initialize(UnitTasker unitTasker)
    {
        _unitTasker = unitTasker;
        _unitTaskEventInvoker = _unitTasker.UnitTaskEventInvoker;
        HandleTask();
    }

    public void AddCommand(ICommand command)
    {
        Commands.Enqueue(command);
    }

    public void AddTask(Task task)
    {
        foreach (ICommand command in task.Commands)
        {
            AddCommand(command);
        }

        _unitTaskEventInvoker.Invoke(_selfUnit, UnitStatusTypes.Busy);
        HandleTask();
    }

    private void HandleTask()
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
            }
        }

        _unitTaskEventInvoker.Invoke(_selfUnit, UnitStatusTypes.Free);
    }
}
