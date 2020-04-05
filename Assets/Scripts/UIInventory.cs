using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInventory : MonoBehaviour
{
    public List<UIItem> UIitems = new List<UIItem>();
    public GameObject textPrefab;
    public Transform textPanel;
    public GameObject MainMenu;
    public GameObject GoldMenu;
    public GameObject MiscellaneousMenu;
    public GameObject QuestMenu;
    public GameObject BioPanel;
    public RectTransform content;
    public Scrollbar scrollbar;

    // Adds an Item into Inventory
    public void addItem(Item item)
    {
        GameObject instance;
        GameObject temp;
        Color color1;
        ColorUtility.TryParseHtmlString("#323232", out color1);
        
        // Gold
        if (item.item == Item.ItemType.Gold)
        {
            instance = GoldMenu;
            temp = instance.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;
            temp.SetActive(true);
            temp.GetComponent<TextMeshProUGUI>().text = "" + item.count;
        }

        // Provisions
        else if(item.item == Item.ItemType.Provisions)
        {
            instance = MainMenu;
            instance.transform.GetChild(3).gameObject.SetActive(true);
            temp = instance.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject;
            temp.GetComponent<TextMeshProUGUI>().text = item.item + " x" + item.count;
        }

        // Ressurection
        else if(item.item == Item.ItemType.Resurrection)
        {
            instance = MainMenu;
            instance.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = color1;
            instance.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(true);
            temp = instance.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
            temp.SetActive(true);
            temp.GetComponent<TextMeshProUGUI>().text = item.item + " x" + item.count;
        }

        // Miscellaneous Items
        else
        {
            instance = Instantiate(Resources.Load("Prefabs/Text") as GameObject);
            instance.GetComponent<UIItem>().item = item;
            instance.GetComponent<TextMeshProUGUI>().text = item.item + " x" + item.count;
            instance.transform.SetParent(textPanel);
            UIitems.Add(instance.GetComponent<UIItem>());
            content.sizeDelta = new Vector2(content.sizeDelta.x, content.sizeDelta.y + 30f);
            instance.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    // Removes an Item from Inventory
    public void removeItem(Item item)
    {
        // Main Menu Items
        if (item.item == Item.ItemType.Gold || item.item == Item.ItemType.Provisions || item.item == Item.ItemType.Resurrection)
            return;

        // Only Miscellaneous Items
        foreach(UIItem UIitem in UIitems)
        {
            if (UIitem.item == item)
            {
                // count = 1 and not stackable
                if (UIitem.item.count == 1 && !(UIitem.item.stackable))
                {
                    UIitems.Remove(UIitem);
                    UIitem.destroyItem();
                    content.sizeDelta = new Vector2(content.sizeDelta.x, content.sizeDelta.y - 30f);      
                }

                // count = 0 and stackable
                else if(UIitem.item.count == 0)
                {
                    UIitems.Remove(UIitem);
                    UIitem.destroyItem();
                    
                }

                // In case of any errors
                else
                    updateUIInventory(UIitem.item);

                return;
            }
        }
    }

    // Updates the UI stats for player
    public void updateUIStats(PlayerClass player)
    {
        GameObject Weapon01;
        GameObject Weapon02;

        GameObject HP;
        GameObject STR;
        GameObject INT;
        GameObject AGI;
        GameObject Quest;
        GameObject Bio;

        GameObject Content = BioPanel.transform.GetChild(0).gameObject;

        Weapon01 = MainMenu.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
        Weapon01.GetComponent<TextMeshProUGUI>().text = player.weapon01.weapon.ToString();

        Weapon02 = MainMenu.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject;
        Weapon02.GetComponent<TextMeshProUGUI>().text = player.weapon02.weapon.ToString();

        HP = Content.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;
        HP.GetComponent<TextMeshProUGUI>().text = "" + player.health + "/" + player.maxHealth;


        STR = Content.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;
        STR.GetComponent<TextMeshProUGUI>().text = "" + player.getStat("STR");

        INT = Content.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;
        INT.GetComponent<TextMeshProUGUI>().text = "" + player.getStat("INT");

        AGI = Content.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;
        AGI.GetComponent<TextMeshProUGUI>().text = "" + player.getStat("AGI");

        Quest = QuestMenu.transform.GetChild(1).gameObject;
        Quest.GetComponent<TextMeshProUGUI>().text = player.quest;

        Bio = Content.transform.GetChild(4).gameObject;
        Bio.GetComponent<TextMeshProUGUI>().text = player.bio;
    }


    // Updates UI text
    public void updateUIInventory(Item item)
    {
        GameObject temp;
        Color color;
        ColorUtility.TryParseHtmlString("#575252", out color);

        // Gold
        if (item.item == Item.ItemType.Gold)
        {
            temp = GoldMenu.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;
            temp.GetComponent<TextMeshProUGUI>().text = "" + item.count;
        }
        
        // Provisions
        else if (item.item == Item.ItemType.Provisions)
        {
      
            temp = MainMenu.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject;
            temp.GetComponent<TextMeshProUGUI>().text = item.item + " x" + item.count;

            if(item.count == 0)
            {
                MainMenu.transform.GetChild(3).gameObject.SetActive(false);
            }
        }

        // Ressurection
        else if (item.item == Item.ItemType.Resurrection)
        {
            temp = MainMenu.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
            temp.GetComponent<TextMeshProUGUI>().text = item.item + " x" + item.count;

            if(item.count == 0)
            {
                temp.GetComponent<TextMeshProUGUI>().color = color;
                MainMenu.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(false);
            }
        }

        // Miscellaneous Items
        else
        {
            foreach (UIItem uItem in UIitems)
            {
                uItem.GetComponent<TextMeshProUGUI>().text = uItem.item.item + " x" + uItem.item.count;
            }
        }
    }
}
