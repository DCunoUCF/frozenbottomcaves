using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Biome : short
{
	NOTHING = -1, FOREST, CAVE, ICECAVE, CASTLE, BOSS
};

public struct Cell
{
	public bool pass;
    public GameObject entity;
    public Vector3 center;

    public Cell(bool passIn, GameObject entityIn, Vector3 centerIn)
    {
        center = centerIn;
        pass = passIn;
        entity = entityIn;
    }

    public bool Pass
    {
        get { return pass; }
        set { pass = value; }
    }

    public GameObject Entity
    {
        get { return entity; }
        set { entity = value; }
    }

}
public struct CList
{
    public GameObject entity;
    public bool move;
    public Vector3 movTar;
    public Vector3[] atkTar;
    public int dir, attack, attackDmg;

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
    private List<GameObject> entitiesList;
    public Cell[,] gridCell;
    private GameObject grid;
    private GameObject activeArena;
    private GameObject[] arenaDeactivate;
    private GameObject[] gridDeactivate;
    private GameObject player;
    private int playerX, playerY;
    private GameObject companion;
    private GameObject[] enemies;
    private Vector3 playerLoc;
    private Vector3 companionLoc;
    private GameObject[] locGrabber;
    private List<Vector3> availEnemyLoc;
    private List<Vector3> enemyLoc;
    private int numEnemies;
    private GameObject[] enemyType;

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
        grid = GameObject.Find("ForestGrid"); // Overworld will set this
        activeArena = GameObject.Find("Arena1"); // Overworld will set this
        arenaDeactivate = GameObject.FindGameObjectsWithTag("Tilemap");
        gridDeactivate = GameObject.FindGameObjectsWithTag("Grid");
        playerLoc = GameObject.FindGameObjectWithTag("pSpawn").transform.position;
        companionLoc = GameObject.FindGameObjectWithTag("cSpawn").transform.position;
        locGrabber = GameObject.FindGameObjectsWithTag("eSpawn");
        entitiesList = new List<GameObject>();
        availEnemyLoc = new List<Vector3>();

        // Deactivate all grids except for chosen grid
        for (int i = 0; i < gridDeactivate.Length; i++)
        {
            if (gridDeactivate[i] == grid)
                continue;
            gridDeactivate[i].SetActive(false);
        }

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
        entitiesList.Add(player);
        entitiesList.Add(companion);

        // Using number of enemies to be spawned to initiliaze their fields and finding random locations for them to spawn
        numEnemies = 4; // Overworld will set this
        enemyLoc = new List<Vector3>(numEnemies);
        enemyType = new GameObject[numEnemies];
        enemies = new GameObject[numEnemies];
        
        RandomEnemyPos();

        // Instantiate Enemies
        for (int i = 0; i < numEnemies; i++)
        {
            enemies[i] = GameObject.Instantiate(GameObject.Find("goblin2"), enemyLoc[i], Quaternion.identity); // Overworld will set the enemy types
            entitiesList.Add(enemies[i]);
        }



        // 
        FillCombatantList();



        // Creating the Grid
        CreateGrid();



    }

    private void Start()
    {
        PlayerManager.Instance.isTurn = true;
        PlayerManager.Instance.x = playerX;
        PlayerManager.Instance.y = playerY;
    }

    void Update()
    {
        if (!PlayerManager.Instance.isTurn)
        {
            ResolveMoves();
            ResolveAttacks();
            WhoStillHasLimbs();
        }
    }

    void CreateGrid()
    {
        float xVec, yVec;
        Vector3 currentVector;
        GameObject tileEntity;
        Tilemap tilemap = activeArena.GetComponent<Tilemap>();
        BoundsInt bounds = tilemap.cellBounds;

        this.gridCell = new Cell[bounds.size.x, bounds.size.y];

        foreach (var position in tilemap.cellBounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(position))
            {
                continue;
            }

            // This x and y needs to be converted to the vector at the center of the tile to grab the GameObject entity from the tile
            xVec = (position.x * 0.5f) - (position.y * 0.5f);
            yVec = ((position.x + 1) * 0.25f) + (position.y * 0.25f);
            currentVector = new Vector3(xVec, yVec, 0);
            tileEntity = GetEntity(currentVector);

            if (tilemap.GetTile(position).name != "isoWall1" && tileEntity == null)
            {
                this.gridCell[position.x - bounds.position.x, position.y - bounds.position.y] = new Cell(true, tileEntity, currentVector);
            }
            else if (tilemap.GetTile(position).name != "isoWall1" && tileEntity != null)
            {
                this.gridCell[position.x - bounds.position.x, position.y - bounds.position.y] = new Cell(false, tileEntity, currentVector);

                if (tileEntity == player)
                {
                    playerX = (position.x - bounds.position.x);
                    playerY = (position.y - bounds.position.y);
                    
                }

                Debug.Log(gridCell[position.x - bounds.position.x, position.y - bounds.position.y].entity);
                Debug.Log(position.x - bounds.position.x);
                Debug.Log(position.y - bounds.position.y);

                //combatantList[FindInCombatantList(tileEntity)].entity.x = (position.x - bounds.position.x);
                //combatantList[FindInCombatantList(tileEntity)].entity.y = (position.y - bounds.position.y);
            }
            else
            {
                this.gridCell[position.x - bounds.position.x, position.y - bounds.position.y] = new Cell(false, tileEntity, currentVector);
            }
        }

        foreach (Cell cell in gridCell)
            if (cell.Entity != null)
                Debug.Log(cell.Entity);
    }

    void ResolveMoves()
    {
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

            // Check if mover at front of list is moving to an obstacle... Maybe not needed because NPC/Player managers will check out of bounds moves

            // If the mover won't collide with anyone else on the board, they can legally move to their target move location
            if (!popped)
            {
                moversList[0].entity.transform.SetPositionAndRotation(moversList[0].movTar, Quaternion.identity);
                PlayerManager.Instance.playerLoc = moversList[0].movTar;
                popped = false;
                moversList.RemoveAt(0);
            }
        }

        PlayerManager.Instance.isTurn = true;
    }

    void ResolveAttacks()
    {

    }

    void WhoStillHasLimbs()
    {
        // Pop all the entities with <= 0 HP
        for (int i = 0; i < combatantList.Count; i++)
        {


            /*if (combatantList[i].entity.GetComponent<PlayerClass>() <= 0)
                combatantList.RemoveAt(i);*/
        }

        // Check for win conditions
        if (combatantList.Count == 0)
            Debug.Log("Lose");
        else if (combatantList.Count == 1 && combatantList[0].entity != player)
            Debug.Log("Lose");
        else if (combatantList.Count == 1 && combatantList[0].entity == player)
            Debug.Log("Win");
        else if (combatantList.Count == 2 && combatantList[0].entity != player)
            Debug.Log("Lose");
        else if (combatantList.Count == 2 && combatantList[0].entity == player && combatantList[1].entity == companion)
            Debug.Log("Win");
    }

    int FindInCombatantList(GameObject entity)
    {
        for (int i = 0; i < this.combatantList.Count; i++)
        {
            if (combatantList[i].entity == entity)
                return i;
        }

        Debug.AssertFormat(false, "Couldn't find in CombatantList");
        return -1;
    }

    GameObject GetEntity(Vector3 pos)
    {
        for (int i = 0; i < this.entitiesList.Count; i++)
        {
            if (entitiesList[i].transform.position == pos)
                return entitiesList[i];
        }

        return null;
    }

    void FillCombatantList()
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
