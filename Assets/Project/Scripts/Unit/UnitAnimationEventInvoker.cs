using System;
using UnityEngine;

[CreateAssetMenu]
public class UnitAnimationEventInvoker : ScriptableObject
{
    public event Action<AnimationsTypes, bool> AnimationChanged;

    public void Invoke(AnimationsTypes animationsType, bool isOn)
    {
        AnimationChanged(animationsType, isOn);
    }
}