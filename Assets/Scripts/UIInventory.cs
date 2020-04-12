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
    public GameObject AttackPanel;
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
            temp.GetComponent<TextMeshProUGUI>().text = item.displayName + " x" + item.count;
        }

        // Resurrection
        else if(item.item == Item.ItemType.Resurrection)
        {
            instance = MainMenu;
            instance.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().color = color1;
            instance.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(true);
            temp = instance.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
            temp.SetActive(true);
            temp.GetComponent<TextMeshProUGUI>().text = item.displayName + " x" + item.count;
        }

        // Miscellaneous Items
        else
        {
            instance = Instantiate(Resources.Load("Prefabs/Text") as GameObject);
            instance.GetComponent<UIItem>().item = item;
            instance.GetComponent<TextMeshProUGUI>().text = item.displayName + " x" + item.count;
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

        // Attack Variables
        GameObject normalAttack1;
        GameObject specialAttack1;
        GameObject specialAttack2;

        // Bio Updates
        GameObject BioContent = BioPanel.transform.GetChild(0).gameObject;
        GameObject AttackContent = AttackPanel.transform.GetChild(0).gameObject;

        Weapon01 = MainMenu.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
        Weapon01.GetComponent<TextMeshProUGUI>().text = player.weapon01.displayName;

        Weapon02 = MainMenu.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject;
        Weapon02.GetComponent<TextMeshProUGUI>().text = player.weapon02.displayName;

        HP = BioContent.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;
        HP.GetComponent<TextMeshProUGUI>().text = "" + player.health + "/" + player.maxHealth;


        STR = BioContent.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;
        STR.GetComponent<TextMeshProUGUI>().text = "" + player.getStat("STR") + " (" + player.getStatModifier("STR") + ")";

        INT = BioContent.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;
        INT.GetComponent<TextMeshProUGUI>().text = "" + player.getStat("INT") + " (" + player.getStatModifier("INT") + ")";

        AGI = BioContent.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;
        AGI.GetComponent<TextMeshProUGUI>().text = "" + player.getStat("AGI") + " (" + player.getStatModifier("AGI") + ")";

        Quest = QuestMenu.transform.GetChild(1).gameObject;
        Quest.GetComponent<TextMeshProUGUI>().text = player.quest;

        Bio = BioContent.transform.GetChild(4).gameObject;
        Bio.GetComponent<TextMeshProUGUI>().text = player.bio;

        // Here is the Attack Content
        normalAttack1 = AttackContent.transform.GetChild(0).gameObject;
        specialAttack1 = AttackContent.transform.GetChild(1).gameObject;
        specialAttack2 = AttackContent.transform.GetChild(2).gameObject;

        // Attack Updates
        string[] a1 = PlayerManager.Instance.getSkillInfo(2);
        normalAttack1.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = a1[0];
        normalAttack1.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = a1[1] + "\nCooldown: " + PlayerManager.Instance.pc.cd2;

        string[] a2 = PlayerManager.Instance.getSkillInfo(3);
        specialAttack1.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = a2[0];
        specialAttack1.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = a2[1] + "\nCooldown: " + (PlayerManager.Instance.pc.cd3-1) + " turns";

        string[] a3 = PlayerManager.Instance.getSkillInfo(4);
        specialAttack2.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = a3[0];
        specialAttack2.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = a3[1] + "\nCooldown: " + (PlayerManager.Instance.pc.cd4-1) + " turns";
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
            temp.GetComponent<TextMeshProUGUI>().text = item.displayName + " x" + item.count;

            /*if(item.count == 0) // Want to keep provisions up even if at 0
            {
                MainMenu.transform.GetChild(3).gameObject.SetActive(false);
            }*/
        }

        // Resurrection
        else if (item.item == Item.ItemType.Resurrection)
        {
            temp = MainMenu.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
            temp.GetComponent<TextMeshProUGUI>().text = item.displayName + " x" + item.count;

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
                uItem.GetComponent<TextMeshProUGUI>().text = uItem.item.displayName + " x" + uItem.item.count;
            }
        }
    }
}
