using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    protected Unit Unit;

    public Queue<ICommand> Commands { get; protected set; }

    public void InitializeExecutor(Unit unit)
    {
        Unit = unit;
    }
}