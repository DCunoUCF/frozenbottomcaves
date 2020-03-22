using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour
{
    List<Item> items;
   
    public void BuildDatabase()
    {
        items = new List<Item>()
        {
           new Item(Item.ItemType.Sword,false),
           new Item(Item.ItemType.Sheild,false),
           new Item(Item.ItemType.Provisions,true),
           new Item(Item.ItemType.Sword,true),
           new Item(Item.ItemType.Ressurection,true),
           new Item(Item.ItemType.Gold,true)
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
}
