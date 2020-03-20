using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager2 : MonoBehaviour
{
    public Player2 player;
    public Inventory inventory;
    public UIInventory inventoryUI;
    public GameObject InventoryPanel;
    public GameObject BioPanel;
 

    void Start()
    {
        player = new Player2("Knight");
        player.inventory = inventory;
        player.inventory.updateStats(player);
        player.inventory.addItem(Item.ItemType.Sword, 5);
        player.inventory.addItem(Item.ItemType.Ressurection, 3);
        player.inventory.addItem(Item.ItemType.Provisions, 5);
        player.inventory.addItem(Item.ItemType.Gold, 100);
 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryUI.gameObject.activeSelf)
            {
                inventoryUI.gameObject.SetActive(false);
            }
            else
            {
                inventory.updateStats(player);
                InventoryPanel.SetActive(true);
                BioPanel.SetActive(false);
                inventoryUI.gameObject.SetActive(true);
            }
        }
    }



}
