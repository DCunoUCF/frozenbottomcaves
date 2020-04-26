using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;

using TMPro;

public enum EnemyClass
{
	Dunce, Brawler, Flanker, Auxillary, Assassin, Archer
}

public class Enemy : MonoBehaviour
{
    public EnemyHealthBar ehb;
    public EnemyDMGNumbers edn;

    private bool debug_messages = false;
    public TextMeshPro id_label;

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
    protected Vector3Int lastMove;

	// General Stats
	public EnemyClass enemyClass = EnemyClass.Dunce;
	public float movePercentage = .66f;
    public float evadePercentage = .33f;
    public float predictPercentage = .33f;
	public float specialAttackPercentage = .5f;

    // Other stats
    public int maxHp = 2;
    public int maxStrength = 1;
    public int minRange = 0;
    public int maxRange = 4;

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
    public int shortDiagonalShotDamage = 1;
    public int mediumDiagonalShotDamage = 1;
    public int longDiagonalShotDamage = 1;

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
    public int shortDiagonalShotHeatup = 2;
    public int mediumDiagonalShotHeatup = 2;
    public int longDiagonalShotHeatup = 2;

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
    protected int shortDiagonalShotCooldown = 0;
    protected int mediumDiagonalShotCooldown = 0;
    protected int longDiagonalShotCooldown = 0;

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
    public bool ShortDiagonalShot = false;
    public bool MediumDiagonalShot = false;
    public bool LongDiagonalShot = false;

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
        this.lastMove = new Vector3Int(-999, -999, -999);

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
            if (debug_messages)
        		Debug.Log("E#"+this.enemyId+" says AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAHHHHHHHHHHHHHHHHHHHHHHHHHHH");
    		// this.kill();
    	}
    	else
    	{
    		this.gridPosition = new Vector3Int(this.combatantEntry.gridX, this.combatantEntry.gridY, 0);
            
            if (this.id_label != null)
            {
                this.id_label.SetText("E#"+this.enemyId);
            }
    	}

        if (debug_messages)
        	Debug.Log("E#"+this.enemyId+" has initialized successfully!");
    }

    protected void generateID()
    {
    	this.enemyId = (int)(Random.value * 9999999);
    }

    //===========   Thinking methods   ===========//
    protected List<Point> getRestrictions(Point me, Point target)
    {
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

        foreach (CList c in BattleManager.Instance.combatantList)
        {
            if (c.gridX != target.X && c.gridY != target.Y)
            {
                offlimits.Add(new Point(c.gridX, c.gridY));
            }
        }

        return offlimits;
    }

    protected void move()
    {
        // Set our decision right away
        this.decision = 1;

        if (debug_messages)
            Debug.Log("E#" + this.enemyId + " is moving!");

        // Grab the target and the enemy's location from the BattleManager
        Point target = new Point(BattleManager.Instance.combatantList[0].gridX, BattleManager.Instance.combatantList[0].gridY);
        Point me = new Point(this.combatantEntry.gridX, this.combatantEntry.gridY);

        // Change our target position to move to if we're within minimum range, and if we're
        // within minimum range then try to evade
        int targetDist = BFS.bfsDist(BattleManager.Instance.gridCell, me, target);
        if (targetDist < this.minRange)
        {
            if (Random.value < this.evadePercentage)
            {
                if (debug_messages)
                {
                    Debug.Log("Attempting to move away");
                    Debug.Log("Original Target: ("+target.X+", "+target.Y+")");
                }

                // Try all sides around
                int northDist, southDist, eastDist, westDist;

                if (me.X + 1 < 0 || me.X + 1 > BattleManager.Instance.gridCell.GetLength(0))
                    northDist = -999;
                else
                    northDist = BFS.bfsDist(BattleManager.Instance.gridCell, new Point(me.X + 1, me.Y), target);

                if (me.X - 1 < 0 || me.X - 1 > BattleManager.Instance.gridCell.GetLength(0))
                    southDist = -999;
                else
                    southDist = BFS.bfsDist(BattleManager.Instance.gridCell, new Point(me.X - 1, me.Y), target);

                if (me.Y - 1 < 0 || me.Y - 1 > BattleManager.Instance.gridCell.GetLength(1))
                    eastDist = -999;
                else
                    eastDist = BFS.bfsDist(BattleManager.Instance.gridCell, new Point(me.X, me.Y - 1), target);

                if (me.Y + 1 < 0 || me.Y + 1 > BattleManager.Instance.gridCell.GetLength(1))
                    westDist = -999;
                else
                    westDist = BFS.bfsDist(BattleManager.Instance.gridCell, new Point(me.X, me.Y + 1), target);

                // Exclude any positions that already have an entity or are impassable
                if (northDist > 0)
                {
                    if (BattleManager.Instance.gridCell[me.X + 1, me.Y].entity != null || !BattleManager.Instance.gridCell[me.X + 1, me.Y].pass)
                        northDist = -999;
                }
                if (southDist > 0)
                {
                    if (BattleManager.Instance.gridCell[me.X - 1, me.Y].entity != null || !BattleManager.Instance.gridCell[me.X - 1, me.Y].pass)
                    southDist = -999;
                }
                if (westDist > 0)
                {
                    if (BattleManager.Instance.gridCell[me.X, me.Y + 1].entity != null || !BattleManager.Instance.gridCell[me.X, me.Y + 1].pass)
                        westDist = -999;
                }
                if (eastDist > 0)
                {
                    if (BattleManager.Instance.gridCell[me.X, me.Y - 1].entity != null || !BattleManager.Instance.gridCell[me.X, me.Y - 1].pass)
                        eastDist = -999;
                }

                if (debug_messages)
                {
                    Debug.Log("NDist: "+northDist);
                    Debug.Log("SDist: "+southDist);
                    Debug.Log("EDist: "+eastDist);
                    Debug.Log("WDist: "+westDist);
                }

                char dir = 'n';
                int longDist = northDist;

                if (longDist < southDist)
                {
                    longDist = southDist;
                    dir = 's';
                }
                if (longDist < eastDist)
                {
                    longDist = eastDist;
                    dir = 'e';
                }
                if (longDist < westDist)
                {
                    longDist = westDist;
                    dir = 'w';
                }

                // Attack if we have no other option
                if (longDist <= 0)
                {
                    this.attack();
                    return;
                }

                // Pick our new target
                switch (dir)
                {
                    default:
                    case 'n':
                        target.X = me.X + 1;
                        target.Y = me.Y;
                        break;
                    case 's':
                        target.X = me.X - 1;
                        target.Y = me.Y;
                        break;
                    case 'e':
                        target.X = me.X;
                        target.Y = me.Y - 1;
                        break;
                    case 'w':
                        target.X = me.X;
                        target.Y = me.Y + 1;
                        break;
                }

                if (debug_messages)
                    Debug.Log("New Target: ("+target.X+", "+target.Y+")");
            }
            else
            {
                // We've failed to evade, so fight!
                this.attack();
                return;
            }
        }

        List<Point> offlimits = new List<Point>();

        // Check if we got where we were going last time
        if (me.X != lastMove.x || me.Y != lastMove.y)
        {
            offlimits.Add(new Point(lastMove.x, lastMove.y));
        }

        // Add in the positions of all current enemies
        foreach (CList c in BattleManager.Instance.combatantList)
        {
            if (c.gridX != target.X && c.gridY != target.Y)
                offlimits.Add(new Point(c.gridX, c.gridY));
        }

        // Pathfind!
        Point nextSpot = BFS.bfs(BattleManager.Instance.gridCell, me, target, offlimits);

        // If our initial pathfind didn't find anything...
        if (nextSpot.X < 0 || nextSpot.Y < 0)
        {
            if (debug_messages)
                Debug.Log("E#"+this.enemyId+" failed to find a path!");

            // Try every position around the target
            for (int xx = -1; xx < 2; xx++)
            {
                for (int yy = -1; yy < 2; yy++)
                {
                    Point tempTarget = new Point(target.X + xx, target.Y + yy);

                    nextSpot = BFS.bfs(BattleManager.Instance.gridCell, me, tempTarget, offlimits);

                    if (debug_messages)
                    {
                        Debug.Log("At tempTarget ("+tempTarget.X+", "+tempTarget.Y+") we found: ("+nextSpot.X+", "+nextSpot.Y+")");
                    }

                    if (nextSpot.X >= 0 && nextSpot.Y >= 0 && nextSpot.X < BattleManager.Instance.gridCell.GetLength(0) && nextSpot.Y < BattleManager.Instance.gridCell.GetLength(1))
                    {
                        break;
                    }
                }
            }

            if (debug_messages)
                Debug.Log("E#"+this.enemyId+" has tried every position and found P("+nextSpot.X+", "+nextSpot.Y+")");
        }

        // If we still haven't found something, we need to try every position around us so that we move somewhere
        if (nextSpot.X < 0 || nextSpot.Y < 0)
        {
            if (debug_messages)
                Debug.Log("E#"+this.enemyId+" failed to find a path again!");

            for (int xx = -1; xx < 2; xx++)
            {
                for (int yy = -1; yy < 2; yy++)
                {
                    Point tempTarget = new Point(me.X + xx, me.Y + yy);

                    nextSpot = BFS.bfs(BattleManager.Instance.gridCell, me, tempTarget, offlimits);

                    if (debug_messages)
                    {
                        Debug.Log("At tempTarget ("+tempTarget.X+", "+tempTarget.Y+") we found: ("+nextSpot.X+", "+nextSpot.Y+")");
                    }

                    if (nextSpot.X >= 0 && nextSpot.Y >= 0 && nextSpot.X < BattleManager.Instance.gridCell.GetLength(0) && nextSpot.Y < BattleManager.Instance.gridCell.GetLength(1))
                    {
                        break;
                    }
                }
            }

            if (debug_messages)
                Debug.Log("E#"+this.enemyId+" has tried every position AGAIN and found P("+nextSpot.X+", "+nextSpot.Y+")");
        }

        // If that STILL doesn't work, give up
        if (nextSpot.X < 0 || nextSpot.Y < 0)
        {
            if (true)
            {
                Debug.Log("E#"+this.enemyId+" has tried everything and cannot move at all!");
            }
        }

        if (debug_messages)
        {
            Debug.Log("GridCell X-Length: " + BattleManager.Instance.gridCell.GetLength(0));
            Debug.Log("GridCell Y-Length: " + BattleManager.Instance.gridCell.GetLength(1));
            Debug.Log("Chosen Next Point: (" + nextSpot.X + ", " + nextSpot.Y + ")");
        }

        // Don't move if we accidentally chose a spot out of bounds, otherwise move to our next spot
        if (nextSpot.X < 0 || nextSpot.X >= BattleManager.Instance.gridCell.GetLength(0) || nextSpot.Y < 0 || nextSpot.Y >= BattleManager.Instance.gridCell.GetLength(1))
        { 
            if (debug_messages)
                Debug.Log("entered failsafe");

            this.lastMove = new Vector3Int(me.X, me.Y, 0);
            this.setMove(BattleManager.Instance.gridCell[me.X, me.Y].center);
        }
        else
        {
            if (debug_messages)
                Debug.Log("NOT FAILSAFE");

            this.lastMove = new Vector3Int(nextSpot.X, nextSpot.Y, 0);
	        this.setMove(BattleManager.Instance.gridCell[nextSpot.X, nextSpot.Y].center);
        }
    }

    protected void attack()
    {
        // Set our decision right away
        this.decision = 2;

        if (debug_messages)
            Debug.Log("E#" + this.enemyId + " is attacking!");

    	// Initialize our list of potential attacks, and our final decision
    	List<List<Point>> attackTiles = new List<List<Point>>();
    	List<Point> decidedTile = new List<Point>();

    	// Grab information from the BattleManager
    	Point me = new Point(this.combatantEntry.gridX, this.combatantEntry.gridY);
    	Point target = new Point(BattleManager.Instance.combatantList[0].gridX, BattleManager.Instance.combatantList[0].gridY);

        if (Random.value < this.predictPercentage)
        {
            int count = 5;

            if (debug_messages)
                Debug.Log("Predicting...");

            do {
                target = new Point(BattleManager.Instance.combatantList[0].gridX, BattleManager.Instance.combatantList[0].gridY);

                if (Random.value < .5f)
                {
                    if (Random.value < .5f)
                        target.X++;
                    else
                        target.X--;
                }
                else
                {
                    if (Random.value < .5f)
                        target.Y++;
                    else
                        target.Y--;
                }
                count--;
            }
            while (!BattleManager.Instance.gridCell[target.X, target.Y].pass || count > 0);

            // If for some reason we keep picking a bush, pick the player instead you asshole
            if (!BattleManager.Instance.gridCell[target.X, target.Y].pass)
            {
                if (debug_messages)
                    Debug.Log("Prediction failed; picking target position instead");

                target = new Point(BattleManager.Instance.combatantList[0].gridX, BattleManager.Instance.combatantList[0].gridY);
            }
        }

    	// Melee Attacks
        bool attackSelected = false;
        bool specialAttack = false;

        if (Random.value < this.specialAttackPercentage)
        {
            specialAttack = true;
        }


        // Point nextSpot = BFS.bfs(BattleManager.Instance.gridCell, me, target, getRestrictions(me, target));

        // First, go through every available attack, and if we have one that would hit the player we will select that one right away
        // Coinflip each special so that we can try and get to later ones as well
        if (specialAttack)
        {
            //==========   Melee Attacks   ===========//
            if (this.Cleave && this.cleaveCooldown <= 0 && !attackSelected && Random.value < .5f)
            {
                attackTiles = this.getCleaveList(me);

                foreach (List<Point> l in attackTiles)
                {
                    if (l.Contains(target))
                    {
                        decidedTile = l;
                        this.damage = this.cleaveDamage;
                        this.cleaveCooldown = this.cleaveHeatup + 1;
                        attackSelected = true;
                    }
                }
            }

            if (this.Whammy && this.whammyCooldown <= 0 && !attackSelected && Random.value < .5f)
            {
                attackTiles = this.getWhammyList(me);

                foreach (List<Point> l in attackTiles)
                {
                    if (l.Contains(target))
                    {
                        decidedTile = l;
                        this.damage = this.whammyDamage;
                        this.whammyCooldown = this.whammyHeatup + 1;
                        attackSelected = true;
                    }
                }
            }

            if (this.Thrust && this.thrustCooldown <= 0 && !attackSelected && Random.value < .5f)
            {
                attackTiles = this.getThrustList(me);

                foreach (List<Point> l in attackTiles)
                {
                    if (l.Contains(target))
                    {
                        decidedTile = l;
                        this.damage = this.thrustDamage;
                        this.thrustCooldown = this.thrustHeatup + 1;
                        attackSelected = true;
                    }
                }
            }
            
            if (this.Slice && this.sliceCooldown <= 0 && !attackSelected && Random.value < .5f)
            {
                attackTiles = this.getSliceList(me);

                foreach (List<Point> l in attackTiles)
                {
                    if (l.Contains(target))
                    {
                        decidedTile = l;
                        this.damage = this.sliceDamage;
                        this.sliceCooldown = this.sliceHeatup + 1;
                        attackSelected = true;
                    }
                }
            }
            
            if (this.Cyclone && this.cycloneCooldown <= 0 && !attackSelected && Random.value < .5f)
            {
                attackTiles = this.getCycloneList(me);

                foreach (List<Point> l in attackTiles)
                {
                    if (l.Contains(target))
                    {
                        decidedTile = l;
                        this.damage = this.cycloneDamage;
                        this.cycloneCooldown = this.cycloneHeatup + 1;
                        attackSelected = true;
                    }
                }
            }
            
            if (this.CycleKick && this.cycleKickCooldown <= 0 && !attackSelected && Random.value < .5f)
            {
                attackTiles = this.getCycleKickList(me);

                foreach (List<Point> l in attackTiles)
                {
                    if (l.Contains(target))
                    {
                        decidedTile = l;
                        this.damage = this.cycleKickDamage;
                        this.cycleKickCooldown = this.cycleKickHeatup + 1;
                        attackSelected = true;
                    }
                }
            }
            
            if (this.CyclePunch && this.cyclePunchCooldown <= 0 && !attackSelected && Random.value < .5f)
            {
                attackTiles = this.getCyclePunchList(me);

                foreach (List<Point> l in attackTiles)
                {
                    if (l.Contains(target))
                    {
                        decidedTile = l;
                        this.damage = this.cyclePunchDamage;
                        this.cyclePunchCooldown = this.cyclePunchHeatup + 1;
                        attackSelected = true;
                    }
                }
            }

            //===========   Ranged Attacks   ============//
            if (this.Shortshot && this.shortshotCooldown <= 0 && !attackSelected && Random.value < .5f)
            {
                attackTiles = this.getShortshotList(me);

                foreach (List<Point> l in attackTiles)
                {
                    if (l.Contains(target))
                    {
                        decidedTile = l;
                        this.damage = this.shortshotDamage;
                        this.shortshotCooldown = this.shortshotHeatup + 1;
                        attackSelected = true;
                    }
                }
            }
            
            if (this.Mediumshot && this.mediumshotCooldown <= 0 && !attackSelected && Random.value < .5f)
            {
                attackTiles = this.getMediumshotList(me);

                foreach (List<Point> l in attackTiles)
                {
                    if (l.Contains(target))
                    {
                        decidedTile = l;
                        this.damage = this.mediumshotDamage;
                        this.mediumshotCooldown = this.mediumshotHeatup + 1;
                        attackSelected = true;
                    }
                }
            }
            
            if (this.Longshot && this.longshotCooldown <= 0 && !attackSelected && Random.value < .5f)
            {
                attackTiles = this.getLongshotList(me);

                foreach (List<Point> l in attackTiles)
                {
                    if (l.Contains(target))
                    {
                        decidedTile = l;
                        this.damage = this.longshotDamage;
                        this.longshotCooldown = this.longshotHeatup + 1;
                        attackSelected = true;
                    }
                }
            }
            
            if (this.ShortRain && this.shortRainCooldown <= 0 && !attackSelected && Random.value < .5f)
            {
                attackTiles = this.getShortRainList(me);

                foreach (List<Point> l in attackTiles)
                {
                    if (l.Contains(target))
                    {
                        decidedTile = l;
                        this.damage = this.shortRainDamage;
                        this.shortRainCooldown = this.shortRainHeatup + 1;
                        attackSelected = true;
                    }
                }
            }
            
            if (this.MediumRain && this.mediumRainCooldown <= 0 && !attackSelected && Random.value < .5f)
            {
                attackTiles = this.getMediumRainList(me);

                foreach (List<Point> l in attackTiles)
                {
                    if (l.Contains(target))
                    {
                        decidedTile = l;
                        this.damage = this.mediumRainDamage;
                        this.mediumRainCooldown = this.mediumRainHeatup + 1;
                        attackSelected = true;
                    }
                }
            }
            
            if (this.LongRain && this.longRainCooldown <= 0 && !attackSelected && Random.value < .5f)
            {
                attackTiles = this.getLongRainList(me);

                foreach (List<Point> l in attackTiles)
                {
                    if (l.Contains(target))
                    {
                        decidedTile = l;
                        this.damage = this.longRainDamage;
                        this.longRainCooldown = this.longRainHeatup + 1;
                        attackSelected = true;
                    }
                }
            }

            if (this.ShortDiagonalShot && this.shortDiagonalShotCooldown <= 0 && !attackSelected && Random.value < .5f)
            {
                attackTiles = this.getShortDiagonalShotList(me);

                foreach (List<Point> l in attackTiles)
                {
                    if (l.Contains(target))
                    {
                        decidedTile = l;
                        this.damage = this.shortDiagonalShotDamage;
                        this.shortDiagonalShotCooldown = this.shortDiagonalShotHeatup + 1;
                        attackSelected = true;
                    }
                }
            }

            if (this.MediumDiagonalShot && this.mediumDiagonalShotCooldown <= 0 && !attackSelected && Random.value < .5f)
            {
                attackTiles = this.getMediumDiagonalShotList(me);

                foreach (List<Point> l in attackTiles)
                {
                    if (l.Contains(target))
                    {
                        decidedTile = l;
                        this.damage = this.mediumDiagonalShotDamage;
                        this.mediumDiagonalShotCooldown = this.mediumDiagonalShotHeatup + 1;
                        attackSelected = true;
                    }
                }
            }

            if (this.LongDiagonalShot && this.longDiagonalShotCooldown <= 0 && !attackSelected && Random.value < .5f)
            {
                attackTiles = this.getLongDiagonalShotList(me);

                foreach (List<Point> l in attackTiles)
                {
                    if (l.Contains(target))
                    {
                        decidedTile = l;
                        this.damage = this.longDiagonalShotDamage;
                        this.longDiagonalShotCooldown = this.longDiagonalShotHeatup + 1;
                        attackSelected = true;
                    }
                }
            }

            // Default to normal attack if we didn't pick anything
            if (!attackSelected)
            {
                attackTiles = this.getStrikeList(me);

                foreach (List<Point> l in attackTiles)
                {
                    if (l.Contains(target))
                    {
                        decidedTile = l;
                        this.damage = this.strikeDamage;
                        attackSelected = true;
                    }
                }

                // If we couldn't pick an attack, move instead
                if (!attackSelected)
                {
                    this.move();
                    return;
                }
            }
        }
        else
        {
            // Our default attack since we chose not to do a special attack
            attackTiles = this.getStrikeList(me);

            foreach (List<Point> l in attackTiles)
            {
                if (l.Contains(target))
                {
                    decidedTile = l;
                    this.damage = this.strikeDamage;
                    attackSelected = true;
                }
            }

            // If we couldn't pick an attack, move instead
            if (!attackSelected)
            {
                if (debug_messages)
                    Debug.Log("Failed to pick an attack...moving instead");

                this.move();
                return;
            }
        }

        // If for some reason we couldn't choose a default tile, pick one at random
        if (decidedTile.Count < 1)
        {
            if (debug_messages)
                Debug.Log("Failed to have any tiles to attack...moving instead");

            this.move();
            return;
        }

    	// Convert to vector positons
    	List<Vector3> temp = new List<Vector3>();

    	foreach (Point p in decidedTile)
    	{
    		if (p.X >= 0 && p.X < BattleManager.Instance.gridCell.GetLength(0) && p.Y >= 0 && p.Y < BattleManager.Instance.gridCell.GetLength(1))
    		{
                if (BattleManager.Instance.gridCell[p.X, p.Y].pass)
                {
    	    		temp.Add(BattleManager.Instance.gridCell[p.X, p.Y].center);
                }
    		}
    	}

    	// Set attacks to be ready for export
    	this.setAttacks(temp);
    }

    public void decide()
    {
        if (debug_messages)
        	Debug.Log("E#"+this.enemyId+" has been called upon to think!");

        Point target = new Point(BattleManager.Instance.combatantList[0].gridX, BattleManager.Instance.combatantList[0].gridY);
        Point me = new Point(this.combatantEntry.gridX, this.combatantEntry.gridY);

        int targetDist = BFS.bfsDist(BattleManager.Instance.gridCell, me, target);
        if (targetDist > this.maxRange || targetDist < this.minRange)
        {
            this.move();
        }
        else
        {
            if (Random.value < this.movePercentage)
            {
                this.move();
            }
            else
            {
                this.attack();
            }
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
        if (debug_messages)
        	Debug.Log("Oy blyat! E#"+this.enemyId+" am dead");

    	Destroy(this);
    }

    public void dealDamage(int dam)
    {
    	this.health -= dam;
        this.combatantEntry.hp = this.health;
        this.ehb.updateBar(this.health);
        if (this.health > 0)
            this.edn.gimmeDemNumbers(dam);
        if (this.health <= 0)
        {
            if (debug_messages)
                Debug.Log("I ded @" + this.combatantEntry.entity.transform.position);
        }
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

    	// Brawler doesn't care at all

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
        int choice = (int)(Random.value * 4);

        switch (choice)
        {
            default:
            case 1:
                list.Add(new Point(target.X + 1, target.Y - 1));
                list.Add(new Point(target.X + 1, target.Y));
                list.Add(new Point(target.X + 1, target.Y + 1));
                break;
            case 2:
                list.Add(new Point(target.X - 1, target.Y - 1));
                list.Add(new Point(target.X - 1, target.Y));
                list.Add(new Point(target.X - 1, target.Y + 1));
                break;
            case 3:
                list.Add(new Point(target.X - 1, target.Y + 1));
                list.Add(new Point(target.X, target.Y + 1));
                list.Add(new Point(target.X + 1, target.Y + 1));
                break;
            case 4:
                list.Add(new Point(target.X - 1, target.Y - 1));
                list.Add(new Point(target.X, target.Y - 1));
                list.Add(new Point(target.X + 1, target.Y - 1));
                break;
        }

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

    protected List<List<Point>> getShortDiagonalShotList(Point me)
    {
        List<List<Point>> list = new List<List<Point>>();

        list.Add(new List<Point>());
        list[0].Add(new Point(me.X + 2, me.Y - 1));
        list[0].Add(new Point(me.X + 2, me.Y + 1));

        list.Add(new List<Point>());
        list[1].Add(new Point(me.X - 2, me.Y + 1));
        list[1].Add(new Point(me.X - 2, me.Y - 1));

        list.Add(new List<Point>());
        list[2].Add(new Point(me.X + 1, me.Y + 2));
        list[2].Add(new Point(me.X - 1, me.Y + 2));

        list.Add(new List<Point>());
        list[3].Add(new Point(me.X + 1, me.Y - 2));
        list[3].Add(new Point(me.X - 1, me.Y - 2));

        return list;
    }

    protected List<List<Point>> getMediumDiagonalShotList(Point me)
    {
        List<List<Point>> list = new List<List<Point>>();

        list.Add(new List<Point>());
        list[0].Add(new Point(me.X + 3, me.Y - 1));
        list[0].Add(new Point(me.X + 3, me.Y + 1));

        list.Add(new List<Point>());
        list[1].Add(new Point(me.X - 3, me.Y + 1));
        list[1].Add(new Point(me.X - 3, me.Y - 1));

        list.Add(new List<Point>());
        list[2].Add(new Point(me.X + 1, me.Y + 3));
        list[2].Add(new Point(me.X - 1, me.Y + 3));

        list.Add(new List<Point>());
        list[3].Add(new Point(me.X + 1, me.Y - 3));
        list[3].Add(new Point(me.X - 1, me.Y - 3));

        return list;
    }

    protected List<List<Point>> getLongDiagonalShotList(Point me)
    {
        List<List<Point>> list = new List<List<Point>>();

        list.Add(new List<Point>());
        list[0].Add(new Point(me.X + 4, me.Y - 1));
        list[0].Add(new Point(me.X + 4, me.Y + 1));

        list.Add(new List<Point>());
        list[1].Add(new Point(me.X - 4, me.Y + 1));
        list[1].Add(new Point(me.X - 4, me.Y - 1));

        list.Add(new List<Point>());
        list[2].Add(new Point(me.X + 1, me.Y + 4));
        list[2].Add(new Point(me.X - 1, me.Y + 4));

        list.Add(new List<Point>());
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
    public bool isDead() { return (this.health <= 0); }
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