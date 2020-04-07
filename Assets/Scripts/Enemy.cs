﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;

public enum EnemyClass
{
	Dunce, Brawler, Flanker, Auxillary, Assassin, Archer
}

public class Enemy : MonoBehaviour
{
	protected BattleManager bm;
	protected int enemyId;
	protected int health;
	protected int damage;
	protected int strength;
	protected Vector3Int gridPosition;
	protected Vector3 moveTarget;
	protected List<Vector3> attackTarget;
	protected int decision; // 0 - no decision, 1 - move, 2 - attack
	protected CList combatantEntry;
	protected bool initFlag = false;

	// General Stats
	public EnemyClass enemyClass = EnemyClass.Dunce;
	public float movePercentage = .66f;
	public float specialAttackPercentage = .5f;

    // Other stats
    public int maxHp = 2;
    public int maxStrength = 1;

	// Melee Attack Damage
	public int strikeDamage = 1;
	public int cleaveDamage = 2;
	public int whammyDamage = 2; 
	public int thrustDamage = 1;
	public int sliceDamage = 2;
	public int cycloneDamage = 2;
	public int cycleKickDamage = 2;
	public int cyclePunchDamage = 2;

	// Ranged Attack Damage
	public int shortshotDamage = 1;
	public int mediumshotDamage = 1;
	public int longshotDamage = 1;
	public int shortRainDamage = 2;
	public int mediumRainDamage = 2;
	public int longRainDamage = 2;

	// Heat-Up!
	public int cleaveHeatup = 2;
	public int whammyHeatup = 2;
	public int thrustHeatup = 2;
	public int sliceHeatup = 2;
	public int cycloneHeatup = 4;
	public int cycleKickHeatup = 3;
	public int cyclePunchHeatup = 3;

	public int shortshotHeatup = 1;
	public int mediumshotHeatup = 1;
	public int longshotHeatup = 1;
	public int shortRainHeatup = 3;
	public int mediumRainHeatup = 3;
	public int longRainHeatup = 3;

	// Cool-Down B)
	protected int cleaveCooldown = 0;
	protected int whammyCooldown = 0;
	protected int thrustCooldown = 0;
	protected int sliceCooldown = 0;
	protected int cycloneCooldown = 0;
	protected int cycleKickCooldown = 0;
	protected int cyclePunchCooldown = 0;

	protected int shortshotCooldown = 0;
	protected int mediumshotCooldown = 0;
	protected int longshotCooldown = 0;
	protected int shortRainCooldown = 0;
	protected int mediumRainCooldown = 0;
	protected int longRainCooldown = 0;

	// Melee Attacks
	// Enemy can always default attack
	public bool Cleave = false;
	public bool Thrust = false;
	public bool Whammy = false;
	public bool Slice = false;
	public bool Cyclone = false;
	public bool CycleKick = false;
	public bool CyclePunch = false;

	// Ranged Attacks
	public bool Shortshot = false;
	public bool Mediumshot = false;
	public bool Longshot = false;
	public bool ShortRain = false;
	public bool MediumRain = false;
	public bool LongRain = false;

	//===========   Unity Methods   ============//

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //===========   Other methods   ===========//
    protected void initialize()
    {
    	this.initFlag = true;

    	// Set default stats
    	this.health = this.maxHp;
    	this.damage = 1;
    	this.strength = this.maxStrength;

    	// Set initial positions
    	this.attackTarget = new List<Vector3>();
    	this.moveTarget = new Vector3(0, 0, 0);

    	// Generate ID
    	this.generateID();

    	if (this.combatantEntry == null)
    	{
    		Debug.Log("E#"+this.enemyId+" says AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAHHHHHHHHHHHHHHHHHHHHHHHHHHH");
    		// this.kill();
    	}
    	else
    	{
    		this.gridPosition = new Vector3Int(this.combatantEntry.gridX, this.combatantEntry.gridY, 0);
    	}

    	Debug.Log("E#"+this.enemyId+" has initialized successfully!");
    }

    protected void generateID()
    {
    	this.enemyId = (int)(Random.value * 9999999);
    }

