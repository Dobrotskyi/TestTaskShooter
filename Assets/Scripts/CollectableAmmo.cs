using UnityEngine;

public class CollectableAmmo : MonoBehaviour
{
    private const int MAX_DROPPED_AMMO = 30;

    public enum AmmoType
    {
        SecondWeapon = 1,
        MainWeapon = 2
    }
    public AmmoType Type => _type;
    public int DroppedAmmoAmt { private set; get; }

    [SerializeField] private AmmoType _type;

    private void OnEnable()
    {
        DroppedAmmoAmt = Random.Range(5, MAX_DROPPED_AMMO + 1);
    }
}
