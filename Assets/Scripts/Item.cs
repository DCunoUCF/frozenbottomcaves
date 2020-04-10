using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public int count;
    public string displayName;
    public bool stackable;
    public int effect;
    public string stat;

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
    public Item(ItemType item, string displayName, bool stackable, int effect, string stat)
    {
        this.item = item;
        this.displayName = displayName;
        this.stackable = stackable;
        this.effect = effect;
        this.stat = stat;
        this.count = 0;
    }

    public Item(ItemType item, string displayName, bool stackable, bool effect)
    {
        this.item = item;
        this.displayName = displayName;
        this.stackable = stackable;
        this.effect = 0;
        this.stat = "";
        this.count = 0;
    }

    public Item(ItemType item, bool stackable, int count)
    {
        this.item = item;
        this.stackable = stackable;
        this.count = count;
    }
}
