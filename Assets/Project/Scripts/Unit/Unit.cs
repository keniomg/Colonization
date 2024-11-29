using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(UnitMover), typeof(CapsuleCollider), typeof(UnitAnimationStatus))]
[RequireComponent(typeof(UnitAnimator), typeof(Animator), typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody), typeof(UnitResourcesHolder), typeof(UnitColonizer))]
[RequireComponent(typeof(UnitCommandController))]
public class Unit : MonoBehaviour
{
    private Animator _animator;
    private UnitAnimator _unitAnimator;
    private UnitAnimationStatus _animationStatus;
    private NavMeshAgent _navMeshAgent;

    public UnitAnimationEventInvoker AnimationEventInvoker { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public UnitMover Mover { get; private set; }
    public UnitCommandController UnitCommandController { get; private set; }
    public UnitResourcesHolder ResourcesHolder { get; private set; }
    public UnitColonizer Colonizer { get; private set; }

    private void Awake()
    {
        GetComponents();
        InitializeComponents();
    }

    private void GetComponents()
    {
        AnimationEventInvoker = ScriptableObject.CreateInstance<UnitAnimationEventInvoker>();

        Mover = GetComponent<UnitMover>();
        _animator = GetComponent<Animator>();
        _unitAnimator = GetComponent<UnitAnimator>();
        _animationStatus = GetComponent<UnitAnimationStatus>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        Colonizer = GetComponent<UnitColonizer>();
        ResourcesHolder = GetComponent<UnitResourcesHolder>();
        UnitCommandController = GetComponent<UnitCommandController>();
    }

    private void InitializeComponents()
    {
        _animationStatus.Initialize(AnimationEventInvoker);
        _unitAnimator.Initialize(_animationStatus, _animator);
        Mover.Initialize(_navMeshAgent);
    }
}