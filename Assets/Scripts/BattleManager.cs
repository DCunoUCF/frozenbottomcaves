using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum biome : short
{
	NOTHING = -1, FOREST, CAVE, ICECAVE, CASTLE, BOSS
};

public struct cell
{
	public bool pass;
    public GameObject entity;
}
public struct cList
{
    public GameObject entity;
    public bool move;
    public Vector3 movTar;
    public int dir;
    public int attack;
    public int attackDmg;
    public Vector3[] atkTar;

    public cList(GameObject newEntity)
    {
        entity = newEntity;
        move = false;
        movTar = new Vector3();
        dir = 0;
        attack = -1;
        attackDmg = 0;
        atkTar = null;
    }
}

public class BattleManager : MonoBehaviour
{
    public List<cList> combatantList;
    private cell[][] gridCell;
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
        grid = GameObject.Find("ForestGrid");
        activeArena = GameObject.Find("Arena1");
        arenaDeactivate = GameObject.FindGameObjectsWithTag("Tilemap");
        playerLoc = GameObject.FindGameObjectWithTag("pSpawn").transform.position;
        companionLoc = GameObject.FindGameObjectWithTag("cSpawn").transform.position;
        locGrabber = GameObject.FindGameObjectsWithTag("eSpawn");
        availEnemyLoc = new List<Vector3>();

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
        combatantList.Add(new cList(player));
        companion = GameObject.Instantiate(GameObject.Find("honey"), companionLoc, Quaternion.identity);
        combatantList.Add(new cList(companion));

        // Instantiate Enemies
        for (int i = 0; i < numEnemies; i++)
        {
            enemies[i] = GameObject.Instantiate(GameObject.Find("goblin2"), enemyLoc[i], Quaternion.identity);
            combatantList.Add(new cList(enemies[i]));
        }
    }

    void Update()
    {

    }

    void resolveMoves()
    {
        List<cList> moversList = new List<cList>();
        List<cList> attackersList = new List<cList>();
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

            // If the mover won't collide with anyone else on the board, they can legally move to their target move location
            if (!popped)
            {
                moversList[0].entity.transform.SetPositionAndRotation(moversList[0].movTar, Quaternion.identity);
                popped = false;
                moversList.RemoveAt(0);
            }
        }
    }

    void resolveAttacks()
    {

    }

    void whoStillHasLimbs()
    {

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
