using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class Enemy : MonoBehaviour
{
    [SerializeField] private LayerMask _ground, _playerLayer;
    [SerializeField] private float _sightRange, _attackRange;
    [SerializeField] private float _walkPointRange;
    [SerializeField] private Vector3 _anchorPoint;
    [SerializeField] private Gun _gun;
    [SerializeField] private Rig _aimRig;

    private NavMeshAgent _agent;
    private Transform _player;
    private bool _playerIsInSight, _playerInAttackRange, _walkingPointSet;
    private Vector3 _walkPoint = Vector3.zero;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        _playerIsInSight = Physics.CheckSphere(transform.position, _sightRange, _playerLayer);
        _playerInAttackRange = Physics.CheckSphere(transform.position, _attackRange, _playerLayer);

        if (!_playerIsInSight && !_playerInAttackRange) Patroling();
        else if (_playerIsInSight && !_playerInAttackRange) ChasePlayer();
        else if (_playerIsInSight && _playerInAttackRange) Attack();
    }

    private void Patroling()
    {
        DisableAimingRig();
        if (!_walkingPointSet) GetWalkingPoint();

        _agent.SetDestination(_walkPoint);
        if (_agent.remainingDistance < 1f)
            _walkingPointSet = false;
    }

    private void GetWalkingPoint()
    {
        float randomZ = Random.Range(-_walkPointRange + _anchorPoint.z, _walkPointRange + _anchorPoint.z);
        float randomX = Random.Range(-_walkPointRange + _anchorPoint.x, _walkPointRange + _anchorPoint.x);

        _walkPoint = new Vector3(randomX, transform.position.y, randomZ);
        if (Physics.Raycast(_walkPoint, -transform.up, 2f, _ground))
            _walkingPointSet = true;
    }

    private void Attack()
    {
        ApplyAimingRig();

        _agent.SetDestination(transform.position);

        Vector3 chestAimDirection = _player.transform.position - transform.position;
        chestAimDirection.y = 0f;
        transform.forward = Vector3.Lerp(transform.forward,
                                        chestAimDirection.normalized,
                                        20f * Time.deltaTime);

        _gun.Shoot();
        if (_gun.AmmoInMag <= 0)
            _gun.StartReloading();
    }

    private void ChasePlayer()
    {
        ApplyAimingRig();
        _agent.SetDestination(_player.position);
    }

    private void ApplyAimingRig()
    {
        if (_aimRig.weight != 1f)
            _aimRig.weight = Mathf.Lerp(_aimRig.weight, 1f, 20f * Time.deltaTime);
    }

    private void DisableAimingRig()
    {
        if (_aimRig.weight != 0)
            _aimRig.weight = Mathf.Lerp(_aimRig.weight, 0f, 20f * Time.deltaTime);
    }
}
