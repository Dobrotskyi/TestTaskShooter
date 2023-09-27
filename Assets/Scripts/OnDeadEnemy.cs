using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class OnDeadEnemy : MonoBehaviour
{
    private const int DEATH_ANIM_AMT = 3;

    [SerializeField] private Rig _aimRig;
    private Health _health;
    private Animator _animator;
    private EnemyBehavoiur _behavoiur;
    private NavMeshAgent _agent;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _animator = GetComponent<Animator>();
        _behavoiur = GetComponent<EnemyBehavoiur>();
        _agent = GetComponent<NavMeshAgent>();
        _health.ZeroHealth += OnEnemyDead;
    }

    private void OnDestroy()
    {
        _health.ZeroHealth -= OnEnemyDead;
    }

    private void OnEnemyDead()
    {
        GetComponent<Collider>().enabled = false;
        _agent.isStopped = true;
        System.Random random = new();
        _aimRig.weight = 0;
        _animator.SetInteger("Death", random.Next(0, DEATH_ANIM_AMT));
    }
}
