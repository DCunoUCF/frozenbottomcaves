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
    private GameManager gm;
    public List<CList> combatantList;
    private List<GameObject> entitiesList;
    public Cell[,] gridCell;
    private GameObject grid;
    public GameObject activeArena;
    private GameObject[] arenaDeactivate;
    private GameObject[] gridDeactivate;
    public GameObject player;
    private int playerX, playerY;
    private GameObject companion;
    private List<GameObject> enemies, enemyType;
    private Vector3 playerLoc;
    private Vector3 companionLoc;
    private GameObject[] enemiesLoc;
    private List<Vector3> availEnemyLoc;
    private List<Vector3> enemyLoc;
    private int numEnemies, numEnemyTypes;
    private bool isResolved;
    private bool didWeWin;

    private GameObject Entities; // parent to all entities spawned for cleanup

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

        this.gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        combatantList = new List<CList>();
        grid = GameObject.Find("ForestGrid"); // Overworld will set this
        arenaDeactivate = GameObject.FindGameObjectsWithTag("Tilemap");
        gridDeactivate = GameObject.FindGameObjectsWithTag("Grid");
        entitiesList = new List<GameObject>();
        availEnemyLoc = new List<Vector3>();
        isResolved = false;
        didWeWin = false;

        Entities = GameObject.Find("Entities");


        // Using number of enemies to be spawned to initiliaze their fields and finding random locations for them to spawn
        if (this.gm.om.dm.currentNode == 9)
        {
            activeArena = GameObject.Find("slime_arena"); // Overworld will set this
            numEnemies = 4;
            numEnemyTypes = 1;
            enemyLoc = new List<Vector3>(numEnemies);
            enemyType = new List<GameObject>(numEnemyTypes);
            enemies = new List<GameObject>(numEnemies);
            for (int i = 0; i < numEnemyTypes; i++)
            {
                enemyType.Add(GameObject.Find("slime_G1"));
            }
        }
        else if (this.gm.om.dm.currentNode == 17)
        {
            activeArena = GameObject.Find("goblin_arena"); // Overworld will set this
            numEnemies = 1;
            numEnemyTypes = 1;
            enemyLoc = new List<Vector3>(numEnemies);
            enemyType = new List<GameObject>(numEnemyTypes);
            enemies = new List<GameObject>(numEnemies);
            for (int i = 0; i < numEnemyTypes; i++)
            {
                enemyType.Add(GameObject.Find("goblin"));
            }
        }
        else if (this.gm.om.dm.currentNode == 21)
        {
            activeArena = GameObject.Find("goblin_arena"); // Overworld will set this
            numEnemies = 2;
            numEnemyTypes = 1;
            enemyLoc = new List<Vector3>(numEnemies);
            enemyType = new List<GameObject>(numEnemyTypes);
            enemies = new List<GameObject>(numEnemies);
            for (int i = 0; i < numEnemyTypes; i++)
            {
                enemyType.Add(GameObject.Find("goblin"));
            }
        }
        else if (this.gm.om.dm.currentNode == 26)
        {
            activeArena = GameObject.Find("goblin_arena"); // Overworld will set this
            numEnemies = 3;
            numEnemyTypes = 1;
            enemyLoc = new List<Vector3>(numEnemies);
            enemyType = new List<GameObject>(numEnemyTypes);
            enemies = new List<GameObject>(numEnemies);
            for (int i = 0; i < numEnemyTypes; i++)
            {
                enemyType.Add(GameObject.Find("goblin"));
            }
        }
        else
        {
            activeArena = GameObject.Find("goblin_arena"); // Overworld will set this
            numEnemies = 3;
            numEnemyTypes = 1;
            enemyLoc = new List<Vector3>(numEnemies);
            enemyType = new List<GameObject>(numEnemyTypes);
            enemies = new List<GameObject>(numEnemies);
            for (int i = 0; i < numEnemyTypes; i++)
            {
                enemyType.Add(GameObject.Find("goblin"));
            }
        }

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

        // Have to grab spawners after other arenas with spawners in them are deactivated
        playerLoc = GameObject.FindGameObjectWithTag("pSpawn").transform.position;
        companionLoc = GameObject.FindGameObjectWithTag("cSpawn").transform.position;
        enemiesLoc = GameObject.FindGameObjectsWithTag("eSpawn");

        // Instantiate Player and Companion
        player = GameObject.Instantiate(GameObject.Find(PlayerManager.Instance.characterName), playerLoc, Quaternion.identity);
        player.transform.SetParent(Entities.transform);
        //companion = GameObject.Instantiate(GameObject.Find("honey"), companionLoc, Quaternion.identity);
        //companion.transform.SetParent(Entities.transform);

        entitiesList.Add(player);
        //entitiesList.Add(companion);


        // Chooses random spawners for the enemy entities to spawn at        
        RandomEnemyPos();

        // Instantiate Enemies
        for (int i = 0; i < numEnemies; i++)
        {
            enemies.Add(GameObject.Instantiate(enemyType[0], enemyLoc[i], Quaternion.identity)); // Overworld will set the enemy types
            entitiesList.Add(enemies[i]);
            enemies[i].transform.SetParent(Entities.transform);
        }

        // Since gameobject is here, tell playerMan to initialize combat vars
        PlayerManager.Instance.initCombat();

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
        PlayerManager.Instance.isTurn = true;
    }

    void Update()
    {
        if (!PlayerManager.Instance.isTurn)
        {
            // NPCManager.Instance.Decide();
            ResolveMoves();
            ResolveAttacks();
            WhoStillHasLimbs();
        }
    }

    void CreateGrid()
    {
        int clidx, xDif, yDif, buffer = 5;
        Vector3 currentVector;
        GameObject tileEntity;
        string wall = "wall";
        Tilemap tilemap = activeArena.GetComponent<Tilemap>();
        Tilemap obstaclesMap = tilemap.transform.GetChild(0).GetComponent<Tilemap>();
        BoundsInt bounds = tilemap.cellBounds;

        this.gridCell = new Cell[bounds.size.x + buffer, bounds.size.y + buffer]; // Added a buffer for upper edges of board
        //Debug.Log("bounds.size.x: " + (bounds.size.x + buffer) + "bounds.size.y" + (bounds.size.y + buffer));

        foreach (var position in tilemap.cellBounds.allPositionsWithin)
        {
            xDif = position.x - bounds.position.x;
            yDif = position.y - bounds.position.y;

            // If we have made a Cell at this grid position already, skip this iteration
            if (this.gridCell[xDif, yDif] != null)
            {
                continue;
            }

            // If there's no tile here, make a "zero" Cell, then skip this iteration
            if (!tilemap.HasTile(position))
            {
                this.gridCell[xDif, yDif] = new Cell(false, null, new Vector3(0, 0, 0), xDif, yDif);
                continue;
            }

            // This x and y needs to be converted to the vector at the center of the tile to grab the GameObject entity from the tile
            currentVector = ConvertVector(position.x, position.y);
            tileEntity = GetEntity(currentVector);

            // If the tile is NOT an obstruction and there is no entity
            if (!obstaclesMap.HasTile(position) && tilemap.GetTile(position).name != wall && tileEntity == null)
            {
                this.gridCell[xDif, yDif] = new Cell(true, tileEntity, currentVector, xDif, yDif);
            }
            // If the tile is NOT an obstruction and there is an entity
            else if (!obstaclesMap.HasTile(position) && tilemap.GetTile(position).name != wall && tileEntity != null)
            {
                this.gridCell[xDif, yDif] = new Cell(true, tileEntity, currentVector, xDif, yDif);

                // Passing the combatantList the coordinates on the grid for the entity
                if (tileEntity != player)
                {
                    clidx = FindInCombatantList(tileEntity);
                    combatantList[clidx].gridX = xDif;
                    combatantList[clidx].gridY = yDif;
                }
                else
                {
                    playerX = xDif;
                    playerY = yDif;
                }
            }
            // If the tile IS an obstruction
            else // (obstaclesMap.HasTile(position) || tilemap.GetTile(position).name == "wall")
            {
                this.gridCell[xDif, yDif] = new Cell(false, tileEntity, currentVector, xDif, yDif);
            }
        }
        //int count = 0;
        //for (int i = 0; i < bounds.size.x; i++)
        //    for (int j = 0; j < bounds.size.y; j++)
        //    {
        //        if (this.gridCell[i, j].entity != null)
        //        {
        //            Debug.Log("(" + i + "," + j + ") is a " + this.gridCell[i,j].entity);
        //        }
        //    }
        //Debug.Log("count: " + count);
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
    }

    void ResolveAttacks()
    {
        CList curAtkTar;
        int atkX, atkY, atkTarIndex;

        // This is only complicated because attack target right now isn't just a relative position which would be easier to check on the gridCell
        for (int i = 0; i < combatantList.Count; i++)
        {
            if (combatantList[i].attack < 0)
                continue;

            atkTarIndex = FindInCombatantList(GetEntity(combatantList[i].atkTar));

            if (atkTarIndex < 0)
                continue;

            curAtkTar = combatantList[atkTarIndex];
            atkX = curAtkTar.gridX;
            atkY = curAtkTar.gridY;

            if (gridCell[atkX, atkY].entity == null)
                continue;

            Debug.Log("Attacker:" + combatantList[i].entity + " Target:" + combatantList[atkTarIndex].entity + " HP was:" + combatantList[atkTarIndex].hp + " HP is now:" + (combatantList[atkTarIndex].hp - combatantList[i].attackDmg));
            // Roll Dice / Incorporate entity stats
            combatantList[atkTarIndex].hp -= combatantList[i].attackDmg;

            if (combatantList[atkTarIndex].entity == player)
                PlayerManager.Instance.pc.health -= combatantList[i].attackDmg;
        }
    }

    public bool isBattleResolved()
    {
        return this.isResolved;
    }

    public bool didWeWinTheBattle()
    {
        return this.didWeWin;
    }

    void WhoStillHasLimbs()
    {
        // Pop all the entities with <= 0 HP
        for (int i = 0; i < combatantList.Count; i++)
        {
            if (combatantList[i].hp <= 0)
            {
                combatantList[i].entity.SetActive(false);
                combatantList.RemoveAt(i);
            }
        }

        if (this.isResolved)
            return;

        if (combatantList.Count == 0 || combatantList[0].entity != player)
        {
            this.didWeWin = false;
            this.isResolved = true;
            Debug.Log("Lose");
        }

        if (combatantList.Count == 1 && combatantList[0].entity == player)
        {
            this.didWeWin = true;
            this.isResolved = true;
            this.CleanScene();
            Debug.Log("Win");
        }
        else if (combatantList.Count == 2 && combatantList[0].entity == player && combatantList[1].entity == companion)
        {
            this.didWeWin = true;
            this.isResolved = true;
            this.CleanScene();
            Debug.Log("Win");
        }

        // Tell PlayerManager it's now the player's turn... do it differently sometime maybe?
        PlayerManager.Instance.isTurn = true;
    }

    int FindInCombatantList(GameObject entity)
    {
        for (int i = 0; i < this.combatantList.Count; i++)
        {
            if (combatantList[i] != null && combatantList[i].entity == entity)
                return i;
        }

        Debug.AssertFormat(false, "Couldn't find in CombatantList");
        return -1;
    }


    public void CleanScene()
    {
        //Destroy(sceneCleaner);
        //foreach (CList entity in this.combatantList)
        //{
        //    Destroy(entity.entity);
        //}

        //foreach (CList entity in this.trashList)
        //{
        //    Destroy(entity.entity);
        //}
    }

    void MoveOnGrid(CList entity)
    {
        // How I WILL do it later entity.dir... maybe?
        float dirX, dirY;
        BoundsInt bounds = activeArena.GetComponent<Tilemap>().cellBounds;
        int xPlus, xMinus, yPlus, yMinus;
        dirX = entity.movTar.x - entity.entity.transform.localPosition.x;
        dirY = entity.movTar.y - entity.entity.transform.localPosition.y;
        GameObject sprite = entity.entity;
        GameObject SE = sprite.transform.GetChild(0).gameObject, SW = sprite.transform.GetChild(1).gameObject,
                   NW = sprite.transform.GetChild(2).gameObject, NE = sprite.transform.GetChild(3).gameObject;

        //Debug.Log("dirX: " + dirX + " dirY: " + dirY);
        //Debug.Log("moving from (" + entity.gridX + "," + entity.gridY + ")");

        xPlus = (entity.gridX + 1) % bounds.size.x;
        yPlus = (entity.gridY + 1) % bounds.size.y;
        xMinus = Mathf.Abs(entity.gridX - 1) % bounds.size.x;
        yMinus = Mathf.Abs(entity.gridY - 1) % bounds.size.y;

        if (dirX > 0)
        {
            if (dirY > 0)
            {
                //Debug.Log("moving to (" + xPlus + "," + entity.gridY + ")");
                gridCell[entity.gridX, entity.gridY].entity = null;
                gridCell[xPlus, entity.gridY].entity = entity.entity;
                entity.gridX = xPlus;
                SE.gameObject.SetActive(false);
                SW.gameObject.SetActive(false);
                NW.gameObject.SetActive(false);
                NE.gameObject.SetActive(true);
            }
            else
            {
                //Debug.Log("moving to (" + entity.gridX + "," + yMinus + ")");
                gridCell[entity.gridX, entity.gridY].entity = null;
                gridCell[entity.gridX, yMinus].entity = entity.entity;
                entity.gridY = yMinus;
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
                //Debug.Log("moving to (" + xMinus + "," + entity.gridY + ")");
                gridCell[entity.gridX, entity.gridY].entity = null;
                gridCell[xMinus, entity.gridY].entity = entity.entity;
                entity.gridX = xMinus;
                SE.gameObject.SetActive(false);
                SW.gameObject.SetActive(true);
                NW.gameObject.SetActive(false);
                NE.gameObject.SetActive(false);
            }
            else
            {
                //Debug.Log("moving to (" + entity.gridX + "," + yPlus + ")");
                gridCell[entity.gridX, entity.gridY].entity = null;
                gridCell[entity.gridX, yPlus].entity = entity.entity;
                entity.gridY = yPlus;
                SE.gameObject.SetActive(false);
                SW.gameObject.SetActive(false);
                NW.gameObject.SetActive(true);
                NE.gameObject.SetActive(false);
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
        Debug.Log(combatantList[0].entity);
        //combatantList.Add(new CList(companion));

        for (int i = 0; i < numEnemies; i++)
        {
            combatantList.Add(new CList(enemies[i]));
        }
    }

    Vector3 ConvertVector(int x, int y)
    {
        return new Vector3((x * 0.5f) - (y * 0.5f), ((x + 1) * 0.25f) + (y * 0.25f), 0);
    }

    void PrintCList(CList c)
    {
        Debug.Log("entity: " + c.entity);
        Debug.Log("gridX: " + c.gridX);
        Debug.Log("gridY: " + c.gridY);
        Debug.Log("move: " + c.move);
        Debug.Log("movTar: " + c.movTar);
        Debug.Log("atkTar: " + c.atkTar);
        Debug.Log("dir: " + c.dir);
        Debug.Log("attack: " + c.attack);
        Debug.Log("attackDmg: " + c.attackDmg);
        Debug.Log("hp: " + c.hp);
    }

    void RandomEnemyPos()
    {
        int random;

        foreach (GameObject i in enemiesLoc)
        {
            availEnemyLoc.Add(i.transform.position);
        }

        for (int i = 0; i < numEnemies; i++)
        {
            random = (int)Random.Range(0, availEnemyLoc.Count);
            enemyLoc.Add(availEnemyLoc[random]);
            availEnemyLoc.RemoveAt(random);
        }
    }
}