    //===========   Thinking methods   ===========//
    protected void move()
    {
		Debug.Log("E#"+this.enemyId+" is moving!");

		// Grab the target and the enemy's location from the BattleManager
        Point target = new Point(BattleManager.Instance.combatantList[0].gridX, BattleManager.Instance.combatantList[0].gridY);
        Point me = new Point(this.combatantEntry.gridX, this.combatantEntry.gridY);

        // Movement Restrictions
        List<Point> offlimits;

        switch (this.enemyClass)
        {
        	default:
        	case EnemyClass.Dunce:
        		offlimits = this.DunceRestrictions(me, target); break;
        	case EnemyClass.Brawler:
        		offlimits = this.BrawlerRestrictions(me, target); break;
        	case EnemyClass.Flanker:
        		offlimits = this.FlankerRestrictions(me, target); break;
        	case EnemyClass.Auxillary:
        		offlimits = this.AuxillaryRestrictions(me, target); break;
        	case EnemyClass.Assassin:
        		offlimits = this.AssassinRestrictions(me, target); break;
        	case EnemyClass.Archer:
        		offlimits = this.ArcherRestrictions(me, target); break;
        }

        // Pathfind!
        Point nextSpot = BFS.bfs(BattleManager.Instance.gridCell, me, target, offlimits);

        Debug.Log("GridCell X-Length: "+BattleManager.Instance.gridCell.GetLength(0));
        Debug.Log("GridCell Y-Length: "+BattleManager.Instance.gridCell.GetLength(1));
        Debug.Log("Chosen Next Point: ("+nextSpot.X+", "+nextSpot.Y+")");

        // Don't move if we accidentally chose a spot out of bounds, otherwise move to our next spot
        if (nextSpot.X < 0 || nextSpot.X >= BattleManager.Instance.gridCell.GetLength(0) || nextSpot.Y < 0 || nextSpot.Y >= BattleManager.Instance.gridCell.GetLength(1))
        	this.setMove(BattleManager.Instance.gridCell[me.X, me.Y].center);
        else
	        this.setMove(BattleManager.Instance.gridCell[nextSpot.X, nextSpot.Y].center);
    }

