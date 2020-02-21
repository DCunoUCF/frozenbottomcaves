using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldManager : MonoBehaviour
{
    private GameManager gm;
    private GameObject player;
    public bool playerSpawned;

    public List<GameObject> nodes;

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
    }

    void spawnPlayer()
    {
        player = Instantiate(Resources.Load("Prefabs/PlayerCharacters/TheWhiteKnight1", typeof(GameObject))) as GameObject;

        player.transform.position = new Vector3(-.5f, 0f, 0f); // Should be changed to starting node
        playerSpawned = true;
        gm.pm.initPM();
    }
}
