using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    public WeaponType weapon;
    public string displayName;

    public enum WeaponType
    {
        KnightSword,
        KnightShield,
        MageStaff,
        MageVeil,
        MonkFist,
        MonkPendant
    };

    public Weapon(WeaponType weapon, string displayName)
    {
        this.displayName = displayName;
        this.weapon = weapon;
    }
}
