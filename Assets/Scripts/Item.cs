using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public int count;
    public bool stackable;
    public ItemType item;

    public enum ItemType
    {
        Sword,
        Shield,
        Provisions,
        Gold,
        Resurrection,
        BeardedKey,
        WolfMeat,
        RustyHelmet,
        StolenGoods,
        Acorn,
        WolfCollar,
        CherryPit,
        HalfChewedChocolate,
        TowerShield
    }

    public Item(ItemType item, bool stackable)
    {
        this.item = item;
        this.stackable = stackable;
        this.count = 0;
    }

    public Item(ItemType item, bool stackable, int count)
    {
        this.item = item;
        this.stackable = stackable;
        this.count = count;
    }
}
