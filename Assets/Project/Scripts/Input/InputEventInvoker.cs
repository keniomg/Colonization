using System;
using UnityEngine;

[CreateAssetMenu]
public class InputEventInvoker : ScriptableObject
{
    public event Action Choosed;
    public event Action FlagSetted;

    public void InvokeChoosed()
    {
        Choosed?.Invoke();
    }

    public void InvokeFlagSetted()
    {
        FlagSetted?.Invoke();
    }
}