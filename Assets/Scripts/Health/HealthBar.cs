using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private GameObject _fillArea;
    private Slider _slider;
    private Health _health;

    private void UpdateBar(float percentage)
    {
        if (percentage == 0)
            _fillArea.transform.localScale = Vector3.zero;
        _slider.value = percentage;
    }

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _health = GameObject.FindWithTag("Player").GetComponent<Health>();
        _health.ValueChanged += UpdateBar;
    }

    private void OnDisable()
    {
        _health.ValueChanged -= UpdateBar;
    }
}
