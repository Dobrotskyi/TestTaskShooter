using StarterAssets;
using UnityEngine;

public class GunController : MonoBehaviour
{
    private const int MAX_WEAPONS_AMT = 3;

    [SerializeField] private Gun[] _guns = new Gun[3];
    private StarterAssetsInputs _input;
    private int _selectedWeaponIndex = 0;

    public Gun SelectedGun => _guns[_selectedWeaponIndex];

    private void Awake()
    {
        _input = GetComponent<StarterAssetsInputs>();
        DisableAllWeapons();
    }

    private void Update()
    {
        if (_input.selectedWeaponIndex != _selectedWeaponIndex)
        {
            if (!_guns[_input.selectedWeaponIndex].Available)
                _input.selectedWeaponIndex = _selectedWeaponIndex;
            else
            {
                _selectedWeaponIndex = _input.selectedWeaponIndex;
                DisableAllWeapons();
                _guns[_selectedWeaponIndex].gameObject.SetActive(true);
            }
        }

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
}
