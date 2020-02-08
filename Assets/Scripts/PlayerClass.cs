using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass : MonoBehaviour
{
    private PlayerClass playerinstance;
    public int health = 15;
    private int baseAttack;
    public int lives = 3;
    public List<string> inventory = null;
    public GameObject attackHighlight;
    public GameObject moveHighlight;
    public List<GameObject> highlights;

    private void awake()
    {
        if (playerinstance == null)
        {
            playerinstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void start()
    {
        //attackHighlight = GameObject.Find("TileHighlight1");
        //moveHighlight = GameObject.Find("TileHighlight2");
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
        }
        return null;
    }
}