    protected void attack()
    {
    	// Initialize our list of potential attacks, and our final decision
    	List<List<Point>> attackTiles = new List<List<Point>>();
    	List<Point> decidedTile;

    	// Grab information from the BattleManager
    	Point me = new Point(this.combatantEntry.gridX, this.combatantEntry.gridY);
    	Point target = new Point(BattleManager.Instance.combatantList[0].gridX, BattleManager.Instance.combatantList[0].gridY);

    	// Melee Attacks
    	if (this.Cleave && this.cleaveCooldown <= 0 && Random.value < this.specialAttackPercentage)
    	{
    		attackTiles = this.getCleaveList(me);
    		this.damage = this.cleaveDamage;
    		this.cleaveCooldown = this.cleaveHeatup + 1;
    	}
    	else if (this.Whammy && this.whammyCooldown <= 0 && Random.value < this.specialAttackPercentage)
    	{
    		attackTiles = this.getWhammyList(me);
    		this.damage = this.whammyDamage;
    		this.whammyCooldown = this.whammyHeatup + 1;
    	}
    	else if (this.Thrust && this.thrustCooldown <= 0 && Random.value < this.specialAttackPercentage)
    	{
    		attackTiles = this.getThrustList(me);
    		this.damage = this.thrustDamage;
    		this.thrustCooldown = this.thrustHeatup + 1;
    	}
    	else if (this.Slice && this.sliceCooldown <= 0 && Random.value < this.specialAttackPercentage)
    	{
    		attackTiles = this.getSliceList(me);
    		this.damage = this.sliceDamage;
    		this.sliceCooldown = this.sliceHeatup + 1;
    	}
    	else if (this.Cyclone && this.cycloneCooldown <= 0 && Random.value < this.specialAttackPercentage)
    	{
    		attackTiles = this.getCycloneList(me);
    		this.damage = this.cycloneDamage;
    		this.cycloneCooldown = this.cycloneHeatup + 1;
    	}
    	else if (this.CycleKick && this.cycleKickCooldown <= 0 && Random.value < this.specialAttackPercentage)
    	{
    		attackTiles = this.getCycleKickList(me);
    		this.damage = this.cycleKickDamage;
    		this.cycleKickCooldown = this.cycleKickHeatup + 1;
    	}
    	else if (this.CyclePunch && this.cyclePunchCooldown <= 0 && Random.value < this.specialAttackPercentage)
    	{
    		attackTiles = this.getCyclePunchList(me);
    		this.damage = this.cyclePunchDamage;
    		this.cyclePunchCooldown = this.cyclePunchHeatup + 1;
    	}

    	// Ranged Attacks
    	else if (this.Shortshot && this.shortshotCooldown <= 0 && Random.value < this.specialAttackPercentage)
    	{
    		attackTiles = this.getShortshotList(me);
    		this.damage = this.shortshotDamage;
    		this.shortshotCooldown = this.shortshotHeatup + 1;
    	}
    	else if (this.Mediumshot && this.mediumshotCooldown <= 0 && Random.value < this.specialAttackPercentage)
    	{
    		attackTiles = this.getMediumshotList(me);
    		this.damage = this.mediumshotDamage;
    		this.mediumshotCooldown = this.mediumshotHeatup + 1;
    	}
    	else if (this.Longshot && this.longshotCooldown <= 0 && Random.value < this.specialAttackPercentage)
    	{
    		attackTiles = this.getLongshotList(me);
    		this.damage = this.longshotDamage;
    		this.longshotCooldown = this.longshotHeatup + 1;
    	}
    	else if (this.ShortRain && this.shortRainCooldown <= 0 && Random.value < this.specialAttackPercentage)
    	{
    		attackTiles = this.getShortRainList(me);
    		this.damage = this.shortRainDamage;
    		this.shortRainCooldown = this.shortRainHeatup + 1;
    	}
    	else if (this.MediumRain && this.mediumRainCooldown <= 0 && Random.value < this.specialAttackPercentage)
    	{
    		attackTiles = this.getMediumRainList(me);
    		this.damage = this.mediumRainDamage;
    		this.mediumRainCooldown = this.mediumRainHeatup + 1;
    	}
    	else if (this.LongRain && this.longRainCooldown <= 0 && Random.value < this.specialAttackPercentage)
    	{
    		attackTiles = this.getLongRainList(me);
    		this.damage = this.longRainDamage;
    		this.longRainCooldown = this.longRainHeatup + 1;
    	}

    	// Default Attack
    	else
    	{
    		attackTiles = this.getStrikeList(me);
    		this.damage = this.strikeDamage;
    	}

    	// Set our default (random attack) first, then try to be more specific
    	int choice = (int)(Random.value * attackTiles.Count);
    	decidedTile = attackTiles[choice];
    	Debug.Log("E#"+this.enemyId+" picked a random tile to attack!");

    	// Try to pick the attack that will hit the player in their current position
    	foreach (List<Point> l in attackTiles)
    	{
    		if (l.Contains(target))
    		{
    			decidedTile = l;
    			Debug.Log("E#"+this.enemyId+" is cleaving the target's position!");
    		}
    	}

    	// Convert to vector positons
    	List<Vector3> temp = new List<Vector3>();

    	foreach (Point p in decidedTile)
    	{
    		if (p.X >= 0 && p.X < BattleManager.Instance.gridCell.GetLength(0) && p.Y >= 0 && p.Y < BattleManager.Instance.gridCell.GetLength(1))
    		{
	    		temp.Add(BattleManager.Instance.gridCell[p.X, p.Y].center);
    		}
    	}

    	// Set attacks to be ready for export
    	this.setAttacks(temp);
    }

    public void decide()
    {
    	Debug.Log("E#"+this.enemyId+" has been called upon to think!");

    	if (Random.value < this.movePercentage)
    	{
    		this.move();
    		this.decision = 1;
    	}
    	else
    	{
    		this.attack();
    		this.decision = 2;
    	}

    	this.coolOff();
    }

