using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Drawing;

public class PlayerClass
{
    //public static PlayerClass Playerinstance { get; set; }
    public int health;
    public int lives = 3;

    public string name;
    public string clonename;

    public List<string> inventory = null;
    public GameObject attackHighlight;
    public GameObject moveHighlight;
    public List<GameObject> highlights;

    public int[] attributes;
    public int[] skill1info;
    public List<Point> skill1;

    public PlayerClass(string n, string cn, int hp, int[] atr, int[] sk1inf, List<Point> sk1)
    {
        this.name = n;
        this.clonename = cn;
        this.health = hp;
        this.attributes = atr;
        this.skill1info = sk1inf;
        this.skill1 = sk1;
    }

    public void setHealth(int hp)
    {
        health = hp;
    }
    public int getHealth()
    {
        return health;
    }

    public void loselife()
    {
        lives -= 1;
    }

    public bool hasItem(string item)
    {
        return inventory.Contains(item);
    }

    public void addItem(string item)
    {
        inventory.Add(item);
    }

    public void removeItem(string item)
    {
        inventory.Remove(item);
    }

    // Places highlights for each skill
    public List<GameObject> useSkill(int key, Vector3 playerloc, int x, int y)
    {
        switch (key)
        {
            case 1:
                foreach (Point tile in Knight.basicAttack)
                {
                    if (BattleManager.Instance.gridCell[Mathf.Abs(x + tile.X), Mathf.Abs(y + tile.Y)] != null)
                        if (BattleManager.Instance.gridCell[Mathf.Abs(x + tile.X), Mathf.Abs(y + tile.Y)].pass)
                            highlights.Add(GameObject.Instantiate(attackHighlight,
                                  BattleManager.Instance.gridCell[Mathf.Abs(x + tile.X), Mathf.Abs(y + tile.Y)].center, Quaternion.identity));
                }
                return highlights;
            case 2:
                //Debug.Log(transform.position.ToString("F2"));
                foreach (Point tile in skill1)
                {
                    if (BattleManager.Instance.gridCell[Mathf.Abs(x + tile.X), Mathf.Abs(y + tile.Y)] != null)
                        if (BattleManager.Instance.gridCell[Mathf.Abs(x + tile.X), Mathf.Abs(y + tile.Y)].pass)
                            highlights.Add(GameObject.Instantiate(moveHighlight,
                                  BattleManager.Instance.gridCell[Mathf.Abs(x + tile.X), Mathf.Abs(y + tile.Y)].center, Quaternion.identity));
                }
                return highlights;
        }
        return null;
    }

    // Returns basic skill info, {dmg, type, ismove}
    public int[] getInfo(int key)
    {
        switch (key)
        {
            case 1:
                return new int[] { 5, 1, 0 };
            case 2:
                return skill1info;
        }
        return null;
    }
}
