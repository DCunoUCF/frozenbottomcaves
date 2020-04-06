using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; set; }

    public GameManager gm;

    // Info about player, PC, name, and gameobject
    [SerializeField]
    public PlayerClass pc;
    public string characterName;
    public string characterNameClone;
    public GameObject player;
    public bool characterSelected, characterFoundOW;

    // Inventory stuff
    public Canvas inventoryCanvas;
    public Inventory inventory;
    public UIInventory inventoryUI;
    public GameObject InventoryPanel;
    public GameObject BioPanel;
    public GameObject AttackPanel;

    // Combat information, clist, bools to guard turn logic
    public Vector3 playerLoc, selectedTile;
    public bool inCombat, isTurn, selectingSkill;
    public List<GameObject> highlights;
    public int x, y;
    public int movx = 0, movy = 0;
    public bool moved;
    public bool combatInitialized, hold;
    public CList combatInfo;

    // References to highlight manager, to communicate with highlighted tiles in combat
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
        // If we're in the OW and the player gameobject has not yet been assigned
        if (!inCombat && characterSelected && !characterFoundOW)
        {
            print("finding character");
            player = GameObject.Find(characterNameClone);
            if (player != null)
                characterFoundOW = true;
        }
        else if(inCombat)// We fightin now bois
        {
            if (moved)
            {
                this.x = this.gm.bm.combatantList[0].gridX;
                this.y = this.gm.bm.combatantList[0].gridY;
                movx = 0;
                movy = 0;
                moved = false;
                print("Final x,y: " + x + ", " + y);
            }

            // Player can select what ability/move to use
            playerTurnCombat();
        }
        else // Not in combat, just check if player wants to open inventory
        {
            inventoryCheck();
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

    // Fills in a few fields for PM to recognize player, as well as set up inventory
    public void initPM()
    {
        characterName = pc.name;
        characterNameClone = pc.clonename;
        characterSelected = true;

        inventory = new Inventory(this);
        inventoryCanvas = GameObject.Find("InventoryCanvas").GetComponent<Canvas>();
        inventoryUI = (UIInventory)GameObject.Find("Inventory").GetComponent("UIInventory");
        InventoryPanel = GameObject.Find("InventoryPanel");
        BioPanel = GameObject.Find("BioPanel");
        AttackPanel = GameObject.Find("AttackPanel");

        pc.inventory = inventory;
        pc.quest = "This is a test quest to see how the text wraps within the text box";
        pc.inventory.updateStats(pc);
        pc.inventory.addItem(Item.ItemType.Sword, 5);
        pc.inventory.addItem(Item.ItemType.Ressurection, 3);
        pc.inventory.addItem(Item.ItemType.Provisions, 5);
        pc.inventory.addItem(Item.ItemType.Gold, 100);
        pc.inventory.addItem(Item.ItemType.Sword, 50);
        pc.inventory.removeItem(Item.ItemType.Sword, 50);
        pc.inventory.addItem(Item.ItemType.Provisions, 10);

        inventoryUI.gameObject.SetActive(false);
        //inventoryCanvas.gameObject.SetActive(false);
        inventoryCanvas.transform.SetParent(gm.transform);
        inventoryCanvas.sortingOrder = 5;
    }

    private void inventoryCheck()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            print("Button press");

            if (inventoryUI.gameObject.activeSelf)
            {
                inventoryUI.gameObject.SetActive(false);
                gm.om.dm.setInteractable();
            }
            else
            {
                gm.om.dm.setUninteractable();
                inventory.updateStats(pc);
                InventoryPanel.SetActive(true);
                BioPanel.SetActive(false); // This is set active by default already in Inventory.cs
                AttackPanel.SetActive(false);
                
                inventoryUI.gameObject.SetActive(true);
                inventory.setInitSelection();
            }
        }
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
                clearHighlights();
                placeHighlights(pc.skill3, 3);
                this.abilityinfo = pc.getInfo(3);
                fillCombatInfo(abilityinfo);
            }
            else if (Input.GetButtonDown("Skill4"))
            {
                clearHighlights();
                placeHighlights(pc.skill4, 4);
                this.abilityinfo = pc.getInfo(4);
                fillCombatInfo(abilityinfo);
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
    public void setSelectedTile(List<Vector3> pos)
    {
        if (this.isTurn)
        {
            clearHighlights();
            //this.selectedTile = pos;
            this.combatInfo.movTar = pos[0];
            this.combatInfo.atkTar = pos;
            //Debug.Log(selectedTile.ToString("F2"));
            this.isTurn = false;
            //getMoveXY(pos);
            this.selectingSkill = true;
            BattleManager.Instance.combatantList[0] = this.combatInfo;
            clearHighlights();
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

    // Waits .3s, used to wait until child highlights are properly orphaned
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

        int direction = 0;

        if (this.player.transform.GetChild(0).gameObject.activeSelf) // SE
            direction = 2;
        else if (this.player.transform.GetChild(1).gameObject.activeSelf) // SW
            direction = 3;
        else if (this.player.transform.GetChild(2).gameObject.activeSelf) // NW
            direction = 0;
        else if (this.player.transform.GetChild(3).gameObject.activeSelf) // NE
            direction = 1;

        if (points.Count == 8)
            direction *= 2;


        if (!hold)
        {
            GameObject[] highlight = pc.getHighlight(key);

            int i = -1;
            foreach (Point tile in points)
            {
                i++;
                int newX = x + tile.X;
                int newY = y + tile.Y;
                if (newX < 0 || newX > BattleManager.Instance.gridCell.GetLength(1))
                    continue;

                if (newY < 0 || newY > BattleManager.Instance.gridCell.GetLength(0))
                    continue;

                if (i == 1 || i == 3)
                {
                    if (BattleManager.Instance.gridCell[newX, newY] != null)
                        if (BattleManager.Instance.gridCell[newX, newY].pass)
                            highlights.Add(Instantiate(highlight[1],
                                  BattleManager.Instance.gridCell[newX, newY].center, Quaternion.identity));
                }
                else
                {
                    if (BattleManager.Instance.gridCell[newX, newY] != null)
                            if (BattleManager.Instance.gridCell[newX, newY].pass)
                                highlights.Add(Instantiate(highlight[0],
                                      BattleManager.Instance.gridCell[newX, newY].center, Quaternion.identity));
                }




                //if (BattleManager.Instance.gridCell[newX, newY] != null)
                //    if (BattleManager.Instance.gridCell[newX, newY].pass)
                //        highlights.Add(Instantiate(highlight,
                //              BattleManager.Instance.gridCell[newX, newY].center, Quaternion.identity));
            }
            foreach (GameObject hl in highlights)
            {
                hl.transform.SetParent(HM.transform);
            }
            HMScript.setTiles(highlights, direction);
        }
        hold = true;
    }


    // Returns the requested stat, 1 - str, 2 - int, 3 - dex
    public int getStat(string i)
    {
        return pc.getStat(i);
    }

    public void takeDmg(int i)
    {
        pc.takeDamage(i);
    }

}