    public void init()
    {
    	if (!this.initFlag)
    	{
    		this.initialize();
    	}
    }

    public void kill()
    {
    	// Fucking die you piece of shit enemy
    	Debug.Log("Oy blyat! E#"+this.enemyId+" am dead");
    	Destroy(this);
    }

    public void dealDamage(int dam)
    {
    	this.health -= dam;

    	if (this.health <= 0)
    		kill();
    }

    //===========   Exporting Functions   ===========//
    protected void setMove(Vector3 target)
    {
        this.moveTarget = target;
    }

    protected void setSingleAttack(Vector3 target)
    {
    	this.attackTarget.Clear();
    	this.attackTarget.Add(target);
    }

    protected void setAttacks(List<Vector3> target)
    {
    	this.attackTarget.Clear();
    	this.attackTarget = target;
    }

    //============   Movement Restrictions   ============//
    protected List<Point> DunceRestrictions(Point me, Point target)
    {
    	List<Point> list = new List<Point>();

    	// Dunce randomly picks a position around the player to avoid
    	int xOffset = (int)(Random.value * 2) + -1;
        int yOffset = (int)(Random.value * 2) + -1;
        list.Add(new Point(target.X + xOffset, target.Y + yOffset));

        return list;
    }

    protected List<Point> BrawlerRestrictions(Point me, Point target)
    {
    	List<Point> list = new List<Point>();

    	// Brawler avoids the diagonal tiles
    	list.Add(new Point(target.X + 1, target.Y - 1));
        list.Add(new Point(target.X + 1, target.Y + 1));
        list.Add(new Point(target.X - 1, target.Y - 1));
        list.Add(new Point(target.X - 1, target.Y + 1));

        return list;
    }

    protected List<Point> FlankerRestrictions(Point me, Point target)
    {
    	List<Point> list = new List<Point>();

    	// Flanker avoids the north and south tiles
        list.Add(new Point(target.X - 1, target.Y));
        list.Add(new Point(target.X + 1, target.Y));

    	return list;
    }

    protected List<Point> AuxillaryRestrictions(Point me, Point target)
    {
    	List<Point> list = new List<Point>();

    	// Auxillary avoids the north, south, east, and west tiles
        list.Add(new Point(target.X - 1, target.Y));
        list.Add(new Point(target.X + 1, target.Y));
        list.Add(new Point(target.X, target.Y - 1));
        list.Add(new Point(target.X, target.Y + 1));

    	return list;
    }

    protected List<Point> AssassinRestrictions(Point me, Point target)
    {
    	List<Point> list = new List<Point>();

    	// Assassin avoids the east and west tiles
        list.Add(new Point(target.X, target.Y - 1));
        list.Add(new Point(target.X, target.Y + 1));

    	return list;
    }

    protected List<Point> ArcherRestrictions(Point me, Point target)
    {
    	List<Point> list = new List<Point>();

    	// Always stays at least 1 tile away from the target
    	// Makes a single hole to traverse through to try and keep its distance at most times
    	list.Add(new Point(target.X + 1, target.Y));
    	list.Add(new Point(target.X - 1, target.Y));
    	list.Add(new Point(target.X, target.Y + 1));
    	list.Add(new Point(target.X, target.Y - 1));

    	int choice = (int)(Random.value * list.Count);
    	list.RemoveAt(choice);

    	list.Add(new Point(target.X + 1, target.Y + 1));
    	list.Add(new Point(target.X + 1, target.Y - 1));
    	list.Add(new Point(target.X - 1, target.Y + 1));
    	list.Add(new Point(target.X - 1, target.Y - 1));

    	return list;
    }

