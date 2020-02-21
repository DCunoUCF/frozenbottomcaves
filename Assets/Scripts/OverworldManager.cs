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
    private DialogueManager dm;
    public bool playerSpawned;

    public List<GameObject> nodes;
    public int playerNodeId;

    public bool panic;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerSpawned = false;
        this.panic = false;
        this.gm = GameObject.Find("GameManager").GetComponent<GameManager>();
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
        // If we're in the overworld for the first time, plop the player character in
        if (SceneManager.GetActiveScene().name == "Overworld" && !playerSpawned)
        {
        	this.dm = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
	        this.playerNodeId = this.dm.currentNode;
	        Debug.Log("OverworldManager sees the player at "+this.playerNodeId);

        	nodes = new List<GameObject>();

	        foreach (GameObject n in GameObject.FindGameObjectsWithTag("OWNode"))
	        {
	        	nodes.Add(n);
	        }

	        if (GameObject.Find("Node0") != null)
	        	Debug.Log("Hooray!");
	        else
	        	Debug.Log("Fail whale :(");
            spawnPlayer();
        }

        if (this.playerNodeId != this.dm.currentNode)
        {
        	foreach (GameObject n in nodes)
        	{
        		int counter = 0;

        		foreach (int id in n.GetComponent<WorldNode>().NodeIDs)
        		{
        			if (id == this.dm.currentNode)
        			{
        				// Move the player along the map
        				this.gm.pm.player.transform.position = new Vector3(n.transform.position.x, n.transform.position.y, this.gm.pm.player.transform.position.z);

        				// Rudimentary Camera Movement
        				GameObject cam = GameObject.Find("MainCamera");
        				cam.GetComponent<Camera>().transform.position = new Vector3(this.gm.pm.player.transform.position.x, this.gm.pm.player.transform.position.y, cam.GetComponent<Camera>().transform.position.z);

        				// Update the player node id
        				this.playerNodeId = id;

        				if (n.GetComponent<WorldNode>().NodeTypes[counter] == FlagType.Battle && this.panic)
        				{
        					// OpenDemoLevel();
        					SceneManager.LoadScene("Battleworld", LoadSceneMode.Single);
			                // this.gm.sm.setBattleMusic();
			                this.gm.sm.setMusicFromDirectory("ForestBattleMusic");
			                gm.pm.combatInitialized = true;
			                gm.pm.inCombat = true;
			                this.panic = false;
        				}

        				counter++;
        			}
        		}
        	}
        }

        this.panic = false;
    }



    void spawnPlayer()
    {
        player = Instantiate(Resources.Load("Prefabs/PlayerCharacters/TheWhiteKnight1", typeof(GameObject))) as GameObject;

        player.transform.position = new Vector3(-.5f, 0f, 0f); // Should be changed to starting node
        playerSpawned = true;
        gm.pm.initPM();
    }
}
