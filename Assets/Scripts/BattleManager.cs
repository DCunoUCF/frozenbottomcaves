using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; set; }
    private GameManager gm;
    public List<CList> combatantList;
    private BattleClass battleClass;
    private int curNode;
    public Cell[,] gridCell;
    private GameObject grid;
    private GameObject activeArena;
    public int gridsizeX, gridsizeY; // Maintain size of board to reduce repeated computation in PM

    // Player and Companion GameObject references. playerX/Y are stand-ins because of the ordering of script execution
    public GameObject player;
    private int playerX, playerY;
    private GameObject companion;

    // Enemy Variables
    private List<GameObject> enemies;
    private int numEnemies;
    private List<Vector3> chosenEnemyLocList;

    // Battle Conclusion Booleans
    private bool isResolved, didWeWin;

    // Pegi added this garbage
    private bool resolvingTurn;
    public float slideSpeed = .5f;

    // Parent to all entities spawned. Used for cleanup after battle is resolved
    private GameObject Entities; 

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
        this.combatantList = new List<CList>();
        this.curNode = this.gm.om.dm.currentNode;
        this.battleClass = this.gm.om.GetBattleClass();
        this.grid = GameObject.Find(this.battleClass.grid.ToString());
        this.activeArena = GameObject.Find(this.battleClass.arena);
        this.numEnemies = this.battleClass.nodeEnemies.Count;
        this.isResolved = false;
        this.didWeWin = false;
        this.Entities = GameObject.Find("Entities");

        // Deactivating all Grids and Arenas not needed
        this.SetupArena();

        this.InstantiateEntities();

        // Since gameobject is here, tell playerMan to initialize combat vars
        this.gm.pm.initCombat();

        // Fill CombatantList with entities that were just instantiated
        this.FillCombatantList();

        // Creating the Grid
        this.CreateGrid();
    }

    private void Start()
    {
        combatantList[0] = this.gm.pm.combatInfo;
        combatantList[0].gridX = playerX;
        combatantList[0].gridY = playerY;
        this.gm.pm.x = playerX;
        this.gm.pm.y = playerY;
        this.gm.pm.isTurn = true;
    }

    void Update()
    {
        if (!this.gm.pm.isTurn && !resolvingTurn) // resolvingTurn guards the coroutine from being called multiple times
        {
            resolvingTurn = true;
            // NPCManager.Instance.Decide();
            StartCoroutine(combatUpdate());  // Added this to have the ability to resolve each step with animations if wanted
            // ResolveMoves();
            //ResolveAttacks();
            //WhoStillHasLimbs();
        }
    }

    IEnumerator combatUpdate()
    {
        // Get NPC decisions
        yield return StartCoroutine(ResolveMoves()); // Allow this coroutine time to finishing sliding ppl around that are moving
        ResolveAttacks();
        WhoStillHasLimbs();
        resolvingTurn = false; // Done with the steps of resolving a turn, flip the flag back
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

        foreach (var position in tilemap.cellBounds.allPositionsWithin)
        {
            xDif = position.x - bounds.position.x;
            yDif = position.y - bounds.position.y;

            // If we have made a Cell at this grid position already, skip this iteration
            if (this.gridCell[xDif, yDif] != null)
                continue;

            // If there's no tile here, make a "zero" Cell, then skip this iteration
            if (!tilemap.HasTile(position))
            {
                this.gridCell[xDif, yDif] = new Cell(false, null, new Vector3(0, 0, 0), xDif, yDif);
                continue;
            }

            // This x and y needs to be converted to the vector at the center of the tile to grab the GameObject entity from the tile
            currentVector = ConvertVector(position.x, position.y);
            tileEntity = GetCombatant(currentVector);

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
                    clidx = GetIndexOfCombatant(tileEntity);
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
    }

    IEnumerator ResolveMoves() // Turned into a coroutine to allow the ability to wait until an entity is done sliding
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
                yield return StartCoroutine(slideEntity(combatantList[i])); // Slides the entity to it's movTar, then does the stuff commented out below
                popped = false;
            }
        }
        yield break;
    }

    IEnumerator slideEntity(CList entity)
    {
        // This while loop is called each frame to slide the entity to it's destination
        while (entity.entity.transform.position != entity.movTar)
        {
            entity.entity.transform.position = Vector3.MoveTowards(entity.entity.transform.position, entity.movTar, slideSpeed * Time.deltaTime);
            yield return null;
        }

        // If the entity was the player, update the pm accordingly
        if (entity.entity == player)
        {
            this.gm.pm.playerLoc = entity.movTar;
            this.gm.pm.moved = true;
        }
        // Flip the move bool back to it's default state
        entity.move = false;
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

            atkTarIndex = GetIndexOfCombatant(GetCombatant(combatantList[i].atkTar));

            if (atkTarIndex < 0)
                continue;

            curAtkTar = combatantList[atkTarIndex];
            atkX = curAtkTar.gridX;
            atkY = curAtkTar.gridY;

            if (gridCell[atkX, atkY].entity == null)
                continue;

            // Roll Dice / Incorporate entity stats
            combatantList[atkTarIndex].hp -= combatantList[i].attackDmg;

            if (combatantList[atkTarIndex].entity == player)
                this.gm.pm.takeDmg(combatantList[i].attackDmg); // changed to use new dmg method
           
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
        this.gm.pm.isTurn = true;
    }

    int GetIndexOfCombatant(GameObject entity)
    {
        for (int i = 0; i < this.combatantList.Count; i++)
        {
            if (combatantList[i] != null && combatantList[i].entity == entity)
                return i;
        }

        Debug.AssertFormat(false, "Couldn't find " + entity + " in CombatantList");
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

        xPlus = (entity.gridX + 1) % bounds.size.x;
        yPlus = (entity.gridY + 1) % bounds.size.y;
        xMinus = Mathf.Abs(entity.gridX - 1) % bounds.size.x;
        yMinus = Mathf.Abs(entity.gridY - 1) % bounds.size.y;

        if (dirX > 0)
        {
            if (dirY > 0)
            {
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


    GameObject GetCombatant(Vector3 pos)
    {
        for (int i = 0; i < this.combatantList.Count; i++)
        {
            if (combatantList[i].entity.transform.position == pos)
                return combatantList[i].entity;
        }

        Debug.AssertFormat(false, "Could not find Combatant in combatantList at Vector3: " + pos);
        return null;
    }

    void FillCombatantList()
    {
        // Add player to combatantList
        combatantList.Add(this.gm.pm.combatInfo);

        // If the player has a companion, add the companion to the combatantList
        if (companion != null)
            combatantList.Add(new CList(companion));

        // For the number of enemies requested to be spawned, add them to the compatantList
        for (int i = 0; i < this.numEnemies; i++)
        {
            combatantList.Add(new CList(enemies[i]));
        }
    }

    Vector3 ConvertVector(int x, int y)
    {
        return new Vector3((x * 0.5f) - (y * 0.5f), ((x + 1) * 0.25f) + (y * 0.25f), 0);
    }

    private void SetupArena()
    {
        GameObject[] arenaDeactivate = GameObject.FindGameObjectsWithTag("Tilemap");
        GameObject[] gridDeactivate = GameObject.FindGameObjectsWithTag("Grid");
        grid = GameObject.Find(this.battleClass.grid.ToString());
        activeArena = GameObject.Find(this.battleClass.arena);

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
    }

    private void InstantiateEntities()
    {
        // Have to grab spawners after other arenas with spawners in them are deactivated
        Vector3 playerSpawnerLoc = GameObject.FindGameObjectWithTag("pSpawn").transform.position;
        Vector3 companionSpawnerLoc = GameObject.FindGameObjectWithTag("cSpawn").transform.position;

        // Instantiate Player and Companion
        this.player = GameObject.Instantiate(GameObject.Find(this.gm.pm.characterName), playerSpawnerLoc, Quaternion.identity);
        this.player.transform.SetParent(Entities.transform);
        //this.companion = GameObject.Instantiate(GameObject.Find("honey"), companionSpawnerLoc, Quaternion.identity);
        //this.companion.transform.SetParent(Entities.transform);

        // Chooses random spawners for the enemy entities to spawn at        
        RandomEnemyPos();

        // Instantiate Enemies
        for (int i = 0; i < this.numEnemies; i++)
        {
            this.enemies.Add(GameObject.Instantiate(GameObject.Find(this.battleClass.nodeEnemies[i].ToString()), chosenEnemyLocList[i], Quaternion.identity)); // Overworld will set the enemy types
            this.enemies[i].transform.SetParent(Entities.transform);
        }
    }

    void RandomEnemyPos()
    {
        int random;
        List<Vector3> availEnemySpawnerLocs = new List<Vector3>();
        GameObject[] enemiesSpawnerLocs = GameObject.FindGameObjectsWithTag("eSpawn");
        this.enemies = new List<GameObject>();
        this.chosenEnemyLocList = new List<Vector3>(this.numEnemies);

        foreach (GameObject i in enemiesSpawnerLocs)
        {
            availEnemySpawnerLocs.Add(i.transform.position);
        }

        for (int i = 0; i < this.numEnemies; i++)
        {
            random = (int)Random.Range(0, availEnemySpawnerLocs.Count);
            chosenEnemyLocList.Add(availEnemySpawnerLocs[random]);
            availEnemySpawnerLocs.RemoveAt(random);
        }
    }
}