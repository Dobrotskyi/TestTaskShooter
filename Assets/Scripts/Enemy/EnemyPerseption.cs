using UnityEngine;

public class EnemyPerseption : MonoBehaviour
{
    private const float _rotationThreashold = 60f;

    public enum State
    {
        Patroling,
        Chasing,
        Attacking
    }

    public State EnemyState { private set; get; } = State.Patroling;
    public bool TargetIsBehind { private set; get; } = true;

    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _sightRange, _attackRange;

    private bool _rotateTowardsEnemy;
    private bool _playerIsInSight, _playerInAttackRange, _walkingPointSet;
    public Transform Target { private set; get; }

    private void Awake()
    {
        Target = GameObject.FindWithTag("Player").transform.Find("EnemyAimPoint");
    }

    private void Update()
    {
        _playerIsInSight = Physics.CheckSphere(transform.position, _sightRange, _playerLayer);
        _playerInAttackRange = Physics.CheckSphere(transform.position, _attackRange, _playerLayer);

        if (!_playerIsInSight && !_playerInAttackRange) EnemyState = State.Patroling;
        else if (_playerIsInSight && !_playerInAttackRange) EnemyState = State.Chasing;
        else if (_playerIsInSight && _playerInAttackRange) EnemyState = State.Attacking;

        if (EnemyState == State.Attacking || EnemyState == State.Chasing)
            CheckIfPlayerIsBehind();
    }

    private void CheckIfPlayerIsBehind()
    {
        Vector3 chestAimDirection = Target.transform.position - transform.position;
        chestAimDirection.y = 0f;
        float angleBetweenChestAndTarget = Vector3.Angle(chestAimDirection, transform.forward);

        if (angleBetweenChestAndTarget > _rotationThreashold)
            TargetIsBehind = true;

        if (TargetIsBehind)
            if (angleBetweenChestAndTarget > 5f)
                return;

        TargetIsBehind = false;
    }
}
