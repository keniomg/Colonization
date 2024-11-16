using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCommandController : MonoBehaviour
{
    private Base _owner;
    private Unit _selfUnit;
    private UnitTaskEventInvoker _unitTaskEventInvoker;

    public Queue<ICommand> Commands { get; private set; }
    public ICommand CurrentCommand { get; private set; }

    private void Awake()
    {
        _selfUnit = TryGetComponent(out Unit unit) ? unit : null;
        Commands = new Queue<ICommand>();
    }

    public void Initialize(Base ownBase)
    {
        _owner = ownBase;
        _unitTaskEventInvoker = _owner.UnitTaskEventInvoker;
        StartCoroutine(HandleTask());
    }

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

        _unitTaskEventInvoker.Invoke(_selfUnit, UnitTaskStatusTypes.Busy);
        StartCoroutine(HandleTask());
    }

    private IEnumerator HandleTask()
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

        _unitTaskEventInvoker.Invoke(_selfUnit, UnitTaskStatusTypes.Free);
    }
}