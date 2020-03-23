using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Drawing;
using System;

enum stats
{
    STR = 0, INT, AGI
}

public class PlayerClass
{
    // Stats
    public int health;
    public int maxHealth;
    private int[] stats; // STR, INT, AGI

    // Inventory
    public Inventory inventory;

    // Information about prefab, flavor text
    public string name;
    public string clonename;
    public string bio;
    public string quest;
    // Hero Weapons
    public Weapon weapon01;
    public Weapon weapon02;

    // Combat information
    public GameObject attackHighlight;
    public GameObject moveHighlight;
    public List<GameObject> highlights;

    public string skill1name;
    public string skill1desc;
    public int[] skill1info;
    public List<Point> skill1;
    public GameObject skill1Highlight;

    public string skill2name;
    public string skill2desc;
    public int[] skill2info;
    public List<Point> skill2;
    public GameObject skill2Highlight;

    public string skill3name;
    public string skill3desc;
    public int[] skill3info;
    public List<Point> skill3;
    public GameObject skill3Highlight;

    public string skill4name;
    public string skill4desc;
    public int[] skill4info;
    public List<Point> skill4;
    public GameObject skill4Highlight;

    // STR = 0, INT = 1, AGI = 2
    public int getStat(string i)
    {
        return stats[(int)Enum.Parse(typeof(stats), i)];
    }

    // Whenever the player takes damage
    public void takeDamage(int dmg)
    {
        /* 
         * 
         * EXTRA DMG LOGIC HERE
         * 
         * 
        */

        health -= dmg;

    }

    public int getHealth()
    {
        return health;
    }

    public void setHealth(int hp)
    {
        this.health = hp;
        this.maxHealth = hp;
    }

    public void setStats(int[] stats)
    {
        this.stats = stats;
    }

    // Need to create prefabs for each unique ability and load them here
    public void setHighlights()
    {
        skill1Highlight = Resources.Load<GameObject>("Prefabs/TileHighlight2");
        skill2Highlight = Resources.Load<GameObject>("Prefabs/TileHighlight1");
        skill3Highlight = Resources.Load<GameObject>("Prefabs/TileHighlight1");
        skill4Highlight = Resources.Load<GameObject>("Prefabs/TileHighlight2");

    }

    public GameObject getHighlight(int key)
    {
        switch(key)
        {
            case 1:
                return skill1Highlight;
            case 2:
                return skill2Highlight;
            case 3:
                return skill3Highlight;
            case 4:
                return skill4Highlight;
            default:
                Debug.Log("Get Highlight Failure");
                return null;
        }
    }

    // Returns basic skill info, {dmg, type, ismove}
    public int[] getInfo(int key)
    {
        switch (key)
        {
            case 1:
                return skill1info;
            case 2:
                return skill2info;
            case 3:
                return skill3info;
            case 4:
                return skill4info;
        }
        return null;
    }
}
