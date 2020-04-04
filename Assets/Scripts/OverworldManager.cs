using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// using UnityEngine.UI;
using UnityEngine.UIElements;

public class OverworldManager : MonoBehaviour
{
    private GameManager gm;
    private GameObject player;
    public DialogueManager dm;
    public bool playerSpawned;

    public List<GameObject> nodes;
    public int playerNodeId;
    public int nodeTypeCount;

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
        // nodes = new List<GameObject>();

        // foreach (GameObject n in GameObject.FindGameObjectsWithTag("OWNode"))
        // {
        // 	nodes.Add(n);
        // }

        // nodes = GameObject.FindGameObjectsWithTag("OWNode");

        // if (GameObject.Find("Node0") != null)
        // 	Debug.Log("Hooray!");
        // else
        // 	Debug.Log("Fail whale :(");

        // Debug.Log(OWNodes.ToString());
    }

    // Update is called once per frame
    void Update()
    {
/*        if(!playerSpawned)
        {
            List<GameObject> nodes = new List<GameObject>();
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("OWNode"))
                nodes.Add(g);
            foreach (GameObject g in nodes)
                print(g);
        }*/

        // If we're in the overworld for the first time, plop the player character in
        if (SceneManager.GetActiveScene().name == "Overworld" && !playerSpawned)
        {
        	this.dm = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
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
        	foreach (GameObject n in nodes)
        	{
        		this.nodeTypeCount = 0;
                curNode = n.GetComponent<WorldNode>();
        		foreach (int id in curNode.NodeIDs)
        		{
        			if (id == this.dm.currentNode)
        			{
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
                            this.SkillSaveEvent("STR");
                        }
                        if (curNode.NodeTypes[this.nodeTypeCount] == FlagType.INTEvent)
                        {
                            print("entered int event");
                            this.SkillSaveEvent("INT");
                        }
                        if (curNode.NodeTypes[this.nodeTypeCount] == FlagType.AGIEvent)
                        {
                            print("entered agi event");
                            this.SkillSaveEvent("AGI");
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
                    }

                    this.nodeTypeCount++;
        		}
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
    }


    public IEnumerator BattleEvent()
    {
        this.dm.Panel.SetActive(false);
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
