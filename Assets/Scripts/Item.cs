using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public int count;
    public string displayName;
    public bool stackable;
    public ItemType item;

    public enum ItemType
    {
        Sword,
        Sheild,
        Provisions,
        Gold,
        Ressurection
    }

    public Item(ItemType item, string displayName, bool stackable)
    {
        this.item = item;
        this.displayName = displayName;
        this.stackable = stackable;
        this.count = 0;
    }
}
