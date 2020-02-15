using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass : MonoBehaviour
{
    public static PlayerClass Playerinstance { get; set; }
    public int health = 15;
    private int baseAttack;
    public int lives = 3;
    public int x, y;
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

    // NEEDS BOUNDS CHECKING FOR MAP
    // Places highlights for each skill
    public List<GameObject> useSkill(int key, Vector3 playerloc)
    {
        Debug.Log("placing highlights");
        switch (key)
        {
            case 1:
                highlights.Add((GameObject)Instantiate(attackHighlight,
                              new Vector3(playerloc.x + .5f, playerloc.y + .25f), Quaternion.identity));
                highlights.Add((GameObject)Instantiate(attackHighlight,
                              new Vector3(playerloc.x + .5f, playerloc.y + -.25f), Quaternion.identity));
                highlights.Add((GameObject)Instantiate(attackHighlight,
                              new Vector3(playerloc.x + -.5f, playerloc.y + .25f), Quaternion.identity));
                highlights.Add((GameObject)Instantiate(attackHighlight,
                              new Vector3(playerloc.x + -.5f, playerloc.y + -.25f), Quaternion.identity));

                highlights.Add((GameObject)Instantiate(attackHighlight,
                              new Vector3(playerloc.x + .0f, playerloc.y + .5f), Quaternion.identity));
                highlights.Add((GameObject)Instantiate(attackHighlight,
                              new Vector3(playerloc.x + .0f, playerloc.y + -.5f), Quaternion.identity));
                highlights.Add((GameObject)Instantiate(attackHighlight,
                              new Vector3(playerloc.x + 1.0f, playerloc.y + .0f), Quaternion.identity));
                highlights.Add((GameObject)Instantiate(attackHighlight,
                              new Vector3(playerloc.x + -1.0f, playerloc.y + .0f), Quaternion.identity));
                return highlights;
            case 2:
                highlights.Add((GameObject)Instantiate(moveHighlight,
                              new Vector3(playerloc.x + .5f, playerloc.y + .25f), Quaternion.identity));
                highlights.Add((GameObject)Instantiate(moveHighlight,
                              new Vector3(playerloc.x + .5f, playerloc.y + -.25f), Quaternion.identity));
                highlights.Add((GameObject)Instantiate(moveHighlight,
                              new Vector3(playerloc.x + -.5f, playerloc.y + .25f), Quaternion.identity));
                highlights.Add((GameObject)Instantiate(moveHighlight,
                              new Vector3(playerloc.x + -.5f, playerloc.y + -.25f), Quaternion.identity));
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
