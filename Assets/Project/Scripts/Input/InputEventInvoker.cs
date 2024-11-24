using System;
using UnityEngine;

[CreateAssetMenu]
public class InputEventInvoker : ScriptableObject
{
    public event Action LeftMouseClicked;
    public event Action RightMouseClicked;

    public void InvokeLeftMouseClicked()
    {
        LeftMouseClicked?.Invoke();
    }

    public void InvokeRightMouseClicked()
    {
        RightMouseClicked?.Invoke();
    }
}