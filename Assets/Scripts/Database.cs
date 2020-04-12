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
           new Item(Item.ItemType.Provisions, "Provisions",true, -5, "HP"),
           new Item(Item.ItemType.Resurrection,"Resurrections",true),
           new Item(Item.ItemType.Gold,"Gold",true),
           new Item(Item.ItemType.BeardedKey, "Bearded Key", false),
           new Item(Item.ItemType.WolfMeat,"Wolf Meat",false),
           new Item(Item.ItemType.WolfCollar,"Wolf Collar",false),
           new Item(Item.ItemType.Acorn,"Acorn",false),
           new Item(Item.ItemType.CherryPit,"Cherry Pit",false),
           new Item(Item.ItemType.HalfChewedChocolate,"Half-Chewed Chocolate",false),
           new Item(Item.ItemType.TowerShield,"Tower Shield",false,3,"MAXHP"),
           new Item(Item.ItemType.RustyHelmet,"Rusty Helmet",false,-2,"MAXHP"),
           new Item(Item.ItemType.StolenGoods,"Stolen Goods",false)
        };
    }

    public Item getItem(Item.ItemType item)
    {
        foreach (Item tempItem in items)
        {
            if (tempItem.item == item)
                return tempItem;
        }

        return null;
    }

    public Weapon getWeapon(Weapon.WeaponType weapon)
    {
        foreach (Weapon tempWeapon in weapons)
        {
            if (tempWeapon.weapon == weapon)
                return tempWeapon;
        }

        return null;
    }
}