    //==========   Melee Attacks   ==========//
    // Hit One Tile Once (8 possible positions)
    protected List<List<Point>> getStrikeList(Point me)
    {
    	List<List<Point>> list = new List<List<Point>>();

    	list.Add(new List<Point>());
    	list[0].Add(new Point(me.X + 1, me.Y + 1));

    	list.Add(new List<Point>());
    	list[1].Add(new Point(me.X + 1, me.Y));

    	list.Add(new List<Point>());
    	list[2].Add(new Point(me.X + 1, me.Y - 1));

    	list.Add(new List<Point>());
    	list[3].Add(new Point(me.X, me.Y + 1));

    	list.Add(new List<Point>());
    	list[4].Add(new Point(me.X, me.Y - 1));

    	list.Add(new List<Point>());
    	list[5].Add(new Point(me.X - 1, me.Y + 1));

    	list.Add(new List<Point>());
    	list[6].Add(new Point(me.X - 1, me.Y));

    	list.Add(new List<Point>());
    	list[7].Add(new Point(me.X - 1, me.Y - 1));

    	return list;
    }

    // Hit Two Adjacent Tiles
    protected List<List<Point>> getCleaveList(Point me)
    {
    	List<List<Point>> list = new List<List<Point>>();

    	list.Add(new List<Point>());
    	list[0].Add(new Point(me.X + 1, me.Y + 1));
    	list[0].Add(new Point(me.X, me.Y + 1));

    	list.Add(new List<Point>());
    	list[1].Add(new Point(me.X - 1, me.Y + 1));
    	list[1].Add(new Point(me.X, me.Y + 1));

    	list.Add(new List<Point>());
    	list[2].Add(new Point(me.X + 1, me.Y - 1));
    	list[2].Add(new Point(me.X, me.Y - 1));

    	list.Add(new List<Point>());
    	list[3].Add(new Point(me.X - 1, me.Y - 1));
    	list[3].Add(new Point(me.X, me.Y - 1));

    	list.Add(new List<Point>());
    	list[4].Add(new Point(me.X + 1, me.Y + 1));
    	list[4].Add(new Point(me.X + 1, me.Y));

    	list.Add(new List<Point>());
    	list[5].Add(new Point(me.X + 1, me.Y - 1));
    	list[5].Add(new Point(me.X + 1, me.Y));

    	list.Add(new List<Point>());
    	list[6].Add(new Point(me.X - 1, me.Y + 1));
    	list[6].Add(new Point(me.X - 1, me.Y));

    	list.Add(new List<Point>());
    	list[7].Add(new Point(me.X - 1, me.Y - 1));
    	list[7].Add(new Point(me.X - 1, me.Y));

    	return list;
    }

    // Hit One Tile Twice
    protected List<List<Point>> getWhammyList(Point me)
    {
    	List<List<Point>> list = new List<List<Point>>();

		list.Add(new List<Point>());
    	list[0].Add(new Point(me.X + 1, me.Y + 1));
    	list[0].Add(new Point(me.X + 1, me.Y + 1));

    	list.Add(new List<Point>());
    	list[1].Add(new Point(me.X + 1, me.Y));
    	list[1].Add(new Point(me.X + 1, me.Y));

    	list.Add(new List<Point>());
    	list[2].Add(new Point(me.X + 1, me.Y - 1));
    	list[2].Add(new Point(me.X + 1, me.Y - 1));

    	list.Add(new List<Point>());
    	list[3].Add(new Point(me.X, me.Y + 1));
    	list[3].Add(new Point(me.X, me.Y + 1));

    	list.Add(new List<Point>());
    	list[4].Add(new Point(me.X, me.Y - 1));
    	list[4].Add(new Point(me.X, me.Y - 1));

    	list.Add(new List<Point>());
    	list[5].Add(new Point(me.X - 1, me.Y + 1));
    	list[5].Add(new Point(me.X - 1, me.Y + 1));

    	list.Add(new List<Point>());
    	list[6].Add(new Point(me.X - 1, me.Y));
    	list[6].Add(new Point(me.X - 1, me.Y));

    	list.Add(new List<Point>());
    	list[7].Add(new Point(me.X - 1, me.Y - 1));
    	list[7].Add(new Point(me.X - 1, me.Y - 1));

    	return list;
    }

