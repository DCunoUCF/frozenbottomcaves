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
    public int[] skill1info;
    public List<Point> skill1;
    public int[] skill2info;
    public List<Point> skill2;

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
        attackHighlight = Resources.Load<GameObject>("Prefabs/TileHighlight1");
        moveHighlight = Resources.Load<GameObject>("Prefabs/TileHighlight2");
    }

    public GameObject getHighlight(int key)
    {
        switch(key)
        {
            case 1:
                return attackHighlight;
            case 2:
                return moveHighlight;
            default:
                return moveHighlight;
        }
    }

    // Returns basic skill info, {dmg, type, ismove}
    public int[] getInfo(int key)
    {
        switch (key)
        {
            case 1:
                return new int[] { 5, 1, 0 };
            case 2:
                return skill2info;
        }
        return null;
    }
}
