using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BiomeSet : short
{
	NOTHING = -1, FOREST, CAVE, ICE
};

public enum Direction
{
	ERROR = -1, NORTH = 0, SOUTH, EAST, WEST
};

public struct Position
{
	public int x;
	public int y;
	public Direction dir;

	public Position(int xx, int yy)
	{
		x = xx;
		y = yy;
		dir = Direction.NORTH;
	}

	public Position(int xx, int yy, Direction d)
	{
		x = xx;
		y = yy;
		dir = d;
	}
}

public struct GridCell
{
	public GameObject tile;
    public GameObject entity;
}

public class CombatBoard : MonoBehaviour
{
	public GameObject grid;
	public GameObject tilemap;
    public GameObject player;
    public GameObject companion;
    public GameObject[] enemies;
    public Vector3 playerLoc = new Vector3(0, (float)-1.75, 0);
    public Vector3 companionLoc = new Vector3((float)-0.5, (float)-2, 0);
    public Vector3[] enemyLoc;
    public int a;
    public int b;
    public int numEnemies;

    void Start()
	{
        this.grid = GameObject.Find("ForestGrid");
        this.tilemap = GameObject.Find("Arena2");

        GameObject[] tilemapDeactivate = GameObject.FindGameObjectsWithTag("Tilemap");

        for(int i = 0; i < tilemapDeactivate.Length; i++)
        {
            tilemapDeactivate[i].SetActive(false);
        }

        this.tilemap.SetActive(true);

        this.player = GameObject.Instantiate(GameObject.Find("TheWhiteKnight1"), playerLoc, Quaternion.identity);
        this.player = GameObject.Instantiate(GameObject.Find("honey"), companionLoc, Quaternion.identity);

        numEnemies = 4;
        a = -2;
        b = 2;
        enemyLoc = new Vector3[numEnemies];
        enemies = new GameObject[numEnemies];
        RandomEnemyPos();

        for(int i = 0; i < numEnemies; i++)
        {
            this.enemies[i] = GameObject.Instantiate(GameObject.Find("goblin2"), enemyLoc[i], Quaternion.identity);
        }
    }

    void Update()
    {

    }

    void RandomEnemyPos()
    {
        int index = 0;
        Start:
            Vector3 val;
            while (true)
            {
                val = new Vector3(((Random.Range(0, 1) == 1) ? 1 : -1) * Random.Range(a, b), 
                                  ((Random.Range(0, 1) == 1) ? 1 : -1) * Random.Range(a, b) + (float)0.25 * ((Random.Range(0, 1) == 1) ? 1 : 3), 
                                  0);
                for (int i = 0; i < numEnemies; i++)
                {
                    if (val == enemyLoc[i]) goto Start;
                }
                goto Outer;
            }
        Outer:
            enemyLoc[index++] = val;
            Debug.Log("enemyLoc[" + (index - 1) + "]: " + val);
            if (index == numEnemies)
                return;
            goto Start;
    }
}
