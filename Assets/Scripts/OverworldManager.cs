using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// using UnityEngine.UI;
using UnityEngine.UIElements;

public class OverworldManager : MonoBehaviour
{
    public GameManager gm;
    private GameObject player;
    public DialogueManager dm;
    public bool playerSpawned;
    public GameObject rollParchment;
    public RollMaster rollScript;
    public GameObject die1, die2;
    public DiceRoller dr1, dr2;

    public int startingNode;
    public List<GameObject> nodes;
    public int playerNodeId;
    public int nodeTypeCount;
    public bool load;
    public List<int> nodeSavedAt;

    public int saveNode, saveNodeTypeCount, saveCurrentNode, facing;

    private Vector3 destPos;
    private float speed = .20f, startTime, journeyLength;
    private bool destReached;

    WorldNode curNode;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerSpawned = false;
        this.gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        destReached = true;
        startingNode = 0;
        nodeSavedAt = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        // If we're in the overworld for the first time, plop the player character in
        if (SceneManager.GetActiveScene().name == "Overworld" && !playerSpawned)
        {
        	this.dm = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
            this.dm.om = this;
	        this.playerNodeId = this.dm.currentNode;
	        Debug.Log("OverworldManager sees the player at " + this.playerNodeId);

        	nodes = new List<GameObject>();

	        foreach (GameObject n in GameObject.FindGameObjectsWithTag("OWNode"))
	        {
	        	nodes.Add(n);
	        }

            spawnPlayer();
        }