    // Hit Two Tiles On Straight-Away
    protected List<List<Point>> getThrustList(Point me)
    {
    	List<List<Point>> list = new List<List<Point>>();

    	list.Add(new List<Point>());
    	list[0].Add(new Point(me.X + 1, me.Y));
    	list[0].Add(new Point(me.X + 2, me.Y));

    	list.Add(new List<Point>());
    	list[1].Add(new Point(me.X - 1, me.Y));
    	list[1].Add(new Point(me.X - 2, me.Y));

    	list.Add(new List<Point>());
    	list[2].Add(new Point(me.X, me.Y + 1));
    	list[2].Add(new Point(me.X, me.Y + 2));

    	list.Add(new List<Point>());
    	list[3].Add(new Point(me.X, me.Y - 1));
    	list[3].Add(new Point(me.X, me.Y - 2));

    	return list;
    }

    // Hit Two Diagonal Tiles on One Side
    protected List<List<Point>> getSliceList(Point me)
    {
    	List<List<Point>> list = new List<List<Point>>();

    	list.Add(new List<Point>());
    	list[0].Add(new Point(me.X + 1, me.Y + 1));
    	list[0].Add(new Point(me.X + 1, me.Y - 1));

    	list.Add(new List<Point>());
    	list[1].Add(new Point(me.X + 1, me.Y + 1));
    	list[1].Add(new Point(me.X - 1, me.Y + 1));

    	list.Add(new List<Point>());
    	list[2].Add(new Point(me.X - 1, me.Y + 1));
    	list[2].Add(new Point(me.X + 1, me.Y + 1));

    	list.Add(new List<Point>());
    	list[3].Add(new Point(me.X + 1, me.Y - 1));
    	list[3].Add(new Point(me.X + 1, me.Y + 1));

    	return list;
    }

    // Hit Every Tile Around Entity (8 tiles)
    protected List<List<Point>> getCycloneList(Point me)
    {
    	List<List<Point>> list = new List<List<Point>>();

    	list.Add(new List<Point>());
    	list[0].Add(new Point(me.X + 1, me.Y + 1));
    	list[0].Add(new Point(me.X + 1, me.Y));
    	list[0].Add(new Point(me.X + 1, me.Y - 1));
    	list[0].Add(new Point(me.X, me.Y - 1));
    	list[0].Add(new Point(me.X - 1, me.Y - 1));
    	list[0].Add(new Point(me.X - 1, me.Y));
    	list[0].Add(new Point(me.X - 1, me.Y + 1));
    	list[0].Add(new Point(me.X, me.Y + 1));

    	return list;
    }

    // Hit Adjacent Tiles Around Entity (4 tiles)
    protected List<List<Point>> getCycleKickList(Point me)
    {
    	List<List<Point>> list = new List<List<Point>>();

    	list.Add(new List<Point>());
    	list[0].Add(new Point(me.X + 1, me.Y));
    	list[0].Add(new Point(me.X - 1, me.Y));
    	list[0].Add(new Point(me.X, me.Y + 1));
    	list[0].Add(new Point(me.X, me.Y - 1));

    	return list;
    }

    // Hit All Diagonals Around Entity (4 tiles)
    protected List<List<Point>> getCyclePunchList(Point me)
    {
    	List<List<Point>> list = new List<List<Point>>();

    	list.Add(new List<Point>());
    	list[0].Add(new Point(me.X + 1, me.Y + 1));
    	list[0].Add(new Point(me.X + 1, me.Y - 1));
    	list[0].Add(new Point(me.X - 1, me.Y + 1));
    	list[0].Add(new Point(me.X - 1, me.Y - 1));

    	return list;
    }

    //==========   Ranged Attacks   ==========//
    // Hits One Tile Away On STRAIGHT-AWAYS
    protected List<List<Point>> getShortshotList(Point me)
    {
    	List<List<Point>> list = new List<List<Point>>();

    	list.Add(new List<Point>());
    	list[0].Add(new Point(me.X + 2, me.Y));

    	list.Add(new List<Point>());
    	list[1].Add(new Point(me.X - 2, me.Y));

    	list.Add(new List<Point>());
    	list[2].Add(new Point(me.X, me.Y + 2));

    	list.Add(new List<Point>());
    	list[3].Add(new Point(me.X, me.Y - 2));

    	return list;
    }

