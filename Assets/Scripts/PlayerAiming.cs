using StarterAssets;
using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private Transform _debugTransform;
#endif

    [SerializeField] private CameraViewToggle _viewToggle;
    [SerializeField] private LayerMask _aimColliderMask;
    private StarterAssetsInputs _inputs;

    private void Awake()
    {
        _inputs = GetComponent<StarterAssetsInputs>();
    }

    private void Update()
    {
        if (_inputs.aim)
            _viewToggle.EnableFirstPersonView();
        else
            _viewToggle.EnableThirdPersonView();


        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit rayHit, 999f, _aimColliderMask))
        {
#if UNITY_EDITOR
            _debugTransform.position = rayHit.point;
#endif
        }
    }
}
