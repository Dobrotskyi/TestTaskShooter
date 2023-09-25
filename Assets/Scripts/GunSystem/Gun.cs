using UnityEngine;

public class Gun : MonoBehaviour
{
    public bool Available { private set; get; }
    public string Name { private set; get; }

    [SerializeField] private GunInfoSO _gunInfo;
    [SerializeField] private Transform _shotPoint;

    private float _lastShotTime = 0;

    public void Shoot()
    {
        if (_lastShotTime + _gunInfo.FireRate > Time.time)
            return;

        _lastShotTime = Time.time;
        Vector3 shootingSpread = new Vector3(UnityEngine.Random.Range(-_gunInfo.Spread.x, _gunInfo.Spread.x),
                                             UnityEngine.Random.Range(-_gunInfo.Spread.y, _gunInfo.Spread.y),
                                             UnityEngine.Random.Range(-_gunInfo.Spread.z, _gunInfo.Spread.z));
        Ray ray = new Ray(_shotPoint.position, (_shotPoint.transform.forward + shootingSpread).normalized);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, _gunInfo.HitLayerMask))
        {
            Quaternion hitEffectRotation = Quaternion.LookRotation(_shotPoint.position - raycastHit.point);
            if (raycastHit.transform.CompareTag("Enemy"))
            {
                Instantiate(_gunInfo.EnemyHit, raycastHit.point, hitEffectRotation);
            }
            else
                Instantiate(_gunInfo.Missed, raycastHit.point, hitEffectRotation);
        }
    }

    private void Awake()
    {
        Available = _gunInfo.AvailableFromStart;
        Name = _gunInfo.Name;
    }
}
