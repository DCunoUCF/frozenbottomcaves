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
           new Item(Item.ItemType.Sword,false),
           new Item(Item.ItemType.Shield,false),
           new Item(Item.ItemType.Provisions,true),
           new Item(Item.ItemType.Resurrection,true),
           new Item(Item.ItemType.Gold,true),
           new Item(Item.ItemType.BeardedKey,false),
           new Item(Item.ItemType.WolfMeat,false),
           new Item(Item.ItemType.WolfCollar,false),
           new Item(Item.ItemType.Acorn,false),
           new Item(Item.ItemType.CherryPit,false),
           new Item(Item.ItemType.HalfChewedChocolate,false),
           new Item(Item.ItemType.TowerShield,false)
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
