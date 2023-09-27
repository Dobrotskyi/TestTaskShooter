using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraViewToggle : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _3rdPersonView;
    [SerializeField] private CinemachineVirtualCamera _1stPersonView;
    private bool _lockCamera = false;
    private Camera _mainCamera;
    private float _transitionDuration = 0;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _transitionDuration = _mainCamera.GetComponent<Cinemachine.CinemachineBrain>().m_DefaultBlend.m_Time;
        EndGameMenu.GameHasEnded += OnGameOver;
    }

    private void OnDestroy()
    {
        EndGameMenu.GameHasEnded -= OnGameOver;
    }

    public void EnableFirstPersonView()
    {
        if (_lockCamera) return;
        if (_1stPersonView.gameObject.activeSelf)
            return;
        StopAllCoroutines();

        if (_3rdPersonView.gameObject.activeSelf)
            _3rdPersonView.gameObject.SetActive(false);

        if (!_1stPersonView.gameObject.activeSelf)
            _1stPersonView.gameObject.SetActive(true);

        StartCoroutine(TurnOffPlayerVisibility());
    }

    public void EnableThirdPersonView()
    {
        if (_lockCamera) return;
        if (_3rdPersonView.gameObject.activeSelf)
            return;
        StopAllCoroutines();

        if (_1stPersonView.gameObject.activeSelf)
            _1stPersonView.gameObject.SetActive(false);
        if (!_3rdPersonView.gameObject.activeSelf)
            _3rdPersonView.gameObject.SetActive(true);

        StartCoroutine(TurnOnPlayerVisibility());
    }

    private void OnGameOver()
    {
        EnableThirdPersonView();
        _lockCamera = true;
    }

    private IEnumerator TurnOffPlayerVisibility()
    {
        yield return new WaitForSeconds(_transitionDuration * 0.5f);
        _mainCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("Player"));
    }

    private IEnumerator TurnOnPlayerVisibility()
    {
        yield return new WaitForSeconds(_transitionDuration * 0.5f);
        _mainCamera.cullingMask |= 1 << LayerMask.NameToLayer("Player");
    }
}
