using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Tilemaps;

public class OverworldManager : MonoBehaviour
{
    private struct nodeInfo
    {
        public int count, physNodeIndex;
        public WorldNode curNode;
        public GameObject physNode;
    }

    public GameManager gm;
    public GameObject player;
    public DialogueManager dm;
    public bool playerSpawned, initialSave;
    public GameObject rollParchment;
    public RollMaster rollScript;
    public GameObject die1, die2;
    public DiceRoller dr1, dr2;
    public bool updating;
    private overworldAnimations oa;

    public int startingNode;
    public List<GameObject> nodes;
    public int playerNodeId;
    public int nodeTypeCount;
    public bool load;
    public List<int> nodeSavedAt;
    public bool dontKillBMYet = false;

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
        this.oa = this.gameObject.AddComponent<overworldAnimations>();
        this.oa.om = this;
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

            GameObject tile = GameObject.Find("OverworldTilemap");
            Tilemap tilemap = tile.GetComponent<Tilemap>();
            //Tilemap obstaclesMap = tilemap.transform.GetChild(0).GetComponent<Tilemap>();
            BoundsInt bounds = tilemap.cellBounds;
            print("SIZE: " + tilemap.size);
            print("x: " + (Mathf.Abs(bounds.x) + bounds.xMax) + " y: " + (Mathf.Abs(bounds.y) + bounds.yMax));

