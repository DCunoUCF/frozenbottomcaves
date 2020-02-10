using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum biome : short
{
	NOTHING = -1, FOREST, CAVE, ICECAVE, CASTLE, BOSS
};

public struct Cell
{
	public bool pass;
    public GameObject entity;

    public Cell(bool passIn, GameObject entityIn)
    {
        pass = passIn;
        entity = entityIn;
    }
}
public struct CList
{
    public GameObject entity;
    public bool move;
    public Vector3 movTar;
    public int dir;
    public int attack;
    public int attackDmg;
    public Vector3[] atkTar;

    public CList(GameObject newEntity)
    {
        entity = newEntity;
        move = false;
        movTar = new Vector3();
        dir = 0;
        attack = 0;
        attackDmg = 0;
        atkTar = null;
    }
}

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; set; }
    public List<CList> combatantList;
    private Cell[,] gridCell;
    private GameObject grid;
    private GameObject activeArena;
    private GameObject[] arenaDeactivate;
    private GameObject player;
    private GameObject companion;
    private GameObject[] enemies;
    private Vector3 playerLoc;
    private Vector3 companionLoc;
    private GameObject[] locGrabber;
    private List<Vector3> availEnemyLoc;
    private List<Vector3> enemyLoc;
    private int numEnemies;

    void Awake()
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

        combatantList = new List<CList>();
        grid = GameObject.Find("ForestGrid");
        activeArena = GameObject.Find("Arena1");
        arenaDeactivate = GameObject.FindGameObjectsWithTag("Tilemap");
        playerLoc = GameObject.FindGameObjectWithTag("pSpawn").transform.position;
        companionLoc = GameObject.FindGameObjectWithTag("cSpawn").transform.position;
        locGrabber = GameObject.FindGameObjectsWithTag("eSpawn");
        availEnemyLoc = new List<Vector3>();
        
        // Create the grid
        createGrid();

        // Using number of enemies to be spawned to initiliaze their fields and finding random locations for them to spawn
        numEnemies = 4;
        enemyLoc = new List<Vector3>(numEnemies);
        enemies = new GameObject[numEnemies];
        RandomEnemyPos();

        // Deactivate all arenas except for chosen arena
        for (int i = 0; i < arenaDeactivate.Length; i++)
        {
            if (arenaDeactivate[i] == activeArena)
                continue;
            arenaDeactivate[i].SetActive(false);
        }

        // Instantiate Player and Companion
        player = GameObject.Instantiate(GameObject.Find("TheWhiteKnight1"), playerLoc, Quaternion.identity);
        companion = GameObject.Instantiate(GameObject.Find("honey"), companionLoc, Quaternion.identity);
        
        // Instantiate Enemies
        for (int i = 0; i < numEnemies; i++)
        {
            enemies[i] = GameObject.Instantiate(GameObject.Find("goblin2"), enemyLoc[i], Quaternion.identity);
        }
    }

    private void Start()
    {
        PlayerManager.Instance.isTurn = true;
    }

    void Update()
    {
        if (!PlayerManager.Instance.isTurn)
        {
            resolveMoves();
            resolveAttacks();
        }
    }

    void createGrid()
    {
        Tilemap tilemap = activeArena.GetComponent<Tilemap>();
        BoundsInt bounds = tilemap.cellBounds;
        Debug.Log("bounds: " + bounds);
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
        Vector3 currentVector;

        gridCell = new Cell[bounds.size.x, bounds.size.y];

        int counter = 0;

        foreach (var position in tilemap.cellBounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(position))
            {
                // Debug.Log("position.x " + position.x + ", position.y " + position.y);
                gridCell[position.x - bounds.position.x, position.y - bounds.position.y] = new Cell(false, null);
                //Debug.Log("Cell x: " + x + "Cell y: " + y + "Cell pass: " + gridCell[x,y].pass + "Cell entity: " + gridCell[x,y].entity);
                continue;
            }

            // This x and y needs to be converted to the vector at the center of the tile to grab the GameObject entity from the tile
            currentVector = new Vector3(position.x, position.y, 0);
            Debug.Log("curVec " + currentVector);

            if (tilemap.GetTile(position).name != "isoWall1")
            {
                gridCell[position.x - bounds.position.x, position.y - bounds.position.y] = new Cell(true, getEntity(currentVector));
                counter++;

                Debug.Log("Cell x: " + (position.x - bounds.position.x)
                        + "Cell y: " + (position.y - bounds.position.y)
                        + "Cell pass: " + gridCell[position.x - bounds.position.x, position.y - bounds.position.y].pass
                        + "Cell entity: " + gridCell[position.x - bounds.position.x, position.y - bounds.position.y].entity);
                // Tile is not empty; do stuff
            }
            else
            {
                Debug.Log("Found iso wall at (" + position.x + ", " + position.y + ")");
            }
        }

        Debug.Log("We found " + counter + " tiles on the tilemap!");


