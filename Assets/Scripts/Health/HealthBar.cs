using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private GameObject _fillArea;
    [SerializeField] private bool _playerHealthBar;
    [SerializeField] private Health _health;
    [SerializeField] private Slider _slider;

    private void Awake()
    {
        if (_playerHealthBar)
            _health = GameObject.FindWithTag("Player").GetComponent<Health>();
        else
            _health.ZeroHealth += DisableHealthBar;
        _health.ValueChanged += UpdateBar;
    }

    private void OnDisable()
    {
        _health.ValueChanged -= UpdateBar;
        if (!_playerHealthBar)
            _health.ZeroHealth -= DisableHealthBar;
    }

    private void UpdateBar(float percentage)
    {
        if (percentage == 0)
            _fillArea.transform.localScale = Vector3.zero;
        _slider.value = percentage;
    }

    private void DisableHealthBar() => gameObject.SetActive(false);
}
