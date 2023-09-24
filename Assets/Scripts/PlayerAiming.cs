using StarterAssets;
using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    [SerializeField] private CameraViewToggle _viewToggle;
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
    }
}
