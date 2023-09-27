using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class OnDeathOrGameWin : MonoBehaviour
{
    [SerializeField] private Health _health;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _health = GetComponent<Health>();
        _health.ZeroHealth += OnDead;
        EndGameMenu.PlayerWon += OnWin;
    }

    private void OnDestroy()
    {
        _health.ZeroHealth -= OnDead;
        EndGameMenu.PlayerWon -= OnWin;
    }

    private void OnDead()
    {
        _animator.SetBool("Dead", true);
        DisableMovement();
    }

    private void OnWin()
    {
        _animator.SetBool("PlayerWon", true);
        DisableMovement();
    }

    private void DisableMovement()
    {
        PlayerInput input = GetComponent<PlayerInput>();
        GetComponent<StarterAssetsInputs>().aim = false;
        input.actions = null;
        input.enabled = false;
        GetComponent<ThirdPersonController>().enabled = false;
        GetComponent<Collider>().enabled = false;
        GetComponent<PlayerAiming>().enabled = false;
    }
}
