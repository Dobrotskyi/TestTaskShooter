using System;
using UnityEngine;

public abstract class Property : MonoBehaviour
{
    public event Action<float> ValueChanged;
    [SerializeField] protected int _max;
    protected int _current;

    public abstract int Current { get; protected set; }
    public float CurrentInPercents => (float)_current / _max;

    protected void RaiseValueChanged() => ValueChanged?.Invoke(CurrentInPercents);

    private void Awake()
    {
        _current = _max;
    }
}