    // Hits Two Tiles Away On STRAIGHT-AWAYS
    protected List<List<Point>> getMediumshotList(Point me)
    {
    	List<List<Point>> list = new List<List<Point>>();

    	list.Add(new List<Point>());
    	list[0].Add(new Point(me.X + 3, me.Y));

    	list.Add(new List<Point>());
    	list[1].Add(new Point(me.X - 3, me.Y));

    	list.Add(new List<Point>());
    	list[2].Add(new Point(me.X, me.Y + 3));

    	list.Add(new List<Point>());
    	list[3].Add(new Point(me.X, me.Y - 3));

    	return list;
    }

    // Hits Three Tiles Away On STRAIGHT-AWAYS
    protected List<List<Point>> getLongshotList(Point me)
    {
    	List<List<Point>> list = new List<List<Point>>();

    	list.Add(new List<Point>());
    	list[0].Add(new Point(me.X + 4, me.Y));

    	list.Add(new List<Point>());
    	list[1].Add(new Point(me.X - 4, me.Y));

    	list.Add(new List<Point>());
    	list[2].Add(new Point(me.X, me.Y + 4));

    	list.Add(new List<Point>());
    	list[3].Add(new Point(me.X, me.Y - 4));

    	return list;
    }

    // Hits One Tile Away On STRAIGHT-AWAYS + Tile Behind + Tile to Either Side
    protected List<List<Point>> getShortRainList(Point me)
    {
    	List<List<Point>> list = new List<List<Point>>();

    	list.Add(new List<Point>());
    	list[0].Add(new Point(me.X + 2, me.Y));
    	list[0].Add(new Point(me.X + 3, me.Y));
    	list[0].Add(new Point(me.X + 2, me.Y + 1));
    	list[0].Add(new Point(me.X + 2, me.Y - 1));

    	list.Add(new List<Point>());
    	list[1].Add(new Point(me.X - 2, me.Y));
    	list[1].Add(new Point(me.X - 3, me.Y));
    	list[1].Add(new Point(me.X - 2, me.Y + 1));
    	list[1].Add(new Point(me.X - 2, me.Y - 1));

    	list.Add(new List<Point>());
    	list[2].Add(new Point(me.X, me.Y + 2));
    	list[2].Add(new Point(me.X, me.Y + 3));
    	list[2].Add(new Point(me.X + 1, me.Y + 2));
    	list[2].Add(new Point(me.X - 1, me.Y + 2));

    	list.Add(new List<Point>());
    	list[3].Add(new Point(me.X, me.Y - 2));
    	list[3].Add(new Point(me.X, me.Y - 3));
    	list[3].Add(new Point(me.X + 1, me.Y - 2));
    	list[3].Add(new Point(me.X - 1, me.Y - 2));

    	return list;
    }

    // Hits Two Tiles Away On STRAIGHT-AWAYS + Tile Behind + Tile to Either Side
    protected List<List<Point>> getMediumRainList(Point me)
    {
    	List<List<Point>> list = new List<List<Point>>();

    	list.Add(new List<Point>());
    	list[0].Add(new Point(me.X + 3, me.Y));
    	list[0].Add(new Point(me.X + 4, me.Y));
    	list[0].Add(new Point(me.X + 3, me.Y + 1));
    	list[0].Add(new Point(me.X + 3, me.Y - 1));

    	list.Add(new List<Point>());
    	list[1].Add(new Point(me.X - 3, me.Y));
    	list[1].Add(new Point(me.X - 4, me.Y));
    	list[1].Add(new Point(me.X - 3, me.Y + 1));
    	list[1].Add(new Point(me.X - 3, me.Y - 1));

    	list.Add(new List<Point>());
    	list[2].Add(new Point(me.X, me.Y + 3));
    	list[2].Add(new Point(me.X, me.Y + 4));
    	list[2].Add(new Point(me.X + 1, me.Y + 3));
    	list[2].Add(new Point(me.X - 1, me.Y + 3));

    	list.Add(new List<Point>());
    	list[3].Add(new Point(me.X, me.Y - 3));
    	list[3].Add(new Point(me.X, me.Y - 4));
    	list[3].Add(new Point(me.X + 1, me.Y - 3));
    	list[3].Add(new Point(me.X - 1, me.Y - 3));

    	return list;
    }

