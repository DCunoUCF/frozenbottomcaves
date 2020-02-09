using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    public PlayerClass playerScript;
    public string characterName;
    public string characterTag;
    public GameObject player;
    public Vector3 playerLoc, selectedTile;
    public bool inCombat, isTurn, selectingSkill;
    public List<GameObject> highlights;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        characterTag = "Knight";
        characterName = "TheWhiteKnight1(Clone)";
        player = GameObject.Find(characterName);
        playerLoc = player.transform.position;
        playerScript = (PlayerClass) player.GetComponent(typeof(PlayerClass));

        inCombat = true;
        isTurn = true;
        selectingSkill = true;
        abilityinfo = new int[3];
    }

    void Update()
    {
        if (!inCombat)
        {
            // do overworld stuff
        }
        else
        {
            // Player can select what ability/move to use
            playerTurnCombat();
        }
    }

    // Player turn -> select ability -> select tile -> turn end
    private void playerTurnCombat()
    {
        if (isTurn && selectingSkill)
        {
            // Read input and set variables based off of what skill
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                this.selectingSkill = false;
                this.highlights = playerScript.useSkill(1, playerLoc);

                this.abilityinfo = playerScript.getInfo(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                this.selectingSkill = false;
                this.highlights = playerScript.useSkill(2, playerLoc);

                this.abilityinfo = playerScript.getInfo(2);
            }
            // Add more cases for more abilities
        }

        // Still player turn, they have pressed a skill and are confirming location by selecting a tile
        else if (isTurn && !selectingSkill)
        {
            // Cancel selected skill -> remove highlights and let them choose another skill
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                foreach (GameObject highlight in highlights)
                    Destroy(highlight);
                this.selectingSkill = true;
            }
        }
    }

    // After selecting a tile, the players turn is ended
    public void setSelectedTile(Vector3 pos)
    {
        if (isTurn)
        {
            this.selectedTile = pos;
            Debug.Log(selectedTile.ToString("F2"));
            isTurn = false;
            foreach (GameObject highlight in highlights)
                Destroy(highlight);
            Debug.Log("Attack dmg: " + this.abilityinfo[0] + " Attack type: " + this.abilityinfo[1] + " isMove: " + (this.abilityinfo[2] == 1));
        }
    }

    public void setTurn(bool set)
    {
        isTurn = set;
    }

    public Vector3 getSelectedTile()
    {
        return this.selectedTile;
    }

    public int getDamage()
    {
        return this.abilityinfo[0];
    }

    public int getType()
    {
        return this.abilityinfo[1];
    }

    public bool getMove()
    {
        return this.abilityinfo[2] == 1;
    }
}
