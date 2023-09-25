using UnityEngine;

[CreateAssetMenu(fileName = "GunInfo", menuName = "Guns/Gun info", order = 2)]
public class GunInfoSO : ScriptableObject
{
    [SerializeField] private bool _availableFromStart;

    [SerializeField] private string _name;
    [SerializeField] private int _magCapacity;
    [SerializeField] private float _reloadTime;
    [SerializeField] private float _fireRate;
    [SerializeField] private int _damage;

    [SerializeField] private Vector3 _spread;
    [SerializeField] private LayerMask _hitLayerMask;

    [SerializeField] private ParticleSystem _enemyHit;
    [SerializeField] private ParticleSystem _missed;

    public bool AvailableFromStart => _availableFromStart;
    public string Name => _name;
    public int MagCapacity => _magCapacity;
    public float ReloadTime => _reloadTime;
    public float FireRate => _fireRate;
    public int Damage => _damage;
    public Vector3 Spread => _spread;
    public LayerMask HitLayerMask => _hitLayerMask;

    public ParticleSystem EnemyHit => _enemyHit;
    public ParticleSystem Missed => _missed;
}
