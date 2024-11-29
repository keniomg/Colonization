using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCommandController : MonoBehaviour
{
    private UnitTaskEventInvoker _unitTaskEventInvoker;
    private Unit _selfUnit;
    private Queue<ICommand> _commands = new Queue<ICommand>();
    private ICommand _currentCommand;

    private void Awake()
    {
        _selfUnit = TryGetComponent(out Unit unit) ? unit : null;
    }

    public void Initialize(UnitTaskEventInvoker unitTaskEventInvoker)
    {
        _unitTaskEventInvoker = unitTaskEventInvoker;
        StartCoroutine(HandleTask());
    }

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

        _unitTaskEventInvoker.Invoke(_selfUnit, UnitTaskStatusTypes.Busy);
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

        _unitTaskEventInvoker.Invoke(_selfUnit, UnitTaskStatusTypes.Free);
    }
}