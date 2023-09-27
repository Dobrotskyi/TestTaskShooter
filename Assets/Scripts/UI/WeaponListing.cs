using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponListing : MonoBehaviour
{
    [Serializable]
    public struct ListingField
    {
        public TextMeshProUGUI NameField;
        public TextMeshProUGUI AmmoInfoField;

        public void SetNameFieldText(string name)
        {
            NameField.text = name;
        }

        public void SetAmmoInfoField(int ammoInMag, int ammoLeft)
        {
            string ammoLeftStr = ammoLeft < 0 ? "∞" : ammoLeft.ToString();
            AmmoInfoField.text = $"{ammoInMag}/{ammoLeftStr}";
        }
    }

    [SerializeField] private ListingField[] fields = new ListingField[3];
    private GunController _gunController;

    private void Awake()
    {
        _gunController = FindObjectOfType<GunController>();
        _gunController.SendGunInfos += UpdateListing;
    }

    private void OnDestroy()
    {
        _gunController.SendGunInfos -= UpdateListing;
    }

    private void UpdateListing(List<GunInfo> gunInfos)
    {
        for (int i = 0; i < fields.Length; i++)
        {
            fields[i].SetNameFieldText(gunInfos[i].Name);
            fields[i].SetAmmoInfoField(gunInfos[i].AmmoInMag, gunInfos[i].AmmoLeft);
        }
    }
}
