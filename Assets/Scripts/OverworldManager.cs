using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldManager : MonoBehaviour
{
    private GameManager gm;
    private GameObject player;
    public bool playerSpawned;


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerSpawned = false;
        this.gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!playerSpawned)
        {
            List<GameObject> nodes = new List<GameObject>();
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("OWNode"))
                nodes.Add(g);
            foreach (GameObject g in nodes)
                print(g);
        }

        // If we're in the overworld for the first time, plop the player character in
        if (SceneManager.GetActiveScene().name == "Overworld" && !playerSpawned)
        {
            spawnPlayer();
        }
    }

    void spawnPlayer()
    {
        string path = "Prefabs/PlayerCharacters/";
        path += gm.pm.playerScript.name;
        print(path);
        player = Instantiate(Resources.Load(path, typeof(GameObject))) as GameObject;
        player.transform.position = new Vector3(-.5f, 0f, 0f); // Should be changed to starting node
        playerSpawned = true;
        gm.pm.initPM();
    }
}
