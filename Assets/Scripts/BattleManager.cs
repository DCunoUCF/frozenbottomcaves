using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Biome : short
{
	NOTHING = -1, FOREST, CAVE, ICECAVE, CASTLE, BOSS
};

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
        activeArena = GameObject.Find("Arena4"); // Overworld will set this
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

        // Chooses random spawners for the enemy entities to spawn at        
        RandomEnemyPos();

        // Instantiate Enemies
        for (int i = 0; i < numEnemies; i++)
        {
            enemies[i] = GameObject.Instantiate(GameObject.Find("goblin2"), enemyLoc[i], Quaternion.identity); // Overworld will set the enemy types
            entitiesList.Add(enemies[i]);
        }

        // Fill CombatantList with entities that were just instantiated
        FillCombatantList();

        // Creating the Grid
        CreateGrid();
    }

    private void Start()
    {
        PlayerManager.Instance.x = playerX;
        PlayerManager.Instance.y = playerY;
        combatantList[0] = PlayerManager.Instance.combatInfo;
        combatantList[0].gridX = playerX;
        combatantList[0].gridY = playerY;
        Debug.Log("playerX: " + playerX + " playerY: " + playerY);
        PlayerManager.Instance.isTurn = true;
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
        int clidx; //x = 0, y = 0;
        Vector3 currentVector;
        //Vector3Int position;
        GameObject tileEntity;
        Tilemap tilemap = activeArena.GetComponent<Tilemap>();
        BoundsInt bounds = tilemap.cellBounds;

        this.gridCell = new Cell[bounds.size.x, bounds.size.y];

        /*this.gridCell = new Cell[(Mathf.Abs(bounds.xMin) + bounds.xMax + 1) * 2, (Mathf.Abs(bounds.yMin) + bounds.yMax + 1) * 4];
        Debug.Log("bounds.xMax: " + bounds.xMax + "bounds.yMax: " + bounds.yMax);
        Debug.Log("bounds.xMin: " + bounds.xMin + "bounds.yMin: " + bounds.yMin);*/

        /*for (float i = bounds.xMin + 0.5f; i < bounds.xMax; i += 0.5f)
        {
            for (float j = bounds.yMin + 0.25f; j < bounds.yMax; j += 0.25f)
            {
                Debug.Log("x: " + x + " y: " + y);
                position = new Vector3Int((int)i, (int)j, 0);
                // If there's no tile here, skip this iteration
                if (!tilemap.HasTile(position))
                {
                    if (y > Mathf.Abs(bounds.yMin) + bounds.yMax)
                        y = 0;
                    else
                        y++;

                    continue;
                }
                else
                {
                    // This x and y needs to be converted to the vector at the center of the tile to grab the GameObject entity from the tile
                    //currentVector = ConvertVector(i, j);
                    currentVector = new Vector3(i, j, 0);
                    tileEntity = GetEntity(currentVector);

                    // If the tile is NOT an obstruction and there is no entity
                    if (tilemap.GetTile(position).name != "isoWall1" && tileEntity == null)
                    {
                        this.gridCell[x, y] = new Cell(true, tileEntity, currentVector, x, y);
                    }
                    // If the tile is NOT an obstruction and there is an entity
                    else if (tilemap.GetTile(position).name != "isoWall1" && tileEntity != null)
                    {
                        this.gridCell[x, y] = new Cell(true, tileEntity, currentVector, x, y);

                        // Passing the combatantList the coordinates on the grid for the entity
                        if (tileEntity != player)
                        {
                            clidx = FindInCombatantList(tileEntity);
                            combatantList[clidx].gridX = x;
                            combatantList[clidx].gridY = y;
                        }
                        else
                        {
                            playerX = x;
                            playerY = y;
                        }
                    }
                    // If the tile IS an obstruction
                    else if (tilemap.GetTile(position).name == "isoWall1")
                    {
                        this.gridCell[x, y] = new Cell(false, tileEntity, currentVector, x, y);
                    }
                    else
                    {
                        this.gridCell[x, y] = new Cell(false, null, new Vector3(0, 0, 0), x, y);
                    }

                    if (y > Mathf.Abs(bounds.yMin) + bounds.yMax)
                        y = 0;
                    else
                        y++;
                }
            }
            x++;
        }*/

        /*        int count = 0;
                foreach (Cell cell in gridCell)
                    if (cell != null)
                    {
                        Debug.Log("Hi I'm jelly and I'm at x: " + cell.x + " Hello again, I'm jelly and I'm at y: " + cell.y);
                        count++;
                    }
                Debug.Log(count);*/

        foreach (var position in tilemap.cellBounds.allPositionsWithin)
        {
            // If there's no tile here, skip this iteration
            if (!tilemap.HasTile(position))
            {
                this.gridCell[position.x - bounds.position.x, position.y - bounds.position.y] = new Cell(false, null, new Vector3(0, 0, 0), position.x - bounds.position.x, position.y - bounds.position.y);
                continue;
            }

            // This x and y needs to be converted to the vector at the center of the tile to grab the GameObject entity from the tile
            currentVector = ConvertVector(position.x, position.y);
            tileEntity = GetEntity(currentVector);

            // If the tile is NOT an obstruction and there is no entity
            if (tilemap.GetTile(position).name != "isoWall1" && tileEntity == null)
            {
                this.gridCell[position.x - bounds.position.x, position.y - bounds.position.y] = new Cell(true, tileEntity, currentVector, position.x - bounds.position.x, position.y - bounds.position.y);
            }
            // If the tile is NOT an obstruction and there is an entity
            else if (tilemap.GetTile(position).name != "isoWall1" && tileEntity != null)
            {
                this.gridCell[position.x - bounds.position.x, position.y - bounds.position.y] = new Cell(true, tileEntity, currentVector, position.x - bounds.position.x, position.y - bounds.position.y);

                // Passing the combatantList the coordinates on the grid for the entity
                if (tileEntity != player)
                {
                    clidx = FindInCombatantList(tileEntity);
                    combatantList[clidx].gridX = position.x - bounds.position.x;
                    combatantList[clidx].gridY = position.y - bounds.position.y;
                }
                else
                {
                    playerX = position.x - bounds.position.x;
                    playerY = position.y - bounds.position.y;
                }
            }
            // If the tile IS an obstruction
            else if (tilemap.GetTile(position).name == "isoWall1")
            {
                this.gridCell[position.x - bounds.position.x, position.y - bounds.position.y] = new Cell(false, tileEntity, currentVector, position.x - bounds.position.x, position.y - bounds.position.y);
            }
            else
            {
                this.gridCell[position.x - bounds.position.x, position.y - bounds.position.y] = new Cell(false, null, new Vector3(0, 0, 0), position.x - bounds.position.x, position.y - bounds.position.y);
            }

/*            int count = 0;
            foreach (Cell cell in gridCell)
                if (cell != null)
                {
                    Debug.Log("Hi I'm jelly and I'm at x: " + cell.x + " Hello again, I'm jelly and I'm at y: " + cell.y);
                    count++;
                }
            Debug.Log(count);*/
        }
    }

    void ResolveMoves()
    {
        bool popped = false;

        for (int i = 0; i < combatantList.Count; i++)
        {
            for (int j = 0; j < combatantList.Count; j++)
            {
                if (combatantList[i].entity == combatantList[j].entity || !combatantList[i].move)
                    continue;

                if (combatantList[j].move && combatantList[i].movTar == combatantList[j].movTar)
                {
                    popped = true;
                    break;
                }

                if (combatantList[j].attack > -1 && combatantList[i].movTar == combatantList[j].entity.transform.localPosition)
                {
                    popped = true;
                    break;
                }
            }

            // If the mover won't collide with anyone else on the board, they can legally move to their target move location
            if (!popped && combatantList[i].move)
            {
                MoveOnGrid(combatantList[i]);
                combatantList[i].entity.transform.SetPositionAndRotation(combatantList[i].movTar, Quaternion.identity);
                PlayerManager.Instance.moved = true;

                if (combatantList[i].entity == player)
                    PlayerManager.Instance.playerLoc = combatantList[i].movTar;

                combatantList[i].move = false;
                popped = false;
            }
        }

        // Tell PlayerManager it's now the player's turn... do it differently sometime maybe?
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
            if (combatantList[i].hp <= 0)
                combatantList.RemoveAt(i);
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
        for (int i = 1; i < this.combatantList.Count; i++) // SKIP PLAYER FOR NOW BECAUSE AWAKE METHOD ORDER AND PLAYERCLASS ORDER
        {
            if (combatantList[i].entity == entity)
                return i;
        }

        Debug.AssertFormat(false, "Couldn't find in CombatantList"); // SKIPPING PLAYER IN LOOP ABOVE MAY CAUSE THIS
        return -1;
    }

    void MoveOnGrid(CList entity)
    {
        // How I WILL do it later entity.dir... maybe?
        float dirX, dirY;
        dirX = entity.movTar.x - entity.entity.transform.localPosition.x;
        dirY = entity.movTar.y - entity.entity.transform.localPosition.y;

        //Debug.Log("dirX: " + dirX + " dirY: " + dirY);
        //Debug.Log("entity.movTar.x: " + entity.movTar.x + " entity.entity.transform.localPosition.x: " + entity.entity.transform.localPosition.x);
        //Debug.Log("entity.movTar.y: " + entity.movTar.y + " entity.entity.transform.localPosition.y: " + entity.entity.transform.localPosition.y);
        //Debug.Log("entity.gridX: " + entity.gridX + " entity.gridY: " + entity.gridY);
        //Debug.Log("gridCell[36, 33]: " + gridCell[37, 31]);

        if (dirX > 0)
        {
            if (dirY > 0)
            {
                gridCell[entity.gridX, entity.gridY].entity = null;
                gridCell[++entity.gridX, ++entity.gridY].entity = entity.entity;
            }
            else
            {
                gridCell[entity.gridX, entity.gridY].entity = null;
                gridCell[++entity.gridX, --entity.gridY].entity = entity.entity;
            }

        }
        else if (dirX < 0)
        {
            if (dirY < 0)
            {
                gridCell[entity.gridX, entity.gridY].entity = null;
                gridCell[--entity.gridX, --entity.gridY].entity = entity.entity;
            }
            else
            {
                gridCell[entity.gridX, entity.gridY].entity = null;
                gridCell[--entity.gridX, ++entity.gridY].entity = entity.entity;
            }
        }
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

    Vector3 ConvertVector(int x, int y)
    {
        return new Vector3((x * 0.5f) - (y * 0.5f), ((x + 1) * 0.25f) + (y * 0.25f), 0);
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
