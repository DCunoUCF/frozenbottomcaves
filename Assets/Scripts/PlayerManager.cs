using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    public PlayerClass playerScript;
    public string characterName;
    public GameObject player;
    public Vector3 playerLoc;
    public bool inCombat;
    public bool isTurn;
    public bool selectingSkill;
    public List<GameObject> highlights;



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
        characterName = "TheWhiteKnight1(Clone)";
        player = GameObject.Find(characterName);
        playerLoc = player.transform.position;
        playerScript = (PlayerClass) player.GetComponent(typeof(PlayerClass));
        inCombat = true;
        isTurn = true;
        selectingSkill = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inCombat)
        {
            // do overworld stuff
        }
        else
        {
            // playerTurn();
            // When player turn and selecting ability take input to find which ability
            // Instantiate highlights on the correct tiles
            if (isTurn && selectingSkill)
            {
                // Read input
                Debug.Log("looking for input");
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    Debug.Log("key pressed");
                    playerScript.useSkill(1, playerLoc);
                    selectingSkill = false;
                    highlights = playerScript.useSkill(1, playerLoc);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    // do some other ability
                }
            }
            // Still player turn, they have pressed a skill and are confirming location by selecting a tile
            else if (isTurn && !selectingSkill)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    foreach (GameObject highlight in highlights)
                        Destroy(highlight);
                    selectingSkill = true;
                }
            }
        }
    }

    //public void playerTurn()
    //{
    //    if (isTurn && selectingSkill)
    //    {
    //        // Read input
    //        Debug.Log("looking for input");
    //        if (Input.GetKeyDown(KeyCode.Alpha1))
    //        {
    //            Debug.Log("key pressed");
    //            highlights = playerScript.useSkill(1, playerLoc);
    //            selectingSkill = false;
    //            //highlights = GameObject.FindGameObjectsWithTag("attackhighlight");
    //        }
    //        else if (Input.GetKeyDown(KeyCode.Alpha2))
    //        {
    //            // do some other ability
    //        }
    //    }
    //    // Still player turn, they have pressed a skill and are confirming location by selecting a tile
    //    else if (isTurn && !selectingSkill)
    //    {
    //        if (Input.GetKeyDown(KeyCode.Escape))
    //        {
    //            foreach (GameObject highlight in highlights)
    //                Destroy(highlight);
    //            selectingSkill = true;
    //        }
    //    }
    //}
}
