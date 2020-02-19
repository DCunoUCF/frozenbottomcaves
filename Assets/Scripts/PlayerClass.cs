using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Drawing;

public class PlayerClass : MonoBehaviour
{
    public static PlayerClass Playerinstance { get; set; }
    public int health = 15;
    private int baseAttack;
    public int lives = 3;
    
    public List<string> inventory = null;
    public GameObject attackHighlight;
    public GameObject moveHighlight;
    public List<GameObject> highlights;
    public int[] info;


    // Keep only one instance alive through scenes
    private void awake()
    {
        if (Playerinstance == null)
        {
            Playerinstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void start()
    {

    }

    public void update()
    {

    }


    public void setHealth(int hp)
    {
        health = hp;
    }
    public int getHealth()
    {
        return health;
    }

    public void changeAttack(int modifier)
    {
        baseAttack = modifier;
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
                            highlights.Add((GameObject)Instantiate(attackHighlight,
                                  BattleManager.Instance.gridCell[Mathf.Abs(x + tile.X), Mathf.Abs(y + tile.Y)].center, Quaternion.identity));
                }
                return highlights;
            case 2:
                //Debug.Log(transform.position.ToString("F2"));
                foreach (Point tile in Knight.basicMove)
                {
                    if (BattleManager.Instance.gridCell[Mathf.Abs(x + tile.X), Mathf.Abs(y + tile.Y)] != null)
                        if (BattleManager.Instance.gridCell[Mathf.Abs(x + tile.X), Mathf.Abs(y + tile.Y)].pass)
                            highlights.Add((GameObject)Instantiate(moveHighlight,
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
                return new int[] { 2, 1, 0 };
            case 2:
                return new int[] { 0, 0, 1 };
        }
        return null;
    }
}