/*        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null && tile.name != "isoWall1")
                {
                    gridCell[x,y] = new Cell(true, null);
                    
                    Debug.Log("Cell x: " + x + "Cell y: " + y + "Cell pass: " + gridCell[x,y].pass + "Cell entity: " + gridCell[x,y].entity);
                }
                else
                {
                    gridCell[x,y] = new Cell(false, null);
                    //Debug.Log("Cell x: " + x + "Cell y: " + y + "Cell pass: " + gridCell[x,y].pass + "Cell entity: " + gridCell[x,y].entity);
                }
            }
        }*/
    }

    void resolveMoves()
    {
        fillCombatantList();
        List<CList> moversList = new List<CList>();
        List<CList> attackersList = new List<CList>();
        bool popped = false;

        for (int i = 0; i < combatantList.Count; i++)
        {
            if (combatantList[i].move)
            {
                moversList.Add(combatantList[i]);
            }

            if (combatantList[i].attack > -1)
            {
                attackersList.Add(combatantList[i]);
            }
        }

        while (moversList.Count > 0)
        {
            // If mover at front of list is moving to a location another mover is move to, then pop both of them
            for (int i = 1; i < moversList.Count; i++)
            {
                if (moversList[0].movTar == moversList[i].movTar)
                {
                    moversList.RemoveAt(i);
                    moversList.RemoveAt(0);
                    popped = true;
                    break;
                }
            }

            // If mover at front of list is moving to a location someone is standing, then pop the mover
            for (int i = 0; i < attackersList.Count; i++)
            {
                if (moversList[0].movTar == attackersList[i].entity.transform.localPosition)
                {
                    moversList.RemoveAt(0);
                    popped = true;
                    break;
                }
            }

            // Check if mover at front of list is moving to an obstacle

            // If the mover won't collide with anyone else on the board, they can legally move to their target move location
            if (!popped)
            {
                moversList[0].entity.transform.SetPositionAndRotation(moversList[0].movTar, Quaternion.identity);
                PlayerManager.Instance.playerLoc = moversList[0].movTar;
                popped = false;
                moversList.RemoveAt(0);
            }
        }

        combatantList.Clear();
        PlayerManager.Instance.isTurn = true;
    }

    void resolveAttacks()
    {

    }

    void whoStillHasLimbs()
    {

    }

    GameObject getEntity(Vector3 pos)
    {
        for (int i = 0; i < this.combatantList.Count; i++)
        {
            if (combatantList[i].entity.transform.position == pos)
                return combatantList[i].entity;
        }

        return null;
    }

    void fillCombatantList()
    {
        combatantList.Add(PlayerManager.Instance.combatInfo);
        combatantList.Add(new CList(companion));

        for (int i = 0; i < numEnemies; i++)
        {
            combatantList.Add(new CList(enemies[i]));
        }
    }

    void RandomEnemyPos()
    {
        int random;

        foreach (GameObject i in locGrabber)
        {
            availEnemyLoc.Add(i.transform.position);
        }

        for(int i = 0; i < numEnemies; i++)
        {
            random = (int) Random.Range(0, availEnemyLoc.Count-1);
            enemyLoc.Add(availEnemyLoc[random]);
            availEnemyLoc.RemoveAt(random);
        }
    }
}
