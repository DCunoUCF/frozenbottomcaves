﻿using System.Collections;
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
        Sheild,
        Provisions,
        Gold,
        Ressurection
    }

    public Item(ItemType item, bool stackable)
    {
        this.item = item;
        this.stackable = stackable;
        this.count = 0;
    }
}