    // Hits Three Tiles Away On STRAIGHT-AWAYS + Tile Behind + Tile to Either Side
    protected List<List<Point>> getLongRainList(Point me)
    {
    	List<List<Point>> list = new List<List<Point>>();

    	list.Add(new List<Point>());
    	list[0].Add(new Point(me.X + 4, me.Y));
    	list[0].Add(new Point(me.X + 5, me.Y));
    	list[0].Add(new Point(me.X + 4, me.Y + 1));
    	list[0].Add(new Point(me.X + 4, me.Y - 1));

    	list.Add(new List<Point>());
    	list[1].Add(new Point(me.X - 4, me.Y));
    	list[1].Add(new Point(me.X - 5, me.Y));
    	list[1].Add(new Point(me.X - 4, me.Y + 1));
    	list[1].Add(new Point(me.X - 4, me.Y - 1));

    	list.Add(new List<Point>());
    	list[2].Add(new Point(me.X, me.Y + 4));
    	list[2].Add(new Point(me.X, me.Y + 5));
    	list[2].Add(new Point(me.X + 1, me.Y + 4));
    	list[2].Add(new Point(me.X - 1, me.Y + 4));

    	list.Add(new List<Point>());
    	list[3].Add(new Point(me.X, me.Y - 4));
    	list[3].Add(new Point(me.X, me.Y - 5));
    	list[3].Add(new Point(me.X + 1, me.Y - 4));
    	list[3].Add(new Point(me.X - 1, me.Y - 4));

    	return list;
    }

    //===========   SUPER IMPORTANT METHOD   ===========//
    public void coolOff()
    {
    	// Melee Cooldowns
    	if (cleaveCooldown > 0)
    	{
    		cleaveCooldown--;
    	}

    	if (whammyCooldown > 0)
    	{
    		whammyCooldown--;
    	}

		if (thrustCooldown > 0)
		{
			thrustCooldown--;
		}

		if (sliceCooldown > 0)
		{
			sliceCooldown--;
		}

		if (cycloneCooldown > 0)
		{
			cycloneCooldown--;
		}

		if (cycleKickCooldown > 0)
		{
			cycleKickCooldown--;
		}

		if (cyclePunchCooldown > 0)
		{
			cyclePunchCooldown--;
		}

		// Ranged Cooldowns
		if (shortshotCooldown > 0)
		{
			shortshotCooldown--;
		}

		if (mediumshotCooldown > 0)
		{
			mediumshotCooldown--;
		}

		if (longshotCooldown > 0)
		{
			longshotCooldown--;
		}

		if (shortRainCooldown > 0)
		{
			shortRainCooldown--;
		}

		if (mediumRainCooldown > 0)
		{
			mediumRainCooldown--;
		}

		if (longRainCooldown > 0)
		{
			longRainCooldown--;
		}
    }

    //===========   Getters   ===========//
    public int getHealth() { return this.health; }
    public int getStrength() { return this.strength; }
    public int getDamage() { return this.damage; }
    public int getID() { return this.enemyId; }
    public int getDecision() { return this.decision; }
    public Vector3 getMoveVector() { return this.moveTarget; }
    public List<Vector3> getAttackVector() { return this.attackTarget; }
    public EnemyClass getClass() { return this.enemyClass; }

    //===========   Setters   ===========//
    public void setCombatantEntry(CList c)
    {
    	this.combatantEntry = c;
    	this.health = c.hp;
    }
    public void setHealth(int h) { this.health = h; }
    public void setDamage(int d) { this.damage = d; }
    public void setStrength(int s) { this.strength = s; }
    public void setStats(int h, int d, int s)
    {
    	this.setHealth(h);
    	this.setDamage(d);
    	this.setStrength(s);
    }
    public void setBattleManager(BattleManager b) { this.bm = b; }
    public void setEnemyClass(EnemyClass ec) { this.enemyClass = ec; }
}