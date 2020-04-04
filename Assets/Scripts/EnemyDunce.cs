using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;

public class EnemyDunce : Enemy
{
	public EnemyDunce()
	{
		this.combatantEntry = null;
	}

	public EnemyDunce(CList c)
	{
		this.combatantEntry = c;
		// this.initialize();
	}

    // Start is called before the first frame update
    void Start()
    {
        this.initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //=============   You Did This To Me David   =============//
    protected override void initialize()
    {
    	// Set type
    	this.enemyRole = 'd';

    	// Set default stats
    	this.health = 2;
    	this.damage = 1;
    	this.strength = 1;

    	// Set default attack damage
    	this.standardDamage = 1;
    	this.cleaveDamage = 2;
    	this.thrustDamage = 0;
    	this.doubleCornerDamage = 0;
    	this.oneAwayDamage = 0;
    	this.twoAwayDamage = 0;
    	this.threeAwayDamage = 0;

    	// Set initial positions
    	this.gridPosition = new Vector3Int(0, 0, 0);
    	this.attackTarget = new Vector3(0, 0, 0);
    	this.moveTarget = new Vector3(0, 0, 0);

    	// Make move scores
    	this.moveNorth = 0;
		this.moveSouth = 0;
		this.moveEast = 0;
		this.moveWest = 0;

		this.attackNorth = 0;
		this.attackSouth = 0;
		this.attackEast = 0;
		this.attackWest = 0;

		this.cleaveNorth = 0;
		this.cleaveSouth = 0;
		this.cleaveEast = 0;
		this.cleaveWest = 0;

		this.thrustNorth = 0;
		this.thrustSouth = 0;
		this.thrustEast = 0;
		this.thrustWest = 0;

		this.doubleCornerNorth = 0;
		this.doubleCornerSouth = 0;
		this.doubleCornerEast = 0;
		this.doubleCornerWest = 0;

		this.oneAwayNorth = 0;
		this.oneAwaySouth = 0;
		this.oneAwayEast = 0;
		this.oneAwayWest = 0;

		this.twoAwayNorth = 0;
		this.twoAwaySouth = 0;
		this.twoAwayEast = 0;
		this.twoAwayWest = 0;

		this.threeAwayNorth = 0;
		this.threeAwaySouth = 0;
		this.threeAwayEast = 0;
		this.threeAwayWest = 0;

		this.lowestMoveScore = 0;
		this.lowestAttackScore = 0;
		this.lowestCleaveScore = 0;
		this.lowestThrustScore = 0;
		this.lowestCornerScore = 0;
		this.lowestOneAwayScore = 0;
		this.lowestTwoAwayScore = 0;
		this.lowestThreeAwayScore = 0;

		this.lowestDecisionScore = 0;

    	// Generate ID
    	this.generateID();

    	if (this.combatantEntry == null)
    	{
    		Debug.Log("E#"+this.enemyId+" says AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAHHHHHHHHHHHHHHHHHHHHHHHHHHH");
    		this.kill();
    	}
    	else
    	{
    		this.gridPosition = new Vector3Int(this.combatantEntry.gridX, this.combatantEntry.gridY, 0);
    	}
    }

    protected override void move()
    {
    	Debug.Log("E#"+this.enemyId+" is moving!");
        Point target = new Point(BattleManager.Instance.combatantList[0].gridX, BattleManager.Instance.combatantList[0].gridY);
        Point me = new Point(this.combatantEntry.gridX, this.combatantEntry.gridY);
        Point nextSpot = BFS.bfs(BattleManager.Instance.gridCell, me, target);
        this.setMove(BattleManager.Instance.gridCell[nextSpot.X, nextSpot.Y].center);
    }

    protected override void attack()
    {
    	// Grab the target from the grid

    	// Score Positions based on what is closest to the target
	    	// Standard Attack

	    	// Cleave Attack

	    	// Thrust Attack

	    	// Corner Attack

	    	// One Away Attack

	    	// Two Away Attack

	    	// Three Away Attack

    	// Invalidate any positions that are impassable (for multi-position attacks, that invalidates the whole attack)
    		// Standard Attack

	    	// Cleave Attack

	    	// Thrust Attack

	    	// Corner Attack

	    	// One Away Attack

	    	// Two Away Attack

	    	// Three Away Attack

    	// Update all lowest scores
    	Debug.Log("E#"+this.enemyId+" is attacking!");
    }

    public override void decide()
    {
    	Debug.Log("E#"+this.enemyId+" has been called upon to think!");

        this.move();

    	//if (Random.value < .5f)
    	//	this.move();
    	//else
    	//	this.attack();
    }

}
