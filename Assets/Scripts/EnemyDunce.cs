using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    	// Grab the target from the grid
    	int targetGridX = this.bm.getPlayerPosition().x;
    	int targetGridY = this.bm.getPlayerPosition().y;

    	// Score positions based on what is closest to the target
    	// this.moveNorth = targetGridY - this.combatantEntry.gridY;
    	// this.moveNorth = this.combatantEntry.gridY - targetGridY;
    	// if (targetGridY < this.combatantEntry.gridY)
    	// 	this.moveNorth = targetGridY - this.combatantEntry.gridY;
    	// else
    	// 	this.moveNorth = this.combatantEntry.gridY - targetGridY;
    	// this.moveNorth = Mathf.Abs(targetGridX - this.combatantEntry.gridX) + Mathf.Abs(targetGridY - (this.combatantEntry.gridY - 1));
    	this.moveNorth = Mathf.Abs(targetGridX - this.combatantEntry.gridX + 1) + Mathf.Abs(targetGridY - this.combatantEntry.gridY);
    	if (targetGridX < this.combatantEntry.gridX)
    		this.moveNorth += this.combatantEntry.gridX - targetGridX;
    	else
    		this.moveNorth -= targetGridX - this.combatantEntry.gridX;
    	// if (this.moveNorth == 0)
    		// this.moveNorth = 99999;

    	// this.moveSouth = this.combatantEntry.gridY - targetGridY;
    	// this.moveSouth = targetGridY - this.combatantEntry.gridY;
    	// if (targetGridY > this.combatantEntry.gridY)
    	// 	this.moveSouth = targetGridY - this.combatantEntry.gridY;
    	// else
    	// 	this.moveSouth = this.combatantEntry.gridY - targetGridY;
    	// this.moveSouth = Mathf.Abs(targetGridX - this.combatantEntry.gridX) + Mathf.Abs(targetGridY - (this.combatantEntry.gridY + 1));
    	this.moveSouth = Mathf.Abs(targetGridX - (this.combatantEntry.gridX - 1)) + Mathf.Abs(targetGridY - this.combatantEntry.gridY);
    	if (targetGridX > this.combatantEntry.gridX)
    		this.moveSouth += targetGridX - this.combatantEntry.gridX;
    	else
    		this.moveSouth -= this.combatantEntry.gridX - targetGridX;
    	// if (this.moveSouth == 0)
    		// this.moveSouth = 99999;

    	// this.moveEast = targetGridX - this.combatantEntry.gridX;
    	// this.moveEast = this.combatantEntry.gridX - targetGridX;
    	// if (targetGridX > this.combatantEntry.gridX)
    	// 	this.moveEast = targetGridX - this.combatantEntry.gridX;
    	// else
    	// 	this.moveEast = this.combatantEntry.gridX - targetGridX;
    	// this.moveEast = Mathf.Abs(targetGridX - (this.combatantEntry.gridX + 1)) + Mathf.Abs(targetGridY - this.combatantEntry.gridY);
    	this.moveEast = Mathf.Abs(targetGridX - this.combatantEntry.gridX) + Mathf.Abs(targetGridY - this.combatantEntry.gridY - 1);
    	if (targetGridY > this.combatantEntry.gridY)
    		this.moveEast += targetGridY - this.combatantEntry.gridY;
    	else
    		this.moveEast -= this.combatantEntry.gridY - targetGridY;
    	// if (this.moveEast == 0)
    		// this.moveEast = 99999;

    	// this.moveWest = this.combatantEntry.gridX - targetGridX;
    	// this.moveWest = targetGridX - this.combatantEntry.gridX;
    	// if (targetGridX < this.combatantEntry.gridX)
    	// 	this.moveWest = targetGridX - this.combatantEntry.gridX;
    	// else
    	// 	this.moveWest = this.combatantEntry.gridX - targetGridX;
    	// this.moveWest = Mathf.Abs(targetGridX - (this.combatantEntry.gridX - 1)) + Mathf.Abs(targetGridY - this.combatantEntry.gridY);
    	this.moveWest = Mathf.Abs(targetGridX - this.combatantEntry.gridX) + Mathf.Abs(targetGridY - this.combatantEntry.gridY + 1);
    	if (targetGridY < this.combatantEntry.gridY)
    		this.moveWest += this.combatantEntry.gridY - targetGridY;
    	else
    		this.moveWest -= targetGridY - this.combatantEntry.gridY;
    		// this.moveWest += targetGridY - this.combatantEntry.gridY;
    	// if (this.moveWest == 0)
    		// this.moveWest = 99999;

    	// Invalidate any positions that are impassable or at the bounds
    	if (this.combatantEntry.gridX == 0)
    		this.moveNorth = 99999;
    	if (this.combatantEntry.gridX == this.bm.gridCell.GetLength(1)) // Columns
    		this.moveSouth = 99999;
    	if (this.combatantEntry.gridY == 0)
    		this.moveEast = 99999;
    	if (this.combatantEntry.gridY == this.bm.gridCell.GetLength(0)) // Rows
    		this.moveWest = 99999;

    	if (!this.bm.gridCell[this.combatantEntry.gridX, this.combatantEntry.gridY + 1].pass)
	    	this.moveWest = 99999;
	    if (!this.bm.gridCell[this.combatantEntry.gridX, this.combatantEntry.gridY - 1].pass)
	    	this.moveEast = 99999;

	    if (!this.bm.gridCell[this.combatantEntry.gridX - 1, this.combatantEntry.gridY].pass)
	    	this.moveSouth = 99999;
	    if (!this.bm.gridCell[this.combatantEntry.gridX + 1, this.combatantEntry.gridY].pass)
	    	this.moveNorth = 99999;

    	/*
    	if (this.combatantEntry.gridY > 0 && this.combatantEntry.gridY < this.bm.gridCell.GetLength(0))
    	{
	    	if (!this.bm.gridCell[this.combatantEntry.gridX, this.combatantEntry.gridY - 1].pass)
	    		this.moveNorth = 99999;
	    	if (!this.bm.gridCell[this.combatantEntry.gridX, this.combatantEntry.gridY + 1].pass)
	    		this.moveSouth = 99999;
    	}
    	if (this.combatantEntry.gridX > 0 && this.combatantEntry.gridX < this.bm.gridCell.GetLength(1))
    	{
	    	if (!this.bm.gridCell[this.combatantEntry.gridX + 1, this.combatantEntry.gridY].pass)
	    		this.moveEast = 99999;
	    	if (!this.bm.gridCell[this.combatantEntry.gridX - 1, this.combatantEntry.gridY].pass)
	    		this.moveWest = 99999;
    	}
    	*/

    	// Update the lowest scoring position
    	this.lowestMoveScore = this.moveNorth;
    	if (this.moveSouth < this.lowestMoveScore)
    		this.lowestMoveScore = this.moveSouth;
    	if (this.moveEast < this.lowestMoveScore)
    		this.lowestMoveScore = this.moveEast;
    	if (this.moveWest < this.lowestMoveScore)
    		this.lowestMoveScore = this.moveWest;

    	Debug.Log("Our movement scores for E#"+this.enemyId+" are: N->"+this.moveNorth+", S->"+this.moveSouth+", E->"+this.moveEast+", W->"+this.moveWest);

    	Debug.Log("E#"+this.enemyId+" is moving!");
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

    	Debug.Log("E#"+this.enemyId+" is at world position ("+this.combatantEntry.entity.transform.position.x+", "+this.combatantEntry.entity.transform.position.y+", "+this.combatantEntry.entity.transform.position.z+")");
    	Debug.Log("The first postiion in the gridCell array is at world position ("+this.bm.gridCell[0,0].center.x+", "+this.bm.gridCell[0,0].center.y+", "+this.bm.gridCell[0,0].center.z+")");
    	Debug.Log("The player is at position ("+this.bm.getPlayerPosition().x+", "+this.bm.getPlayerPosition().y+", "+this.bm.getPlayerPosition().z+")");

    	this.move();
    	this.attack();

    	char decision = 'm';

    	this.lowestDecisionScore = this.lowestMoveScore;
    	// Uncomment this once attacks are implemented
    	// if (this.lowestAttackScore < this.lowestDecisionScore)
    	// {
    	// 	decision = 'a';
    	// 	this.lowestDecisionScore = this.lowestAttackScore;
    	// }
    	// if (this.lowestCleaveScore < this.lowestDecisionScore)
    	// {
    	// 	decision = 'c';
    	// 	this.lowestDecisionScore = this.lowestCleaveScore;
    	// }
    	// if (this.lowestCornerScore < this.lowestDecisionScore)
    	// {
    	// 	decision = 'd';
    	// 	this.lowestDecisionScore = this.lowestCornerScore;
    	// }
    	// if (this.lowestOneAwayScore < this.lowestDecisionScore)
    	// {
    	// 	decision = '1';
    	// 	this.lowestDecisionScore = this.lowestOneAwayScore;
    	// }
    	// if (this.lowestTwoAwayScore < this.lowestDecisionScore)
    	// {
    	// 	decision = '2';
    	// 	this.lowestDecisionScore = this.lowestTwoAwayScore;
    	// }
    	// if (this.lowestThreeAwayScore < this.lowestDecisionScore)
    	// {
    	// 	decision = '3';
    	// 	this.lowestDecisionScore = this.lowestThreeAwayScore;
    	// }

    	if (this.lowestDecisionScore >= 999)
    		decision = '!';

    	int lowest = 0;
    	char direction = 'n';
    	switch (decision)
    	{
    		case 'm': // Move
    			// Sort through again and keep track of the direction
    			lowest = this.moveNorth;
    			direction = 'n';
    			if (this.moveSouth < lowest)
    			{
    				direction = 's';
    				lowest = this.moveSouth;
    			}
    			if (this.moveEast < lowest)
    			{
    				direction = 'e';
    				lowest = this.moveEast;
    			}
    			if (this.moveWest < lowest)
    			{
    				direction = 'w';
    				lowest = this.moveWest;
    			}

    			// Actually move
    			switch (direction)
    			{
    				case 'n': this.moveUp(); break;
    				case 's': this.moveDown(); break;
    				case 'e': this.moveRight(); break;
    				case 'w': this.moveLeft(); break;
    				default: break;
    			}

    			Debug.Log("E#"+this.enemyId+" has chosen "+direction);
    			break;
    		case 'a': // Standard Attack
    			break;
    		case 'c': // Cleave
    			break;
    		case 'd': // Corner Attack (Diagonal)
    			break;
    		case '1': // One-Away Attack
    			break;
    		case '2': // Two-Away Attack
    			break;
    		case '3': // Three-Away Attack
    			break;
    		default: // Some type of error
    			Debug.Log("E#"+this.enemyId+" has had an error or chosen not to move");
    			break;
    	}
    }

}
