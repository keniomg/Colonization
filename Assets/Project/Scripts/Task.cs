using System.Collections.Generic;
using UnityEngine;

public abstract class Task : MonoBehaviour 
{
    public Queue<ICommand> Commands {get; private set; }
}
