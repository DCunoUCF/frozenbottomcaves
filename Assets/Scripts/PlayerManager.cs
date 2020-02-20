using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; set; }
    [SerializeField]
    public PlayerClass playerScript;
    public string characterName;
    public string characterNameClone;
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

    public bool combatInitialized;
    public CList combatInfo;

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
        //characterName = "TheWhiteKnight";
        //characterNameClone = "TheWhiteKnight(Clone)";
        //player = GameObject.Find(characterName);
        //playerLoc = player.transform.position;
        //playerScript = (PlayerClass)player.GetComponent(typeof(PlayerClass));
        //combatInfo = new CList(player);


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
            print(characterName);
            player = GameObject.Find(characterNameClone);
            print(player.transform.position);
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

            // Combat is flagged, find character and create init Clist entry
            if (!combatInitialized)
            {
                player = GameObject.Find(characterNameClone);
                playerLoc = player.transform.position;
                print(playerLoc);
                combatInfo = new CList(player);
                combatInitialized = true;
            }

            // Player can select what ability/move to use
            playerTurnCombat();
        }
    }

    public void initPM()
    {
        characterName = playerScript.name;
        characterNameClone = playerScript.clonename;
        characterSelected = true;
    }

    // Player turn -> select ability -> select tile -> turn end
    private void playerTurnCombat()
    {
        if (isTurn)
        {
            this.combatInfo = BattleManager.Instance.combatantList[0];
            // Read input and set combat info based off of what skill
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                clearHighlights();
                this.highlights = playerScript.useSkill(1, playerLoc, x, y);
                this.abilityinfo = playerScript.getInfo(1);
                fillCombatInfo(abilityinfo);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                clearHighlights();
                this.highlights = playerScript.useSkill(2, playerLoc, x, y);
                this.abilityinfo = playerScript.getInfo(2);
                fillCombatInfo(abilityinfo);
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
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

    // Clears current highlights
    public void clearHighlights()
    {
        foreach (GameObject highlight in highlights)
            Destroy(highlight);
        highlights.Clear();
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

    public void getMoveXY(Vector3 movTarget)
    {
        Vector3 temp = movTarget - playerLoc;
        int x, y;
        //Debug.Log((temp.x / .5f).ToString("F2"));
        //Debug.Log((temp.y / .25f).ToString("F2"));
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

    public void setTurn(bool set)
    {
        this.isTurn = set;
    }

    public CList getCombatInfo()
    {
        return this.combatInfo;
    }

}