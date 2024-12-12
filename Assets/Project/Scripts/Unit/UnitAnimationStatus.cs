using UnityEngine;

public class UnitAnimationStatus : MonoBehaviour
{
    private UnitAnimationEventInvoker _unitAnimationEventInvoker;

    public bool IsHolding {get; private set; }
    public bool IsWalking {get; private set; }

    public void Initialize(UnitAnimationEventInvoker unitAnimationEventInvoker)
    {
        _unitAnimationEventInvoker = unitAnimationEventInvoker;
        _unitAnimationEventInvoker.AnimationChanged += OnAnimationChanged;
    }

    private void OnDestroy()
    {
        _unitAnimationEventInvoker.AnimationChanged -= OnAnimationChanged;
    }

    private void OnAnimationChanged(AnimationsTypes animationsType, bool isOn)
    {
        switch (animationsType)
        {
            case AnimationsTypes.Walk:
                IsWalking = isOn;
                break;
            case AnimationsTypes.Hold:
                IsHolding = isOn;
                break;
            default:
                ResetAnimationStatus();
                break;
        }
    }

    private void ResetAnimationStatus()
    {
        IsWalking = false;
        IsHolding = false;
    }
}