        if (playerSpawned && this.playerNodeId != this.dm.currentNode && destReached)
        {
        	for(int i = startingNode; i < nodes.Count; i++)
        	{
                load = false;
                GameObject n = nodes[i];
        		this.nodeTypeCount = 0;
                curNode = n.GetComponent<WorldNode>();
        		foreach (int id in curNode.NodeIDs)
        		{
        			if (id == this.dm.currentNode)
        			{
                        Debug.Log("currentNode OM: " + this.dm.currentNode);
                        // Move the player along the map
                        this.TurnPlayer(this.player, new Vector3(n.transform.position.x, n.transform.position.y, this.player.transform.position.z));
        				//this.player.transform.position = new Vector3(n.transform.position.x, n.transform.position.y, this.player.transform.position.z);


                        
                        destPos = new Vector3(n.transform.position.x, n.transform.position.y, this.player.transform.position.z);

                        if (player.transform.position != destPos)
                        {
                            destReached = false;
                            dm.Panel.SetActive(false);
                        }

                        startTime = Time.time;
                        journeyLength = Vector3.Distance(player.transform.position, destPos);

        				// Rudimentary Camera Movement
        				//GameObject cam = GameObject.Find("MainCamera");
        				//cam.GetComponent<Camera>().transform.position = new Vector3(this.player.transform.position.x, this.player.transform.position.y, cam.GetComponent<Camera>().transform.position.z);

        				// Update the player node id
        				this.playerNodeId = id;

        				if (curNode.NodeTypes[this.nodeTypeCount] == FlagType.Battle)
        				{
                            print("entered combat");
                            StartCoroutine(BattleEvent());
                        }

                        if (curNode.NodeTypes[this.nodeTypeCount] == FlagType.STREvent)
                        {
                            print("entered str event");
                            this.dm.putCanvasBehind();
                            StartCoroutine(SkillSaveEventCR("STR", n, curNode.SkillCheckDifficulty[this.nodeTypeCount]));
                            this.dm.putCanvasInFront();
                        }
                        if (curNode.NodeTypes[this.nodeTypeCount] == FlagType.INTEvent)
                        {
                            print("entered int event");
                            this.dm.putCanvasBehind();
                            StartCoroutine(SkillSaveEventCR("INT", n, curNode.SkillCheckDifficulty[this.nodeTypeCount]));
                            this.dm.putCanvasInFront();
                        }
                        if (curNode.NodeTypes[this.nodeTypeCount] == FlagType.AGIEvent)
                        {
                            print("entered agi event");
                            this.dm.putCanvasBehind();
                            StartCoroutine(SkillSaveEventCR("AGI", n, curNode.SkillCheckDifficulty[this.nodeTypeCount]));
                            this.dm.putCanvasInFront();
                            //this.SkillSaveEvent("AGI");
                        }

                        if (curNode.NodeTypes[this.nodeTypeCount] == FlagType.HPEvent)
                        {
                            print("Hp event");
                            this.HPEvent(curNode.HealthChange[this.nodeTypeCount]);
                        }

                        if (curNode.NodeTypes[this.nodeTypeCount] == FlagType.Item)
                        {
                            print("Getting item(s)");
                            this.ItemGet(curNode.NodeItems[this.nodeTypeCount].item, curNode.NodeItems[this.nodeTypeCount].count);
                        }

                        if (curNode.NodeTypes[this.nodeTypeCount] == FlagType.ItemLose)
                        {
                            print("Losing item(s)");
                            this.ItemRemove(curNode.NodeItemsLose[this.nodeTypeCount].item, curNode.NodeItemsLose[this.nodeTypeCount].count);
                        }

                        if (curNode.NodeTypes[this.nodeTypeCount] == FlagType.HPMaxEvent)
                        {
                            print("HP MAX event");
                            this.HPMaxEvent(curNode.MaxHealthChange[this.nodeTypeCount]);
                        }

                        if (curNode.NodeTypes[this.nodeTypeCount] == FlagType.SaveEvent)
                        {
                            if (!nodeSavedAt.Contains(curNode.NodeIDs[this.nodeTypeCount]))
                            {
                                nodeSavedAt.Add(curNode.NodeIDs[this.nodeTypeCount]);
                                
                                // If this save event has the provisions checked, eat one 
                                if (curNode.SaveProvisions[this.nodeTypeCount])
                                    this.gm.pm.pc.inventory.removeProvision();
                                // Either way heal 5
                                this.HPEvent(5);

                                // Tell the pm to create a deep copy of the current player and inventory
                                this.gm.pm.createSave();

                                // Remember the node information
                                this.saveNode = i;
                                this.saveNodeTypeCount = this.nodeTypeCount;
                                this.saveCurrentNode = this.dm.currentNode;

                                // Save direction they're facing
                                if (player.transform.GetChild(0).gameObject.activeSelf)
                                    facing = 0;
                                else if (player.transform.GetChild(1).gameObject.activeSelf)
                                    facing = 1;
                                else if (player.transform.GetChild(2).gameObject.activeSelf)
                                    facing = 2;
                                else if (player.transform.GetChild(3).gameObject.activeSelf)
                                    facing = 3;
                            }
                        }

                        if (curNode.NodeTypes[this.nodeTypeCount] == FlagType.LoadEvent)
                        {
                            this.loadSave();
                            load = true;
                        }
                    }

                    this.nodeTypeCount++;
                    if (load)
                        break;
        		}
                if (load)
                    break;
        	}
        }
        else if (playerSpawned && !gm.pm.inCombat && !destReached)
        {
            movePlayer();
            if (player.transform.position == destPos)
            {
                destReached = true;
                dm.Panel.SetActive(true);
                dm.setInitialSelection();
            }

        }
    }

    public void loadSave()
    {
        this.gm.pm.loadSave();
        this.dm.currentNode = this.saveCurrentNode;
        this.startingNode = saveNode;
        this.nodeTypeCount = saveNodeTypeCount;

        this.dm.init();

        destPos = new Vector3(nodes[startingNode].transform.position.x, nodes[startingNode].transform.position.y, nodes[startingNode].transform.position.z);
        this.player.transform.position = new Vector3(nodes[startingNode].transform.position.x,
                                                     nodes[startingNode].transform.position.y,
                                                     nodes[startingNode].transform.position.z);
        movePlayer();
        for (int i = 0; i < 4; i++)
        {
            if (i == facing)
                this.player.transform.GetChild(i).gameObject.SetActive(true);
            else
                this.player.transform.GetChild(i).gameObject.SetActive(false);
        }
        this.gm.om.dm.setInterableAll();
        this.gm.om.dm.setInitialSelection();
    }

    private void movePlayer()
    {
        float distCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distCovered / journeyLength;
        player.transform.position = Vector3.Lerp(player.transform.position, destPos, fractionOfJourney);
    }

    private void spawnPlayer()
    {
        string path = "Prefabs/PlayerCharacters/";
        path += gm.pm.pc.name;
        print(path + " " + gm.pm.pc.name);
        player = Instantiate(Resources.Load(path, typeof(GameObject))) as GameObject;
        player.transform.position = GameObject.Find("0").transform.position; // hard coding node 0
        print("node 0:" + nodes[0].transform.position);
        playerSpawned = true;
        GameObject cam = GameObject.Find("MainCameraOW");
        cam.transform.SetParent(player.transform);
        cam.transform.localPosition = new Vector3(0, 0, -10);
        gm.pm.initPM();


        rollParchment = GameObject.Find("RollParchment");
        rollScript = rollParchment.GetComponent<RollMaster>();
        die1 = GameObject.Find("d1");
        die2 = GameObject.Find("d2");
        dr1 = die1.GetComponent<DiceRoller>();
        dr2 = die2.GetComponent<DiceRoller>();
        rollParchment.SetActive(false);
    }


    public IEnumerator BattleEvent()
    {
        this.dm.Panel.SetActive(false);
        this.rollParchment.SetActive(false);
        //this.gm.sm.setMusicFromDirectory("ForestBattleMusic");
        this.gm.sm.setBattleMusic();
        SceneManager.LoadScene("Battleworld", LoadSceneMode.Additive);
        this.gm.pm.combatInitialized = true;
        this.gm.pm.inCombat = true;

        yield return new WaitUntil(() => this.gm.bm != null);
        yield return new WaitUntil(() => this.gm.bm.isBattleResolved() == true);

        if (this.gm.bm.didWeWinTheBattle())
            this.dm.currentNode += 1;
        else
            this.dm.currentNode += 2;
        
        this.gm.pm.combatInitialized = false;
        this.gm.pm.inCombat = false;

        //SceneManager.LoadScene("Overworld", LoadSceneMode.Single);
        
        this.dm.Panel.SetActive(true);
        this.dm.EventComplete();
        //dm.setInitialSelection();
    }

    public void SkillSaveEvent(string stat)
    {
        // Check which skill the event is for from WorldNode struct
        // Maybe have difficulties in the WorldNode struct to alter how high the roll needs to be
        // Call getters to the playerClass/Manager to check the player's skill
        // Do random chance roll
        // Setter for dm.currentNode += 1(save) or += 2(fail)
        this.dm.Panel.SetActive(false);
        int modifier = this.gm.pm.getStatModifier(stat);
        int r1 = Random.Range(1, 7);
        int r2 = Random.Range(1, 7);
        print("roll 1 " + r1 + " roll 2 " + r2 + "modifier " + modifier);

        if (r1+r2+modifier < 3)
        {
            print("FAIL");
            this.dm.currentNode += 2;
        }
        else
        {
            print("SAVE");
            this.dm.currentNode += 1;
        }


        this.dm.Panel.SetActive(true);
        this.dm.EventComplete();
        dm.setInitialSelection();
    }

    public IEnumerator SkillSaveEventCR(string stat, GameObject n, int difficulty)
    {
        this.dm.Panel.SetActive(false);
        this.rollParchment.SetActive(true);
        yield return StartCoroutine(rollScript.waitForStart(stat, this.gm.pm.getStatModifier(stat), difficulty));
        this.rollParchment.SetActive(false);

        int modifier = this.gm.pm.getStatModifier(stat);
        if (dr1.final + dr2.final + modifier < difficulty)
        {
            print("FAIL");
            this.dm.currentNode += 2;
        }
        else
        {
            print("SAVE");
            this.dm.currentNode += 1;
        }

        yield return new WaitForSeconds(.05f);
        this.dm.Panel.SetActive(true);
        this.dm.EventComplete();
        dm.setInitialSelection();
        if (player.transform.position == n.transform.position)
            this.dm.Panel.SetActive(true);
        else
            this.dm.Panel.SetActive(false);
        dr1.final = 0;
        dr2.final = 0;
        yield break;
    }

    public void HPEvent(int dmg)
    {
        this.gm.pm.setHealthEvent(dmg);
    }

    public void HPMaxEvent(int change)
    {
        this.gm.pm.maxHealthEvent(change);
    }

    public void ItemGet(Item.ItemType item, int count)
    {
        this.gm.pm.pc.inventory.addItem(item, count);
    }

    public void ItemRemove(Item.ItemType item, int count)
    {
        this.gm.pm.pc.inventory.removeItem(item, count);
    }

    public GameObject GetCurrentNode()
    {
        WorldNode curWorldNode;

        if (this.nodes == null)
        {
            Debug.AssertFormat(false, "OWNodes list(nodes) has not been created yet.");
            return null;
        }

        for (int i = 0; i < this.nodes.Count; i++)
        {
            curWorldNode = this.nodes[i].GetComponent<WorldNode>();

            for (int j = 0; j < curWorldNode.NodeIDs.Count; j++)
            {
                if (curWorldNode.NodeIDs[j] == this.dm.currentNode)
                    return this.nodes[i];
            }
        }


        Debug.AssertFormat(false, "Couldn't find currentNode " + this.dm.currentNode + " in OWNodes list(nodes).");
        return null;
    }

    public BattleClass GetBattleClass()
    {
        WorldNode curWorldNode = this.GetCurrentNode().GetComponent<WorldNode>();

        for (int i = 0; i < curWorldNode.NodeIDs.Count; i++)
        {
            if (curWorldNode.NodeIDs[i] == this.dm.currentNode)
                return curWorldNode.battleClassList.list[i];
        }

        Debug.AssertFormat(false, "Couldn't find BattleClass in BattleClassList at currentNode " + this.dm.currentNode);
        return null;
    }

    void TurnPlayer(GameObject entity, Vector3 movTar)
    {
        // How I WILL do it later entity.dir... maybe?
        float dirX, dirY;
        dirX = movTar.x - entity.transform.localPosition.x;
        dirY = movTar.y - entity.transform.localPosition.y;

        GameObject SE = entity.transform.GetChild(0).gameObject, SW = entity.transform.GetChild(1).gameObject,
                   NW = entity.transform.GetChild(2).gameObject, NE = entity.transform.GetChild(3).gameObject;

        if (dirX > 0)
        {
            if (dirY > 0)
            {
                SE.gameObject.SetActive(false);
                SW.gameObject.SetActive(false);
                NW.gameObject.SetActive(false);
                NE.gameObject.SetActive(true);
            }
            else
            {
                SE.gameObject.SetActive(true);
                SW.gameObject.SetActive(false);
                NW.gameObject.SetActive(false);
                NE.gameObject.SetActive(false);
            }

        }
        else if (dirX < 0)
        {
            if (dirY < 0)
            {
                SE.gameObject.SetActive(false);
                SW.gameObject.SetActive(true);
                NW.gameObject.SetActive(false);
                NE.gameObject.SetActive(false);
            }
            else
            {
                SE.gameObject.SetActive(false);
                SW.gameObject.SetActive(false);
                NW.gameObject.SetActive(true);
                NE.gameObject.SetActive(false);
            }
        }
    }
}
