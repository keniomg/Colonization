using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitCommandController<UnitType> : MonoBehaviour where UnitType : Unit
{
    protected Base Owner;
    protected UnitTaskEventInvoker<UnitType> UnitTaskEventInvoker;

    private UnitType _selfUnit;
    private Queue<ICommand> _commands = new Queue<ICommand>();
    private ICommand _currentCommand;

    private void Awake()
    {
        _selfUnit = TryGetComponent(out UnitType unit) ? unit : null;
    }

    public abstract void Initialize(Base ownBase);

    public void AddCommand(ICommand command)
    {
        _commands.Enqueue(command);
    }

    public void AddTask(Task task)
    {
        task.InitializeExecutor(_selfUnit);

        foreach (ICommand command in task.GetCommands())
        {
            AddCommand(command);
        }

        UnitTaskEventInvoker.Invoke(_selfUnit, UnitTaskStatusTypes.Busy);
        StartCoroutine(HandleTask());
    }

    protected IEnumerator HandleTask()
    {
        while (_commands.Count > 0)
        {
            if (_currentCommand == null)
            {
                _currentCommand = _commands.Dequeue();
            }

            if (_currentCommand != null)
            {
                _currentCommand.Execute();

                if (_currentCommand.IsComplete == false)
                {
                    if (_currentCommand.IsInterrupted)
                    {
                        _commands.Clear();
                        _currentCommand = null;
                    }
                }
                else
                {
                    _currentCommand = null;
                }

                yield return null;
            }
        }

        UnitTaskEventInvoker.Invoke(_selfUnit, UnitTaskStatusTypes.Free);
    }
}