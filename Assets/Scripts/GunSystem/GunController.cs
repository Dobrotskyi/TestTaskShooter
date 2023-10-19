using StarterAssets;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public event Action<int, GunInfo> GunWasReloaded;
    public event Action<List<GunInfo>> SendGunInfos;

    private const int MAX_WEAPONS_AMT = 3;

    [SerializeField] private Gun[] _guns = new Gun[3];

    private Dictionary<Gun, int> _additionalAmmo = new();
    private StarterAssetsInputs _input;
    private int _selectedWeaponIndex = 0;

    public Gun SelectedGun => _guns[_selectedWeaponIndex];

    private void Awake()
    {
        for (int i = 0; i < _guns.Length; i++)
        {
            if (_guns[i].Available)
                _additionalAmmo[_guns[i]] = -1;
            else
                _additionalAmmo[_guns[i]] = 0;
        }

        _input = GetComponent<StarterAssetsInputs>();
        DisableAllWeapons();

        List<GunInfo> gunInfos = new();
        for (int i = 0; i < _guns.Length; i++)
            gunInfos.Add(new(_guns[i].Name, _guns[i].AmmoInMag, _additionalAmmo[_guns[i]]));
        SendGunInfos?.Invoke(gunInfos);
    }

    private void Update()
    {
        CheckReloading();
        CheckIfChangeWeapon();
        ShowSelectedWeaponIfAiming();
    }

    private void FixedUpdate()
    {
        List<GunInfo> gunInfos = new();
        for (int i = 0; i < _guns.Length; i++)
            gunInfos.Add(new(_guns[i].Name, _guns[i].AmmoInMag, _additionalAmmo[_guns[i]]));
        SendGunInfos?.Invoke(gunInfos);
    }

    private void CheckReloading()
    {
        if (_input.startReloading)
        {
            int amt = _additionalAmmo[SelectedGun];
            if (amt != 0)
            {
                SelectedGun.StartReloading(ref amt);
                _additionalAmmo[SelectedGun] = amt;
                GunWasReloaded?.Invoke(_selectedWeaponIndex, new(SelectedGun.Name,
                                                                 SelectedGun.AmmoInMag,
                                                                 amt));
            }
            _input.startReloading = false;
        }
    }

    private void CheckIfChangeWeapon()
    {
        if (_additionalAmmo[SelectedGun] != -1 && (SelectedGun.AmmoInMag + _additionalAmmo[SelectedGun] == 0))
        {
            SelectedGun.SetAvaliable(false);
            _selectedWeaponIndex = 0;
        }

        if (_input.selectedWeaponIndex != _selectedWeaponIndex)
        {
            if (!_guns[_input.selectedWeaponIndex].Available)
                _input.selectedWeaponIndex = _selectedWeaponIndex;
            else
                _selectedWeaponIndex = _input.selectedWeaponIndex;
        }

        if (!SelectedGun.gameObject.activeSelf)
        {
            DisableAllWeapons();
            SelectedGun.gameObject.SetActive(true);
        }
    }

    private void ShowSelectedWeaponIfAiming()
    {
        if (_input.aim)
            if (_guns[_selectedWeaponIndex].gameObject.activeSelf == false)
                _guns[_selectedWeaponIndex].gameObject.SetActive(true);
        if (!_input.aim)
            if (_guns[_selectedWeaponIndex].gameObject.activeSelf)
                _guns[_selectedWeaponIndex].gameObject.SetActive(false);
    }

    private void DisableAllWeapons()
    {
        foreach (var weapon in _guns) weapon.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CollectableAmmo>(out CollectableAmmo collectableAmmo))
        {
            int gunIndex = (int)collectableAmmo.Type;
            _additionalAmmo[_guns[gunIndex]] += collectableAmmo.DroppedAmmoAmt;
            _guns[gunIndex].SetAvaliable(true);
            Destroy(collectableAmmo.gameObject);
        }
    }
}
