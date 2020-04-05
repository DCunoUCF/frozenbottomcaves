using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database
{
    List<Item> items;
    List<Weapon> weapons;

    public Database()
    {
        BuildDatabase();
    }

    public void BuildDatabase()
    {
        items = new List<Item>()
        {
           new Item(Item.ItemType.Sword,"Sword",false),
           new Item(Item.ItemType.Shield,"Shield",false),
           new Item(Item.ItemType.Provisions, "Provisions",true),
           new Item(Item.ItemType.Ressurection,"Ressurections",true),
           new Item(Item.ItemType.Gold,"Gold",true)

        };
    }

    public Item getItem(Item.ItemType item)
    {
        foreach(Item tempItem in items)
        {
            if (tempItem.item == item)
                return tempItem;
        }

        return null;
    }

    public Weapon getWeapon(Weapon.WeaponType weapon)
    {
        foreach(Weapon tempWeapon in weapons)
        {
            if (tempWeapon.weapon == weapon)
                return tempWeapon;
        }

        return null;
    }
}
