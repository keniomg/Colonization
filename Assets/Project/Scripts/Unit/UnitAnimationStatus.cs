using UnityEngine;

public class UnitAnimationStatus : MonoBehaviour
{
    private UnitAnimationEventInvoker _unitAnimationEventInvoker;

    public bool IsHolding {get; private set; }
    public bool IsWalking {get; private set; }

    private void OnEnable()
    {
        if (_unitAnimationEventInvoker != null)
        {
            _unitAnimationEventInvoker.AnimationChanged += OnAnimationChanged;
        }
    }

    private void OnDisable()
    {
        _unitAnimationEventInvoker.AnimationChanged -= OnAnimationChanged;
    }

    public void Initialize(UnitAnimationEventInvoker unitAnimationEventInvoker)
    {
        _unitAnimationEventInvoker = unitAnimationEventInvoker;
        _unitAnimationEventInvoker.AnimationChanged += OnAnimationChanged;
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
        }
    }
}