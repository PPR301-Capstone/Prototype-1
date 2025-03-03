using UnityEngine;

public class Weapon : Item
{
    public enum WeaponType
    {
        SWORD_1H,
        SWORD_2H,
        AXE_1H,
        AXE_2H,
        POLEARM,
        RANGED
    }

    public WeaponType weaponType;
    public float WeaponSpeed;
    public float WeaponDamage;
}
