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
    // For cloning
    public string txtName;

    // Stats
    public int health;
    public int maxHealth;
    public int[] stats; // STR, INT, AGI

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
    public GameObject[] skill1Highlights = new GameObject[2];
    public int cd1;

    public string skill2name;
    public string skill2desc;
    public int[] skill2info;
    public List<Point> skill2;
    public GameObject skill2Highlight;
    public GameObject[] skill2Highlights = new GameObject[2];
    public int cd2;

    public string skill3name;
    public string skill3desc;
    public int[] skill3info;
    public List<Point> skill3;
    public GameObject skill3Highlight;
    public GameObject[] skill3Highlights = new GameObject[2];
    public int cd3;

    public string skill4name;
    public string skill4desc;
    public int[] skill4info;
    public List<Point> skill4;
    public GameObject skill4Highlight;
    public GameObject[] skill4Highlights = new GameObject[4];
    public int cd4;

    public int[] cooldowns = new int[4];

    // STR = 0, INT = 1, AGI = 2
    public int getStat(string i)
    {
        return stats[(int)Enum.Parse(typeof(stats), i)];
    }

    // Ability score modifier, same as D&D 5e rules
    public int getStatModifier(string i)
    {
        int temp = stats[(int)Enum.Parse(typeof(stats), i)];
        if (temp < 6)
            return -3;
        if (temp < 8)
            return -2;
        if (temp < 10)
            return -1;
        if (temp < 12)
            return 0;
        if (temp < 14)
            return 1;
        if (temp < 16)
            return 2;
        if (temp > 16)
            return 3;
        return 0;
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

    public void setHealthEvent(int hp)
    {
        this.health += hp;
        if (health > maxHealth)
            this.health = this.maxHealth;
    }

    public int getHealth()
    {
        return health;
    }

    // ONLY USED IN BUILDING CLASS DON'T USE ANYWHERE ELSE PLS
    public void setHealth(int hp)
    {
        this.health = hp;
        this.maxHealth = hp;
    }

    public void changeMaxHealth(int hp)
    {
        this.maxHealth += hp;
        if (hp > 0)
            setHealthEvent(hp);
        if (this.health > this.maxHealth)
            this.health = this.maxHealth;
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

        skill1Highlights[0] = Resources.Load<GameObject>("Prefabs/TileHighlight2");
        skill1Highlights[1] = Resources.Load<GameObject>("Prefabs/TileHighlight2");

        skill2Highlights[0] = Resources.Load<GameObject>("Prefabs/TileHighlight1");
        skill2Highlights[1] = Resources.Load<GameObject>("Prefabs/TileHighlight1");

        skill3Highlights[0] = Resources.Load<GameObject>("Prefabs/TileHighlight3");
        skill3Highlights[1] = Resources.Load<GameObject>("Prefabs/TileHighlight3_5");

        skill4Highlights[0] = Resources.Load<GameObject>("Prefabs/TileHighlight4");
        skill4Highlights[1] = Resources.Load<GameObject>("Prefabs/TileHighlight4_2");
        skill4Highlights[2] = Resources.Load<GameObject>("Prefabs/TileHighlight4_3");
        skill4Highlights[3] = Resources.Load<GameObject>("Prefabs/TileHighlight4_4");

        //skill4Highlights[0] = Resources.Load<GameObject>("Prefabs/TileHighlight5");
        //skill4Highlights[1] = Resources.Load<GameObject>("Prefabs/TileHighlight5");

    }
    
    public List<Point> getPoints(int key)
    {
        switch(key)
        {
            case 1:
                return skill1;
            case 2:
                return skill2;
            case 3:
                return skill3;
            case 4:
                return skill4;
            default:
                Debug.Log("Get info failed");
                return null;
        }
    }

    public GameObject[] getHighlight(int key)
    {
        switch(key)
        {
            case 1:
                return skill1Highlights;
            case 2:
                return skill2Highlights;
            case 3:
                return skill3Highlights;
            case 4:
                return skill4Highlights;
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

    public int getCD(int key)
    {
        switch (key)
        {
            case 1:
                return cd1;
            case 2:
                return cd2;
            case 3:
                return cd3;
            case 4:
                return cd4;
        }
        return 0;
    }

    public void setCD(int n, int cd) { cooldowns[n] = cd; }

    public void enterCombat()
    {
        for (int i = 0; i < 4; i++)
            cooldowns[i] = 0;
    }

    // decrement any cd > 0
    public void updatePlayerCd()
    {
        for (int i = 0; i < 4; i++)
        {
            if (cooldowns[i] > 0)
                cooldowns[i] -= 1;
        }
    }
}
