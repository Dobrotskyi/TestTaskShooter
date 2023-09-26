using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public bool Available { private set; get; }
    public bool IsReloading { private set; get; }

    public string Name { private set; get; }
    public int AmmoInMag { private set; get; }

    [SerializeField] private GunInfoSO _gunInfo;
    [SerializeField] private Transform _shotPoint;
    [SerializeField] private ProjectileTrail _trailPrefab;

    private float _lastShotTime = 0;

    public void StartReloading()
    {
        if (!IsReloading)
            StartCoroutine(Reloading());
    }

    private IEnumerator Reloading()
    {
        IsReloading = true;
        Vector3 rotationEulers = transform.localRotation.eulerAngles;
        transform.localRotation = Quaternion.Euler(rotationEulers.x - 90f, rotationEulers.y, rotationEulers.z);
        yield return new WaitForSeconds(_gunInfo.ReloadTime);
        IsReloading = false;
        AmmoInMag = _gunInfo.MagCapacity;
        transform.localRotation = Quaternion.Euler(rotationEulers);
    }

    public void AimAt(Vector3 position)
    {
        if (IsReloading)
            return;
        transform.forward = (position - transform.position).normalized;
    }

    public void Shoot()
    {
        if (_lastShotTime + _gunInfo.FireRate > Time.time || AmmoInMag <= 0 || IsReloading)
            return;

        _lastShotTime = Time.time;
        Vector3 shootingSpread = new Vector3(UnityEngine.Random.Range(-_gunInfo.Spread.x, _gunInfo.Spread.x),
                                             UnityEngine.Random.Range(-_gunInfo.Spread.y, _gunInfo.Spread.y),
                                             UnityEngine.Random.Range(-_gunInfo.Spread.z, _gunInfo.Spread.z));
        Ray ray = new Ray(_shotPoint.position, (_shotPoint.transform.forward + shootingSpread).normalized);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, _gunInfo.HitLayerMask))
        {
            Quaternion hitEffectRotation = Quaternion.LookRotation(_shotPoint.position - raycastHit.point);
            if (raycastHit.transform.CompareTag("Enemy") || raycastHit.transform.CompareTag("Player"))
            {
                Instantiate(_gunInfo.EnemyHit, raycastHit.point, hitEffectRotation);
                if (raycastHit.transform.TryGetComponent<Health>(out Health health))
                    health.TakeDamage(_gunInfo.Damage);
            }
            else
                Instantiate(_gunInfo.Missed, raycastHit.point, hitEffectRotation);

            Instantiate(_trailPrefab, _shotPoint.position, Quaternion.identity).Setup(raycastHit.point);
        }
        AmmoInMag--;
    }

    private void Awake()
    {
        Available = _gunInfo.AvailableFromStart;
        Name = _gunInfo.Name;
        AmmoInMag = _gunInfo.MagCapacity;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnEnable()
    {
        if (IsReloading)
            StartCoroutine(Reloading());
    }
}
