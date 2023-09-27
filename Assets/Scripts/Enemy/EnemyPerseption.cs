using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnemyPerseption : MonoBehaviour
{
    private const float ROTATION_THREASHOLD = 60f;

    public enum State
    {
        Patroling,
        Chasing,
        Attacking
    }

    public State EnemyState { private set; get; } = State.Patroling;
    public bool TargetIsBehind { private set; get; } = true;
    public Transform Target { private set; get; }

    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _sightRange, _attackRange;
    [SerializeField] private Transform _eyeLevel;

    private bool _rotateTowardsEnemy;
    private bool _playerIsInSight, _playerInAttackRange, _walkingPointSet;

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
        else if (_playerIsInSight && _playerInAttackRange && !PlayerBehindObstacle()) EnemyState = State.Attacking;

        if (EnemyState == State.Attacking || EnemyState == State.Chasing)
            CheckIfPlayerIsBehind();

        if (EnemyState == State.Attacking)
        {
            if (PlayerBehindObstacle())
                EnemyState = State.Chasing;
        }
    }

    private void CheckIfPlayerIsBehind()
    {
        Vector3 chestAimDirection = Target.transform.position - transform.position;
        chestAimDirection.y = 0f;
        float angleBetweenChestAndTarget = Vector3.Angle(chestAimDirection, transform.forward);

        if (angleBetweenChestAndTarget > ROTATION_THREASHOLD)
            TargetIsBehind = true;

        if (TargetIsBehind)
            if (angleBetweenChestAndTarget > 5f)
                return;

        TargetIsBehind = false;
    }

    private bool PlayerBehindObstacle()
    {
        Ray ray = new(_eyeLevel.position, (Target.position - _eyeLevel.position).normalized);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
        {
            if (!raycastHit.transform.CompareTag("Player"))
                return true;
        }
        return false;
    }
}
