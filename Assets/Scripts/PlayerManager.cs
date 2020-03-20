﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; set; }
    [SerializeField]
    public PlayerClass pc;
    public string characterName;
    public string characterNameClone;
    public string characterNameClone2 = "TheWhiteKnight(Clone)";
    public string characterTag = "Player";
    public GameObject player;
    public Vector3 playerLoc, selectedTile;
    public bool inCombat, isTurn, selectingSkill;
    public List<GameObject> highlights;
    public int x, y;
    public int movx = 0, movy = 0;
    public bool moved;
    private GameManager gm;
    public bool characterSelected;

    public bool combatInitialized, hold;
    public CList combatInfo;

    GameObject HM;
    HighlightManager HMScript;

    // int array with {type, dmg, move}
    public int[] abilityinfo;

    // Keep only one instance alive
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        this.gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        highlights = new List<GameObject>();
        inCombat = false;
        combatInitialized = false;
        isTurn = false;
        selectingSkill = true;
        moved = false;
        abilityinfo = new int[3];
        characterSelected = false;
    }

    void Update()
    {
        if (!inCombat && characterSelected)
        {
            player = GameObject.Find(characterNameClone);
        }
        else if(inCombat)// We fightin now bois
        {
            if (moved)
            {
                this.x += movx;
                this.y += movy;
                movx = 0;
                movy = 0;
                moved = false;
            }

            // Player can select what ability/move to use
            playerTurnCombat();
        }
    }

    public void initCombat()
    {
        this.player = BattleManager.Instance.player;
        playerLoc = player.transform.position;
        print(playerLoc);
        combatInfo = new CList(this.player);
        combatInitialized = true;
        inCombat = true;
        pc.setHighlights();
        HM = GameObject.Find("Highlights");
        HMScript = (HighlightManager)HM.GetComponent("HighlightManager");
    }

    public void initPM()
    {
        characterName = pc.name;
        characterNameClone = pc.clonename;
        characterSelected = true;
    }

    // Player turn -> select ability -> select tile -> turn end
    private void playerTurnCombat()
    {
        if (isTurn)
        {
            this.combatInfo = BattleManager.Instance.combatantList[0];
            // Read input and set combat info based off of what skill
            if (Input.GetButtonDown("Skill1"))
            {
                clearHighlights();
                placeHighlights(pc.skill1, 1);
                this.abilityinfo = pc.getInfo(1);
                fillCombatInfo(abilityinfo);
            }
            else if (Input.GetButtonDown("Skill2"))
            {
                clearHighlights();
                placeHighlights(pc.skill2, 2);
                this.abilityinfo = pc.getInfo(2);
                fillCombatInfo(abilityinfo);
            }
            else if (Input.GetButtonDown("Skill3"))
            {

            }
            else if (Input.GetButtonDown("Skill4"))
            {

            }
            else if (Input.GetButtonDown("Cancel"))
            {
                clearHighlights();
            }
            // Add more cases for more abilities
        }
    }

    public void fillCombatInfo(int[] info)
    {
        this.combatInfo.attackDmg = abilityinfo[0];
        this.combatInfo.attack = abilityinfo[1];
        this.combatInfo.move = abilityinfo[2] == 1;
        BattleManager.Instance.combatantList[0] = this.combatInfo;
    }


    // After selecting a tile, the players turn is ended
    public void setSelectedTile(Vector3 pos)
    {
        if (isTurn)
        {
            this.selectedTile = pos;
            this.combatInfo.movTar = pos;
            this.combatInfo.atkTar = pos;
            //Debug.Log(selectedTile.ToString("F2"));
            isTurn = false;
            clearHighlights();
            getMoveXY(pos);
            this.selectingSkill = true;
            BattleManager.Instance.combatantList[0] = this.combatInfo;
            //Debug.Log("combatInfo.move: " + combatInfo.move);
        }
    }

    // Gets the x, y for moving on the grid based of given mov target
    public void getMoveXY(Vector3 movTarget)
    {
        Vector3 temp = movTarget - playerLoc;
        int x, y;
        x = (int)(temp.x / .5f);
        y = (int)(temp.y / .25f);
        if (x == 0)
        {
            movx = 0;
            movy = y;
        }
        if (y == 0)
        {
            movx = x;
            movy = 0;
        }
        if (x > 0)
        {
            if (y > 0)
            {
                movx = x;
                movy = 0;
            }
            if (y < 0)
            {
                movx = 0;
                movy = y;
            }
        }
        else if (x < 0)
        {
            if (y > 0)
            {
                movx = 0;
                movy = y;
            }
            if (y < 0)
            {
                movx = x;
                movy = 0;
            }
        }
    }

    IEnumerator HODL()
    {
        yield return new WaitForSeconds(.3f);
        hold = false;
    }

    // Clears current highlights
    public void clearHighlights()
    {
        HMScript.clearTiles();
        StartCoroutine(HODL());
        if (!hold)
        {
            foreach (GameObject highlight in highlights)
                Destroy(highlight);
            highlights.Clear();
        }
        HM.transform.DetachChildren();
        hold = false;
    }
    
    public void placeHighlights(List<Point> points, int key)
    {
        clearHighlights();
        StartCoroutine(HODL());
        if (!hold)
        {
            GameObject highlight = pc.getHighlight(key);

            foreach (Point tile in points)
            {
                int newX = x + tile.X;
                int newY = y + tile.Y;
                if (newX < 0 || newX > BattleManager.Instance.gridCell.GetLength(1))
                    continue;

                if (newY < 0 || newY > BattleManager.Instance.gridCell.GetLength(0))
                    continue;

                if (BattleManager.Instance.gridCell[newX, newY] != null)
                    if (BattleManager.Instance.gridCell[newX, newY].pass)
                        highlights.Add(Instantiate(highlight,
                              BattleManager.Instance.gridCell[newX, newY].center, Quaternion.identity));
            }
            foreach (GameObject hl in highlights)
            {
                hl.transform.SetParent(HM.transform);
            }
            HMScript.setTiles(highlights);
        }
        hold = true;
    }

    // Returns the requested stat, 1 - str, 2 - int, 3 - dex
    public int getStat(int i)
    {
        return this.pc.attributes[i];
    }

}
