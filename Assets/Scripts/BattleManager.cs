using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; set; }
    private GameManager gm;
    private NPCManager npcm;
    public List<CList> combatantList;
    private BattleClass battleClass;
    private int curNode;
    public Cell[,] gridCell;
    private GameObject grid;
    private GameObject activeArena;
    public int gridsizeX, gridsizeY; // Maintain size of board to reduce repeated computation in PM

    // Player GameObject references. playerX/Y are stand-ins because of the ordering of script execution
    public GameObject player;
    private int playerX, playerY;

    // Enemy Variables
    private List<GameObject> enemies;
    private int numEnemies;
    private List<Vector3> chosenEnemyLocList;

    // Battle Conclusion Booleans
    private bool isResolved, didWeWin;

    // Pegi added this garbage
    private bool resolvingTurn;
    public float slideSpeed = 1.5f;
    public float attackSpeed = 4.0f;

    // Parent to all entities spawned. Used for cleanup after battle is resolved
    private GameObject Entities;

    public GameObject rollParchment;
    public RollMaster rollScript;
    public DiceRoller dr1, dr2;

    void Awake()
    {
        //if (Instance == null)
        //{
        //    Instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}
        Instance = this;
        this.gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        this.gm.setBM(this);
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
        combatantList[0].hp = this.gm.pm.pc.getHealth();
        this.gm.pm.x = playerX;
        this.gm.pm.y = playerY;
        this.gm.pm.isTurn = true;
        PlayerManager.Instance.isTurn = true;
        this.npcm = new NPCManager(this);
        printGrid();

        this.rollParchment = this.gm.om.rollParchment;
        this.rollScript = this.gm.om.rollScript;
        this.dr1 = this.gm.om.dr1;
        this.dr2 = this.gm.om.dr2;
        this.rollParchment.SetActive(false);
    }

    void Update()
    {
        if (!this.gm.pm.isTurn && !resolvingTurn) // resolvingTurn guards the coroutine from being called multiple times
        {
            foreach (CList c in combatantList)
                print(c.gridX);
            resolvingTurn = true;
            StartCoroutine(combatUpdate());  // Added this to have the ability to resolve each step with animations if wanted

        }
    }

    IEnumerator combatUpdate()
    {
        // Get NPC decisions
        npcm.makeDecisions();
        yield return StartCoroutine(ResolveMoves()); // Allow this coroutine time to finishing sliding ppl around that are moving
        yield return StartCoroutine(ResolveAttacks()); // quick jab .25 move
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

            if (xDif == 0 && yDif == 0)
            {
                Debug.Log("&&&& -> gridCell[0, 0] = ("+position.x+", "+position.y+", "+position.z+")");
            }
            if (xDif == 0 && yDif == 1)
            {
                Debug.Log("&&&& -> gridCell[0, 1] = ("+position.x+", "+position.y+", "+position.z+")");
            }
            if (xDif == 1 && yDif == 0)
            {
                Debug.Log("&&&& -> gridCell[1, 0] = ("+position.x+", "+position.y+", "+position.z+")");
            }
            if (xDif == 1 && yDif == 1)
            {
                Debug.Log("&&&& -> gridCell[0, 0] = ("+position.x+", "+position.y+", "+position.z+")");
            }

            // If we have made a Cell at this grid position already, skip this iteration
            if (this.gridCell[xDif, yDif] != null)
                continue;


            // This used to be after we add "zero" cells, moved here to test where these zero cells really are
            currentVector = ConvertVector(position.x, position.y);

            // If there's no tile here, make a "zero" Cell, then skip this iteration
            if (!tilemap.HasTile(position))
            {
                this.gridCell[xDif, yDif] = new Cell(false, null, currentVector, xDif, yDif);
                continue;
            }

            // This x and y needs to be converted to the vector at the center of the tile to grab the GameObject entity from the tile
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
        List<Vector3> freedSpots = new List<Vector3>();
        List<CList> leftoverMovers = new List<CList>();
        List<CList> collidedAlready = new List<CList>();

        for (int i = 0; i < combatantList.Count; i++)
        {
            for (int j = 0; j < combatantList.Count; j++)
            {
                if (combatantList[i].entity == combatantList[j].entity || !combatantList[i].move)
                    continue;

                // both go halfway
                if (combatantList[j].move && combatantList[i].movTar == combatantList[j].movTar)
                {
                    if (!collidedAlready.Contains(combatantList[j]) || !collidedAlready.Contains(combatantList[i]))
                    {
                        collidedAlready.Add(combatantList[i]);
                        collidedAlready.Add(combatantList[j]);
                        yield return StartCoroutine(slideBothCollide(combatantList[i], combatantList[j]));
                    }
                    popped = true;
                    break;
                }

                // i goes halfway
                if (combatantList[j].attack > -1 && combatantList[i].movTar == combatantList[j].entity.transform.localPosition)
                {
                    yield return StartCoroutine(slideSingleCollide(combatantList[i]));
                    popped = true;
                    break;
                }
            }

            // If the mover won't collide with anyone else on the board, they can legally move to their target move location
            if (!popped && combatantList[i].move)
            {
                if (GetCombatant(combatantList[i].movTar) == null && checkFreeSpots(freedSpots, combatantList[i].movTar))
                {
                    if (isPassable(combatantList[i].movTar))
                    {
                        freedSpots.Add(combatantList[i].entity.transform.position);
                        MoveOnGrid(combatantList[i]);
                        yield return StartCoroutine(slideEntity(combatantList[i])); // Slides the entity to it's movTar, then does the stuff commented out below
                    }
                    else
                    {
                        yield return StartCoroutine(slideSingleCollide(combatantList[i]));
                    }
                }
                else
                {
                    leftoverMovers.Add(combatantList[i]);
                }
                popped = false;
            }
        }
        foreach (CList c in leftoverMovers)
        {
            if (isPassable(c.movTar) && GetCombatant(c.movTar) == null)
            {
                MoveOnGrid(c);
                yield return StartCoroutine(slideEntity(c));
            }
            else
            {
                yield return StartCoroutine(slideSingleCollide(c));

            }
        }

        yield break;
    }

    private bool checkFreeSpots(List<Vector3> spots, Vector3 check)
    {
        foreach (Vector3 v in spots)
        {
            if (check == v)
                return false;
        }
        return true;
    }

    IEnumerator slideBothCollide(CList entity, CList entity2)
    {
        Vector3 start = entity.entity.transform.position;
        Vector3 end = entity.movTar;
        Vector3 start2 = entity2.entity.transform.position;
        Vector3 end2 = entity.movTar;
        Vector3 halfway = (start + end) / 2;
        Vector3 halfway2 = (start2 + end2) / 2;

        turnEntity(entity.entity, entity.movTar);
        turnEntity(entity2.entity, entity2.movTar);

        while (entity.entity.transform.position != halfway && entity2.entity.transform.position != halfway2)
        {
            if (entity.entity.transform.position != halfway)
                entity.entity.transform.position = Vector3.MoveTowards(entity.entity.transform.position, halfway, slideSpeed * Time.deltaTime);
            if (entity2.entity.transform.position != halfway2)
                entity2.entity.transform.position = Vector3.MoveTowards(entity2.entity.transform.position, halfway2, slideSpeed * Time.deltaTime);

            yield return null;
        }

        while (entity.entity.transform.position != start && entity2.entity.transform.position != start2)
        {
            if (entity.entity.transform.position != start)
                entity.entity.transform.position = Vector3.MoveTowards(entity.entity.transform.position, start, slideSpeed * Time.deltaTime);
            if (entity2.entity.transform.position != start2)
                entity2.entity.transform.position = Vector3.MoveTowards(entity2.entity.transform.position, start2, slideSpeed * Time.deltaTime);

            yield return null;
        }
        // In case something happens, framerate dip or something that leads to them being off, force them to be in their appropriate spots
        entity.entity.transform.position = start;
        entity2.entity.transform.position = start2;
        yield break;
    }

    IEnumerator slideSingleCollide(CList entity)
    {
        Vector3 start = entity.entity.transform.position;
        Vector3 end = entity.movTar;
        Vector3 halfway = (start + end) / 2;

        turnEntity(entity.entity, entity.movTar);

        while (entity.entity.transform.position != halfway)
        {
            entity.entity.transform.position = Vector3.MoveTowards(entity.entity.transform.position, halfway, slideSpeed * Time.deltaTime);
            yield return null;
        }
        while (entity.entity.transform.position != start)
        {
            entity.entity.transform.position = Vector3.MoveTowards(entity.entity.transform.position, start, slideSpeed * Time.deltaTime);
            yield return null;
        }

        entity.entity.transform.position = start;
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

    IEnumerator ResolveAttacks()
    {
        CList curAtkTar, clashCList = null, playerCList = combatantList[0];
        GameObject tempEntity;
        bool clash = false;
        int atkX, atkY, atkTarIndex, tempIndex = -1;

        if (playerCList.atkTar != null)
        {
            foreach (Vector3 v in playerCList.atkTar)
            {
                tempEntity = GetCombatant(v);
                if (tempEntity != null)
                {
                    tempIndex = GetIndexOfCombatant(tempEntity);
                    clashCList = combatantList[tempIndex];
                    if (clashCList.atkTar != null)
                    {
                        foreach (Vector3 v2 in clashCList.atkTar)
                        {
                            if (v2 == playerCList.entity.transform.position)
                            {
                                clash = true;
                                yield return StartCoroutine(ClashAnim(playerCList, clashCList));
                                break;
                            }
                        }
                    }
                }
                if (clash)
                    break;
            }
        }

        // This is only complicated because attack target right now isn't just a relative position which would be easier to check on the gridCell
        for (int i = 0; i < combatantList.Count; i++)
        {
            //Debug.Log("attack: " + combatantList[i].attack);
            if (combatantList[i].attack < 0)
                continue;
            if (combatantList[i].hp <= 0)
                continue;
            if (clash && i == 0)
                continue;
            if (clash && i == tempIndex)
                continue;

            yield return StartCoroutine(attackAnim(combatantList[i]));
            for (int j = 0; j < combatantList[i].atkTar.Count; j++)
            {

                atkTarIndex = GetIndexOfCombatant(GetCombatant(combatantList[i].atkTar[j]));

                if (atkTarIndex < 0)
                    continue;

                curAtkTar = combatantList[atkTarIndex];
                atkX = curAtkTar.gridX;
                atkY = curAtkTar.gridY;

                if (gridCell[atkX, atkY].entity == null)
                    continue;

                print("combatant: " + combatantList[atkTarIndex].entity + "combatant hp before attack:" + combatantList[atkTarIndex].hp);

                // If the attacker is the player or if the attacker is the enemy and the target is the player 
                if (i == 0 || (i != 0 && atkTarIndex == 0))
                    combatantList[atkTarIndex].hp -= combatantList[i].attackDmg;

                print("enemy: " + combatantList[i].entity + " enemy damage: " + combatantList[i].attackDmg + " combatant hp after attack: " + combatantList[atkTarIndex].hp);
                
                if (combatantList[atkTarIndex].hp <= 0)
                {
                    combatantList[atkTarIndex].entity.SetActive(false);
                }

                if (combatantList[atkTarIndex].entity == player)
                    this.gm.pm.takeDmg(combatantList[i].attackDmg); // changed to use new dmg method
            }
        }
        yield break;
    }

    IEnumerator ClashAnim(CList entity, CList entity2)
    {
        this.rollParchment.SetActive(true);
        Vector3 start = entity.entity.transform.position;
        Vector3 end = entity2.entity.transform.position;
        Vector3 start2 = entity2.entity.transform.position;
        Vector3 end2 = entity.entity.transform.position;
        Vector3 halfway = (start + end) / 2;
        Vector3 halfway2 = (start2 + end2) / 2;

        GameObject tile = Resources.Load<GameObject>("Prefabs/attackAnimHighlight");

        GameObject tileTemp1 = Instantiate(tile, entity2.entity.transform.position, Quaternion.identity);
        GameObject tileTemp2 = Instantiate(tile, entity.entity.transform.position, Quaternion.identity);

        turnEntity(entity.entity, entity.atkTar[0]);
        turnEntity(entity2.entity, entity2.atkTar[0]);

        while (entity.entity.transform.position != halfway && entity2.entity.transform.position != halfway2)
        {
            if (entity.entity.transform.position != halfway)
                entity.entity.transform.position = Vector3.MoveTowards(entity.entity.transform.position, halfway, slideSpeed * Time.deltaTime);
            if (entity2.entity.transform.position != halfway2)
                entity2.entity.transform.position = Vector3.MoveTowards(entity2.entity.transform.position, halfway2, slideSpeed * Time.deltaTime);

            yield return null;
        }

        // clash logic
        int roll = Random.Range(1, 13) + this.gm.pm.pc.getStatModifier2(entity2.entity.GetComponent<Enemy>().getStrength());
        yield return StartCoroutine(rollScript.waitForStart("STR", this.gm.pm.getStatModifier("STR"), roll));

        if ((dr1.final + dr2.final + this.gm.pm.getStatModifier("STR") >= roll))
        {
            entity2.hp -= entity.attackDmg;

            if (entity2.hp <= 0)
            {
                entity2.entity.SetActive(false);
            }

        }
        else
        {
            entity.hp -= entity2.attackDmg;

            if (entity.hp <= 0)
            {
                entity.entity.SetActive(false);
            }
            this.gm.pm.takeDmg(entity2.attackDmg);

        }

        this.rollParchment.SetActive(false);
        this.dr1.final = 0;
        this.dr2.final = 0;


        while (entity.entity.transform.position != start && entity2.entity.transform.position != start2)
        {
            if (entity.entity.transform.position != start)
                entity.entity.transform.position = Vector3.MoveTowards(entity.entity.transform.position, start, slideSpeed * Time.deltaTime);
            if (entity2.entity.transform.position != start2)
                entity2.entity.transform.position = Vector3.MoveTowards(entity2.entity.transform.position, start2, slideSpeed * Time.deltaTime);

            yield return null;
        }

        Destroy(tileTemp1);
        Destroy(tileTemp2);

        // In case something happens, framerate dip or something that leads to them being off, force them to be in their appropriate spots
        entity.entity.transform.position = start;
        entity2.entity.transform.position = start2;
        yield break;
    }

    IEnumerator attackAnim(CList c)
    {
        Vector3 s = c.entity.transform.position; // start pos
        List<Vector3> attacks = new List<Vector3>(); // list of attack spots

        turnEntity(c.entity, c.atkTar[0]);

        GameObject tile = Resources.Load<GameObject>("Prefabs/attackAnimHighlight");
        foreach (Vector3 v in c.atkTar)
        {
            attacks.Add((((v+s)/2) + s)/2);
        }

        int i = 0;
        foreach(Vector3 v in attacks)
        {
            GameObject tileTemp = Instantiate(tile, c.atkTar[i++], Quaternion.identity);
            while (c.entity.transform.position != v)
            {
                c.entity.transform.position = Vector3.MoveTowards(c.entity.transform.position, v, slideSpeed * Time.deltaTime);
                yield return null;
            }
            while (c.entity.transform.position != s)
            {
                c.entity.transform.position = Vector3.MoveTowards(c.entity.transform.position, s, slideSpeed * Time.deltaTime);
                yield return null;
            }
            Destroy(tileTemp);
        }

        c.entity.transform.position = s; // incase something went wrong with the moveTowards
        yield break;
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
            Debug.Log("Win");
        }

        // Tell PlayerManager it's now the player's turn... do it differently sometime maybe?
        //this.gm.pm.isTurn = true;
        this.gm.pm.newTurn(); // This new method will flip it back to player turn and decrement any cooldown timers
    }

    int GetIndexOfCombatant(GameObject entity)
    {
        if (entity == null)
            return -1;

        for (int i = 0; i < this.combatantList.Count; i++)
        {
            if (combatantList[i] != null && combatantList[i].entity == entity)
            {
                return i;
            }

        }

        Debug.AssertFormat(false, "Couldn't find " + entity + " in CombatantList");
        return -1;
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

        // Debug.AssertFormat(false, "Could not find Combatant in combatantList at Vector3: " + pos);
        return null;
    }

    // Garbage O(n^2) passable detector for checking if a vector3 is passable
    // Used by highlights to detect whether or not to draw tile in a wall or off the map etc.
    public bool isPassable(Vector3 v)
    {
        int rows = gridCell.GetLength(0);
        int cols = gridCell.GetLength(1);
        for (int i = 0; i<rows; i++)
        {
            for (int j = 0; j<cols; j++)
            {
                if (gridCell[i,j] != null)
                    if (gridCell[i, j].center == v)
                        return gridCell[i, j].pass;
            }
        }
        return false;
    }

    void FillCombatantList()
    {
        // Add player to combatantList
        combatantList.Add(this.gm.pm.combatInfo);

        // For the number of enemies requested to be spawned, add them to the compatantList
        for (int i = 0; i < this.numEnemies; i++)
        {
            print(enemies[i].name);
            combatantList.Add(new CList(enemies[i]));
            combatantList[GetIndexOfCombatant(enemies[i])].hp = enemies[i].GetComponent<Enemy>().maxHp;
            // combatantList[GetIndexOfCombatant(enemies[i])] = enemies[i].GetComponent<Enemy>().maxHp; THIS SHOULD UPDATE WITH THE ENEMY STRENGTH
        }
        if (combatantList == null)
            print("OH NO");
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

        // Instantiate Player
        this.player = GameObject.Instantiate(GameObject.Find(this.gm.pm.characterName), playerSpawnerLoc, Quaternion.identity);
        this.player.transform.SetParent(Entities.transform);
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

    void printGrid()
    {
        int rowLen = this.gridCell.GetLength(0);
        int colLen = this.gridCell.GetLength(1);

        print("Here's the grid");
        string matrix = "";

        if (gridCell[0,0] != null)
            print(gridCell[0, 0].center.ToString("F2"));

        for (int i = 0; i < rowLen; i++)
        {
            for (int j = 0; j < colLen; j++)
            {
                if (gridCell[i,j] != null)
                {
                    if (gridCell[i,j].pass)
                        matrix += string.Format("{0, 3}", "0");
                    else
                        matrix += string.Format("{0, 3}", "1");
                }
                else                   
                        matrix += string.Format("{0, 3}", "n");
            }
            matrix += System.Environment.NewLine + System.Environment.NewLine;
        }
        print(matrix);
    }

    public List<CList> getCombatantList() { return this.combatantList; }
    public Cell[,] getGrid() { return this.gridCell; }
    public Vector3 getPlayerPosition() { return new Vector3(this.playerX, this.playerY, 0); }

    void turnEntity(GameObject entity, Vector3 movTar)
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
        else if (dirX <= 0)
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
