using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class OnDeadPlayer : MonoBehaviour
{
    [SerializeField] private Health _health;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _health = GetComponent<Health>();
        _health.ZeroHealth += OnDead;
    }

    private void OnDestroy()
    {
        _health.ZeroHealth -= OnDead;
    }

    private void OnDead()
    {
        _animator.SetBool("Dead", true);
        PlayerInput input = GetComponent<PlayerInput>();
        input.actions = null;
        input.enabled = false;
        GetComponent<ThirdPersonController>().enabled = false;
        GetComponent<Collider>().enabled = false;
    }
}
