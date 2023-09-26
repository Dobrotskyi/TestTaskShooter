using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(EnemyPerseption))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBehavoiur : MonoBehaviour
{
    [SerializeField] private Vector3 _anchorPoint;
    [SerializeField] private float _walkPointRange;
    [SerializeField] private LayerMask _ground;
    [SerializeField] private Rig _aimRig;
    [SerializeField] private Gun _gun;

    private EnemyPerseption _perseption;
    private Animator _animator;
    private NavMeshAgent _agent;
    private Vector3 _walkPoint;
    private bool _walkingPointSet;

    private void Awake()
    {
        _perseption = GetComponent<EnemyPerseption>();
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        if (_perseption.EnemyState == EnemyPerseption.State.Patroling)
            Patroling();
        else if (_perseption.EnemyState == EnemyPerseption.State.Chasing)
            ChaseTarget();
        else if (_perseption.EnemyState == EnemyPerseption.State.Attacking)
            Attack();
    }

    private void Patroling()
    {
        _animator.SetBool("Walk", true);
        DisableAimingRig();
        if (!_walkingPointSet) GetWalkingPoint();

        _agent.SetDestination(_walkPoint);
        if (_agent.remainingDistance < 1f)
            _walkingPointSet = false;
    }

    private void ApplyAimingRig()
    {
        if (!_perseption.TargetIsBehind)
            if (_aimRig.weight != 1f)
                _aimRig.weight = Mathf.Lerp(_aimRig.weight, 1f, 20f * Time.deltaTime);
    }

    private void DisableAimingRig()
    {
        if (_aimRig.weight != 0)
            _aimRig.weight = Mathf.Lerp(_aimRig.weight, 0f, 20f * Time.deltaTime);
    }

    private void GetWalkingPoint()
    {
        float randomZ = Random.Range(-_walkPointRange + _anchorPoint.z, _walkPointRange + _anchorPoint.z);
        float randomX = Random.Range(-_walkPointRange + _anchorPoint.x, _walkPointRange + _anchorPoint.x);

        _walkPoint = new Vector3(randomX, transform.position.y, randomZ);
        if (Physics.Raycast(_walkPoint, -transform.up, 2f, _ground))
            _walkingPointSet = true;
    }

    private void ChaseTarget()
    {
        _animator.SetBool("Walk", true);
        ApplyAimingRig();
        _agent.SetDestination(_perseption.Target.position);
    }

    private void Attack()
    {
        if (_perseption.TargetIsBehind)
            HandleEnemyRotation();
        _animator.SetBool("Walk", false);

        if (!_perseption.TargetIsBehind)
            ApplyAimingRig();
        else
            DisableAimingRig();

        _agent.SetDestination(transform.position);
        _gun.Shoot();
        if (_gun.AmmoInMag <= 0)
            _gun.StartReloading();
    }

    private void HandleEnemyRotation()
    {
        Vector3 chestAimDirection = _perseption.Target.position - transform.position;
        chestAimDirection.y = 0f;
        transform.forward = Vector3.Lerp(transform.forward,
                                        chestAimDirection.normalized,
                                        20f * Time.deltaTime);
    }
}