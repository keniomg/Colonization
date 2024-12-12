using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private string _holdingBoolName;
    [SerializeField] private string _walkBoolName;

    private Animator _animator;
    private UnitAnimationStatus _unitAnimationStatus;

    public void Initialize(UnitAnimationStatus unitAnimationStatus, Animator animator)
    {
        _unitAnimationStatus = unitAnimationStatus;
        _animator = animator;
    }

    private void Update()
    {
        HandleAnimations();
    }

    private void HandleAnimations()
    {
        if (_unitAnimationStatus != null) 
        {
            _animator.SetBool(_holdingBoolName, _unitAnimationStatus.IsHolding);
            _animator.SetBool(_walkBoolName, _unitAnimationStatus.IsWalking);
        }
    }
}