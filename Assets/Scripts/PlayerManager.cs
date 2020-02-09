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
    public int attackDmg;

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

    private void playerTurnCombat()
    {
        if (isTurn && selectingSkill)
        {
            // Read input
            Debug.Log("awaiting input");
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log("key pressed");
                this.selectingSkill = false;
                this.highlights = playerScript.useSkill(1, playerLoc);
                this.attackDmg = playerScript.getDmg(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                // do some other ability/move ...
            }
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
}
