using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody), typeof(UnitCommandController), typeof(UnitResourcesHolder))]
[RequireComponent(typeof(UnitMover), typeof(CapsuleCollider), typeof(UnitAnimationStatus))]
[RequireComponent(typeof(UnitAnimator), typeof(Animator), typeof(NavMeshAgent))]

public class Unit : MonoBehaviour
{
    private Animator _animator;
    private UnitAnimator _unitAnimator;
    private UnitAnimationStatus _animationStatus;
    private NavMeshAgent _navMeshAgent;

    public UnitAnimationEventInvoker AnimationEventInvoker { get; private set; }
    public Rigidbody Rigidbody {get; private set; }
    public UnitCommandController CommandController { get; private set; }
    public UnitResourcesHolder ResourcesHolder { get; private set; }
    public UnitMover Mover { get; private set; }

    private void Awake()
    {
        GetComponents();
        InitializeComponents();
    }

    private void GetComponents()
    {
        AnimationEventInvoker = ScriptableObject.CreateInstance<UnitAnimationEventInvoker>();

        CommandController = TryGetComponent(out UnitCommandController commandController) ? commandController : null;
        ResourcesHolder = TryGetComponent(out UnitResourcesHolder unitResourcesHolder) ? unitResourcesHolder : null;
        Mover = TryGetComponent(out UnitMover mover) ? mover : null;
        _animator = TryGetComponent(out Animator animator) ? animator : null;
        _unitAnimator = TryGetComponent(out UnitAnimator unitAnimator) ? unitAnimator : null;
        _animationStatus = TryGetComponent(out UnitAnimationStatus unitAnimationStatus) ? unitAnimationStatus : null;
        _navMeshAgent = TryGetComponent(out NavMeshAgent navMeshAgent) ? navMeshAgent : null;
    }

    private void InitializeComponents()
    {
        _animationStatus.Initialize(AnimationEventInvoker);
        _unitAnimator.Initialize(_animationStatus, _animator);
        Mover.Initialize(_navMeshAgent);
    }
}