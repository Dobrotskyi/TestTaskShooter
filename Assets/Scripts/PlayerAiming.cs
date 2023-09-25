using StarterAssets;
using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private Transform _debugTransform;
#endif

    [SerializeField] private CameraViewToggle _viewToggle;
    [SerializeField] private LayerMask _aimColliderMask;

    [SerializeField] private ParticleSystem _enemyHitMarker;
    [SerializeField] private ParticleSystem _missedHitMarker;

    private GunController _gunController;
    private StarterAssetsInputs _inputs;
    private Animator _animator;

    private void Awake()
    {
        _inputs = GetComponent<StarterAssetsInputs>();
        _animator = GetComponent<Animator>();
        _gunController = GetComponent<GunController>();
    }

    private void Update()
    {
        Vector3 mouseAimWorldPos = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit rayHit, 999f, _aimColliderMask))
        {
#if UNITY_EDITOR
            _debugTransform.position = rayHit.point;
#endif
            mouseAimWorldPos = rayHit.point;
        }

        if (_inputs.aim)
        {
            _viewToggle.EnableFirstPersonView();

            Vector3 worldAimTarget = mouseAimWorldPos;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, 20f * Time.deltaTime);

            _animator.SetLayerWeight(1, Mathf.Lerp(_animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));

            if (_inputs.shoot)
                _gunController.SelectedGun.Shoot();
        }
        else
        {
            _viewToggle.EnableThirdPersonView();
            _animator.SetLayerWeight(1, Mathf.Lerp(_animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
        }
    }
}
