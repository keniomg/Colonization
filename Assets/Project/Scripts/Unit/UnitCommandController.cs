using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCommandController : MonoBehaviour
{
    private UnitTaskEventInvoker _unitTaskEventInvoker;
    private UnitAnimationEventInvoker _unitAnimationEventInvoker;
    private Unit _selfUnit;
    private Queue<ICommand> _commands = new();
    private ICommand _currentCommand;

    public void Initialize(UnitTaskEventInvoker unitTaskEventInvoker, UnitAnimationEventInvoker unitAnimationEventInvoker)
    {
        _selfUnit ??= TryGetComponent(out Unit unit) ? unit : null;
        _unitTaskEventInvoker = unitTaskEventInvoker;
        _unitAnimationEventInvoker = unitAnimationEventInvoker;
        StartCoroutine(HandleTask());
    }

    public void AddCommand(ICommand command)
    {
        _commands.Enqueue(command);
    }

    public void AddTask(Task task)
    {
        _unitTaskEventInvoker.Invoke(_selfUnit, UnitTaskStatusTypes.Busy);
        task.InitializeExecutor(_selfUnit);

        foreach (ICommand command in task.GetCommands())
        {
            AddCommand(command);
        }

        StartCoroutine(HandleTask());
    }

    protected IEnumerator HandleTask()
    {
        while (_commands.Count > 0)
        {
            _currentCommand ??= _commands.Dequeue();

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
        _unitAnimationEventInvoker.Invoke(AnimationsTypes.Idle, true);
    }
}