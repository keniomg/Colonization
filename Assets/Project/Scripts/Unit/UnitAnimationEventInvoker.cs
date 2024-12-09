using System;
using UnityEngine;

public class UnitAnimationEventInvoker : MonoBehaviour
{
    public event Action<AnimationsTypes, bool> AnimationChanged;

    public void Invoke(AnimationsTypes animationsType, bool isOn)
    {
        AnimationChanged(animationsType, isOn);
    }
}