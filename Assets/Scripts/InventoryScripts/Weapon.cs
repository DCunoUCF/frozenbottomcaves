using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    public WeaponType weapon;

    public enum WeaponType
    {
        KnightSword,
        KnightShield,
        MageStaff,
        MageVeil,
        MonkFist,
        MonkPendant
    };

    public Weapon(WeaponType weapon)
    {
        this.weapon = weapon;
    }
}
