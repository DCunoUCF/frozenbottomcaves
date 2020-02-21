using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldManager : MonoBehaviour
{
    private GameManager gm;
    private GameObject player;
    private DialogueManager dm;
    public bool playerSpawned;

    public List<GameObject> nodes;
    public int playerNodeId;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerSpawned = false;
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
        		foreach (int id in n.GetComponent<WorldNode>().NodeIDs)
        		{
        			if (id == this.dm.currentNode)
        			{
        				// Move the player along the map
                        this.TurnPlayer(this.gm.pm.player, new Vector3(n.transform.position.x, n.transform.position.y, this.gm.pm.player.transform.position.z));
        				this.gm.pm.player.transform.position = new Vector3(n.transform.position.x, n.transform.position.y, this.gm.pm.player.transform.position.z);

        				// Rudimentary Camera Movement
        				GameObject cam = GameObject.Find("MainCamera");
        				cam.GetComponent<Camera>().transform.position = new Vector3(this.gm.pm.player.transform.position.x, this.gm.pm.player.transform.position.y, cam.GetComponent<Camera>().transform.position.z);

        				// Update the player node id
        				this.playerNodeId = id;
        			}
        		}
        	}
        }
    }

    void spawnPlayer()
    {
        player = Instantiate(Resources.Load("Prefabs/PlayerCharacters/TheWhiteKnight1", typeof(GameObject))) as GameObject;

        player.transform.position = new Vector3(-.5f, 0f, 0f); // Should be changed to starting node
        playerSpawned = true;
        gm.pm.initPM();
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
