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

    private StarterAssetsInputs _inputs;

    private void Awake()
    {
        _inputs = GetComponent<StarterAssetsInputs>();
    }

    private void Update()
    {
        Vector3 mouseAimWorldPos = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        Transform hitTransform = null;
        if (Physics.Raycast(ray, out RaycastHit rayHit, 999f, _aimColliderMask))
        {
#if UNITY_EDITOR
            _debugTransform.position = rayHit.point;
#endif
            mouseAimWorldPos = rayHit.point;
            hitTransform = rayHit.transform;
        }

        if (_inputs.aim)
        {
            _viewToggle.EnableFirstPersonView();

            Vector3 worldAimTarget = mouseAimWorldPos;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, 20f * Time.deltaTime);

            if (_inputs.shoot)
            {
                if (hitTransform != null)
                {
                    Quaternion hitEffectRotation = Quaternion.LookRotation(transform.position - rayHit.point);

                    if (hitTransform.CompareTag("Enemy"))
                        Instantiate(_enemyHitMarker, rayHit.point, hitEffectRotation);
                    else
                        Instantiate(_missedHitMarker, rayHit.point, hitEffectRotation);
                }
                _inputs.shoot = false;
            }
        }
        else
            _viewToggle.EnableThirdPersonView();
    }
}
