using UnityEngine;

public class GunAmmoStorage : MonoBehaviour
{
    public int AmmoLeft { private set; get; }
    [SerializeField] private bool _playersWeapon;

    public void AddBullets(int amt)
    {
        if (_playersWeapon)
            AmmoLeft += amt;
    }

    public void ReloadAmmo(int amt)
    {
        if (_playersWeapon)
            AmmoLeft -= amt;
    }
}
