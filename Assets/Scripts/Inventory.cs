using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory
{
    public List<Item> items;
    public Database database;
    public UIInventory inventoryUI;
    public GameObject InventoryPanel;
    public GameObject AttackPanel;
    public GameObject BioPanel;
    public Button[] buttons;
    public PlayerManager pm;

    public Inventory(PlayerManager pm)
    {
        items = new List<Item>();

        inventoryUI = (UIInventory)GameObject.Find("Inventory").GetComponent("UIInventory");
        InventoryPanel = GameObject.Find("InventoryPanel");
        BioPanel = GameObject.Find("BioPanel");
        AttackPanel = GameObject.Find("AttackPanel");
        buttons = new Button[6];

        // Buttons
        buttons[0] = GameObject.Find("ResButton").GetComponent<Button>();
        buttons[1] = GameObject.Find("ProvButton").GetComponent<Button>();
        buttons[2] = GameObject.Find("BackButton").GetComponent<Button>();
        buttons[3] = GameObject.Find("InventoryButton").GetComponent<Button>();
        buttons[4] = GameObject.Find("BioButton").GetComponent<Button>();
        buttons[5] = GameObject.Find("AttackButton").GetComponent<Button>();

        // Listeners
        buttons[0].onClick.AddListener(removeRessurection);
        buttons[1].onClick.AddListener(removeProvision);
        buttons[2].onClick.AddListener(toggleInventory);
        buttons[3].onClick.AddListener(toggleInventoryPanel);
        buttons[4].onClick.AddListener(toggleBioPanel);
        buttons[5].onClick.AddListener(toggleAttackPanel);

        database = new Database();
        this.pm = pm;
    }

    public void setInitSelection()
    {
        buttons[3].Select();
        buttons[3].OnSelect(null);
    }


    // Adds an item to our inventory
    public void addItem(Item.ItemType item, int count)
    {
        // Used to update the UIInventory Efficiently
        // 1 item is not in the list
        // 2 item is in the list and is stackable
        // 3 item is not in the list and is not stackable

        if (count <= 0)
        {
            Debug.Log("Count cannot be less than or equal 0");
            return;
        }

        int flag;

        // Look into database
        Item databaseItem = database.getItem(item);

        // Look into inventory
        Item inventoryItem = CheckItem(item);

        // Item is not the Inventory
        if (inventoryItem == null)
        {
            // Not stackable
            if (!databaseItem.stackable)
            {
                for (int i = 0; i < count; i++)
                {
                    items.Add(databaseItem);
                    flag = 1;
                    databaseItem.count = 1;
                    updateInventory(databaseItem, count, flag);
                }
            }
            // Stackable
            else
            {
                items.Add(databaseItem);
                flag = 1;
                databaseItem.count += count;
                updateInventory(databaseItem, count, flag);
            }
        }
        // Item is in the inventory and stackable
        else if (inventoryItem != null && inventoryItem.stackable)
        {
            inventoryItem.count += count;
            flag = 2;
            updateInventory(inventoryItem, count, flag);
        }
        // Item is in the inventory and not stackable
        else
        {
            for (int i = 0; i < count; i++)
            {
                items.Add(databaseItem);
                flag = 3;
                updateInventory(databaseItem, count, flag);
            }
        }
    }

    // Refreshes UI Inventory
    public void updateInventory(Item item, int count, int flag)
    {
        if(flag == 1)
        {
            inventoryUI.addItem(item);
        }
        else if(flag == 2)
        {
            inventoryUI.updateUIInventory(item);
        }
        else
        {
            inventoryUI.addItem(item);
        }
    }

    // Removes an item from our inventory
    public void removeItem(Item.ItemType item, int count)
    {
        if (count <= 0)
        {
            Debug.Log("Error. Count cannot be less than or equal to 0");
            return;
        }

        // Check if Item exists
        Item inventoryItem = CheckItem(item);

        // If it does exist, proceed
        if (inventoryItem != null)
        {
            // Count greater than 0 and Stackable
            if (inventoryItem.count - count > 0 && inventoryItem.stackable)
            {
                inventoryItem.count -= count;
                inventoryUI.updateUIInventory(inventoryItem);
            }

            // Count <= 0 and Stackable
            else if (inventoryItem.count - count <= 0 && inventoryItem.stackable)
            {
                inventoryItem.count = 0;
                items.Remove(inventoryItem);
                inventoryUI.removeItem(inventoryItem);
                inventoryUI.updateUIInventory(inventoryItem);

            }
            // Item is not stackable
            else if (!(inventoryItem.stackable))
            {
                for (int i = 0; i < count; i++)
                {
                    items.Remove(inventoryItem);
                    inventoryUI.removeItem(inventoryItem);
                    inventoryUI.updateUIInventory(inventoryItem);
                }
            }
            else
            {
                Debug.Log("Error. Option not possible. RemoveItem function");
            }

        }
    }

    // Checks if item exists in our inventory
    public Item CheckItem(Item.ItemType item)
    {
        // Loop through all items in list till you find id
        foreach (Item tempItem in items)
        {
            if (tempItem.item == item)
            {
                return tempItem;
            }
        }
        return null;
    }

    // Testing purposes
    public void printList()
    {
        foreach(Item item in items)
        {
            Debug.Log(item.item + " x" + item.count);
        }
    }

    // Peggi will use this function 
    public void updateStats(PlayerClass player)
    {

        inventoryUI.updateUIStats(player);

    }

    // Button that adds sword
    public void addSword()
    {
        addItem(0, 1);
    }

    // Button that removes sword
    public void removeSword()
    {
        removeItem(0, 1);
    }

    // Button that adds provisions
    public void addProvision()
    {
        addItem(Item.ItemType.Provisions, 1);
    }

    // Button that removes provisions
    public void removeProvision()
    {
        removeItem(Item.ItemType.Provisions, 1);
        this.pm.pc.setHealthEvent(5);
    }

    public void addRessurection()
    {
        addItem(Item.ItemType.Ressurection, 1);
    }

    public void removeRessurection()
    {
        removeItem(Item.ItemType.Ressurection, 1);
    }

    public void toggleInventory()
    {
        pm.invImg.color = Color.white;
        inventoryUI.gameObject.SetActive(false);
        pm.gm.om.dm.setInteractable();
    }

    public void toggleInventoryPanel()
    {
        InventoryPanel.SetActive(true);
        BioPanel.SetActive(false);
        AttackPanel.SetActive(false);

    }

    public void toggleBioPanel()
    {
        InventoryPanel.SetActive(false);
        BioPanel.SetActive(true);
        AttackPanel.SetActive(false);

    }

    public void toggleAttackPanel()
    {
        InventoryPanel.SetActive(false);
        BioPanel.SetActive(false);
        AttackPanel.SetActive(true);
    }
}
