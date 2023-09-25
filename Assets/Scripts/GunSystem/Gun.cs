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

    private float _lastShotTime = 0;

    public void StartReloading()
    {
        if (!IsReloading)
            StartCoroutine(Reloading());
    }

    private IEnumerator Reloading()
    {
        IsReloading = true;
        transform.localRotation = Quaternion.Euler(-90f, 0, 0);
        yield return new WaitForSeconds(_gunInfo.ReloadTime);
        IsReloading = false;
        AmmoInMag = _gunInfo.MagCapacity;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
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
            if (raycastHit.transform.CompareTag("Enemy"))
            {
                Instantiate(_gunInfo.EnemyHit, raycastHit.point, hitEffectRotation);
            }
            else
                Instantiate(_gunInfo.Missed, raycastHit.point, hitEffectRotation);
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