using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
	protected BattleManager bm;
	protected int enemyId;
	protected char enemyRole;
	protected int health;
	protected int damage;
	protected int strength;
	protected Vector3Int gridPosition;
	protected Vector3 moveTarget;
	protected Vector3 attackTarget;
	protected int decision; // 0 - no decision, 1 - move, 2 - attack
	protected CList combatantEntry;

	protected int standardDamage;
	protected int cleaveDamage;
	protected int thrustDamage;
	protected int doubleCornerDamage;
	protected int oneAwayDamage;
	protected int twoAwayDamage;
	protected int threeAwayDamage;

	protected int moveNorth;
	protected int moveSouth;
	protected int moveEast;
	protected int moveWest;

	protected int attackNorth;
	protected int attackSouth;
	protected int attackEast;
	protected int attackWest;

	protected int cleaveNorth;
	protected int cleaveSouth;
	protected int cleaveEast;
	protected int cleaveWest;

	protected int thrustNorth;
	protected int thrustSouth;
	protected int thrustEast;
	protected int thrustWest;

	protected int doubleCornerNorth;
	protected int doubleCornerSouth;
	protected int doubleCornerEast;
	protected int doubleCornerWest;

	protected int oneAwayNorth;
	protected int oneAwaySouth;
	protected int oneAwayEast;
	protected int oneAwayWest;

	protected int twoAwayNorth;
	protected int twoAwaySouth;
	protected int twoAwayEast;
	protected int twoAwayWest;

	protected int threeAwayNorth;
	protected int threeAwaySouth;
	protected int threeAwayEast;
	protected int threeAwayWest;

	protected int lowestMoveScore;
	protected int lowestAttackScore;
	protected int lowestCleaveScore;
	protected int lowestThrustScore;
	protected int lowestCornerScore;
	protected int lowestOneAwayScore;
	protected int lowestTwoAwayScore;
	protected int lowestThreeAwayScore;

	protected int lowestDecisionScore;

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
    protected abstract void initialize();

    protected void generateID()
    {
    	this.enemyId = (int)(Random.value * 9999999);
    }

    //===========   Thinking methods   ===========//
    protected abstract void move();
    protected abstract void attack();
    public abstract void decide();

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

    //===========   Movers   ===========//
    public void moveRandomly()
    {
    	float r = Random.value;
    	if (r < .25f)
    		this.moveUp();
    	else if (r < .5f)
    		this.moveDown();
    	else if (r < .75f)
    		this.moveLeft();
    	else
    		this.moveRight();
    }

    protected void moveUp()
    {
    	this.moveTarget = new Vector3(0.5f, 0.25f, 0f);
    }

    protected void moveDown()
    {
    	this.moveTarget = new Vector3(-0.5f, -0.25f, 0f);
    }

    protected void moveLeft()
    {
    	this.moveTarget = new Vector3(-0.5f, 0.25f, 0f);
    }

    protected void moveRight()
    {
    	this.moveTarget = new Vector3(0.5f, -0.25f, 0f);
    }

    //===========   Hitters   ===========//
    protected void attackUp()
    {
    	this.attackTarget = new Vector3(0.5f, 0.25f, 0f);
    }

    protected void attackDown()
    {
    	this.attackTarget = new Vector3(this.gridPosition.x - 0.5f, this.gridPosition.y - 0.25f, 0f);
    }

    protected void attackLeft()
    {
    	this.attackTarget = new Vector3(this.gridPosition.x - 0.5f, this.gridPosition.y + 0.25f, 0f);
    }

    protected void attackRight()
    {
    	this.attackTarget = new Vector3(this.gridPosition.x + 0.5f, this.gridPosition.y - 0.25f, 0f);
    }

    //===========   Getters   ===========//
    public int getHealth() { return this.health; }
    public int getStrength() { return this.strength; }
    public int getDamage() { return this.damage; }
    public int getID() { return this.enemyId; }
    public int getDecision() { return this.decision; }
    public Vector3 getMoveVector() { return this.moveTarget; }
    public Vector3 getAttackVector() { return this.attackTarget; }
    public char getRole() { return this.enemyRole; }

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
}