            spawnPlayer();
        }

        if (playerSpawned)
            if (this.dm.currentNode == -1)
                this.HPEvent(-(this.gm.pm.pc.health));

        // Creates save at node 0
        if (playerSpawned && !initialSave)
        {
            if (this.dm.currentNode == 0)
            {
                nodeSavedAt.Add(0);

                // Tell the pm to create a deep copy of the current player and inventory
                this.gm.pm.createSave();

                // Remember the node information
                this.saveNode = 0;
                this.saveNodeTypeCount = 0;
                this.saveCurrentNode = 0;

                // Save direction they're facing
                if (player.transform.GetChild(0).gameObject.activeSelf)
                    facing = 0;
                else if (player.transform.GetChild(1).gameObject.activeSelf)
                    facing = 1;
                else if (player.transform.GetChild(2).gameObject.activeSelf)
                    facing = 2;
                else if (player.transform.GetChild(3).gameObject.activeSelf)
                    facing = 3;
                initialSave = true;
            }
        }

        if (!updating && playerSpawned)
        {
            updating = true;
            StartCoroutine(overworldUpdate());
        }
    }

    private nodeInfo getCurrentNode(int id)
    {
        nodeInfo node = new nodeInfo();
        for (int i = 0; i < nodes.Count; i++)
        {
            this.nodeTypeCount = 0;
            GameObject n = nodes[i];
            WorldNode tempNode = n.GetComponent<WorldNode>();
            foreach (int ID in tempNode.NodeIDs)
            {
                if (ID == this.dm.currentNode)
                {
                    node.count = nodeTypeCount;
                    node.curNode = tempNode;
                    node.physNode = n;
                    node.physNodeIndex = i;
                }
                nodeTypeCount++;
            }
        }
        return node;
    }

    IEnumerator overworldUpdate()
    {
        // Wait until the new node has been selected
        while (this.playerNodeId == this.dm.currentNode)
        {
            yield return null;
        }

        // dm current node has changed, grab it's information
        nodeInfo n = getCurrentNode(this.dm.currentNode);

        // If there is some sort of animation/sfx to play do it here
        yield return StartCoroutine(this.oa.events(this.dm.currentNode));

        // If the node position is not where the player is, move to it
        if (player.transform.position != n.physNode.transform.position)
        {
            print("Lets move");
            yield return StartCoroutine(slerpTest(n.physNode.transform.position));
        }
        // Update player's node id after moving there
        this.playerNodeId = this.dm.currentNode;
        this.dm.Panel.SetActive(true);
        this.dm.setInitialSelection();

        if (n.curNode.NodeTypes[n.count] == FlagType.Battle)
        {
            print("entered combat");
            yield return StartCoroutine(BattleEvent());
        }

        if (n.curNode.NodeTypes[n.count] == FlagType.STREvent)
        {
            print("entered str event");
            this.dm.putCanvasBehind();
            yield return StartCoroutine(SkillSaveEventCR("STR", n.physNode, n.curNode.SkillCheckDifficulty[n.count]));
            this.dm.putCanvasInFront();
        }
        if (n.curNode.NodeTypes[n.count] == FlagType.INTEvent)
        {
            print("entered int event");
            this.dm.putCanvasBehind();
            yield return StartCoroutine(SkillSaveEventCR("INT", n.physNode, n.curNode.SkillCheckDifficulty[n.count]));
            this.dm.putCanvasInFront();
        }
        if (n.curNode.NodeTypes[n.count] == FlagType.AGIEvent)
        {
            print("entered agi event");
            this.dm.putCanvasBehind();
            yield return StartCoroutine(SkillSaveEventCR("AGI", n.physNode, n.curNode.SkillCheckDifficulty[n.count]));
            this.dm.putCanvasInFront();
        }

        if (n.curNode.NodeTypes[n.count] == FlagType.HPEvent)
        {
            print("Hp event");
            this.HPEvent(n.curNode.HealthChange[n.count]);
        }

        if (n.curNode.NodeTypes[n.count] == FlagType.Item)
        {
            print("Getting item(s)");
            this.ItemGet(n.curNode.NodeItems[n.count].item, n.curNode.NodeItems[n.count].count);
        }

        if (n.curNode.NodeTypes[n.count] == FlagType.ItemLose)
        {
            print("Losing item(s)");
            this.ItemRemove(n.curNode.NodeItemsLose[n.count].item, n.curNode.NodeItemsLose[n.count].count);
        }

        if (n.curNode.NodeTypes[n.count] == FlagType.HPMaxEvent)
        {
            print("HP MAX event");
            this.HPMaxEvent(n.curNode.MaxHealthChange[n.count]);
        }

        if (n.curNode.NodeTypes[n.count] == FlagType.SaveEvent)
        {
            if (!nodeSavedAt.Contains(n.curNode.NodeIDs[n.count]))
            {
                nodeSavedAt.Add(n.curNode.NodeIDs[n.count]);

                // If this save event has the provisions checked, eat one 
                if (n.curNode.SaveProvisions[n.count])
                    this.gm.pm.pc.inventory.removeProvision();
                // Either way heal 5
                this.HPEvent(5);

                // Tell the pm to create a deep copy of the current player and inventory
                this.gm.pm.createSave();

                // Remember the node information
                this.saveNode = n.physNodeIndex;
                this.saveNodeTypeCount = n.count;
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

        updating = false;

        yield break;
    }

    // Hops on over to the destination node
    IEnumerator moveToDest(Vector3 dest)
    {
        this.dm.Panel.SetActive(false);
        TurnPlayer(player, dest);
        while (player.transform.position != dest)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, dest, 5f * Time.deltaTime);
            yield return null;
        }
        yield break;
    }

    IEnumerator slerpTest(Vector3 dest)
    {
        TurnPlayer(player, dest);
        Vector3 start = player.transform.position;
        float startTime = Time.time;
        float journeyTime = 1.0f;

        print(start + " end " + dest);

        while (player.transform.position != dest)
        {
            print("Slerping");
            Vector3 center = (start + dest) * 0.5f;

            center -= new Vector3(0, 0.1f, 0);

            Vector3 playerRelCenter = start - center;
            Vector3 endRelCenter = dest - center;

            float fracComplete = (Time.time - startTime) / journeyTime;

            player.transform.position = Vector3.Slerp(playerRelCenter, endRelCenter, fracComplete);
            player.transform.position += center;

            yield return null;
        }


        yield break;
    }

    public void loadSave()
    {
        this.gm.pm.loadSave();
        this.dm.currentNode = this.saveCurrentNode;
        this.startingNode = saveNode;
        this.nodeTypeCount = saveNodeTypeCount;

        this.dm.Panel.SetActive(true);
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
        //this.dontKillBMYet = true; // GM checks for this before killing BM. We need more coroutines
    }

    public IEnumerator SkillSaveEventCR(string stat, GameObject n, int difficulty)
    {
        print("IN SKILL SAVE");
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

        this.dm.EventComplete();
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
