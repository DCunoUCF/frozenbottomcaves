using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
	protected int enemyId;
	protected char enemyRole;
	protected int health;
	protected int damage;
	protected int strength;
	protected Vector3 gridPosition;
	protected Vector3 moveTarget;
	protected Vector3 attackTarget;
	protected int decision; // 0 - no decision, 1 - move, 2 - attack
	protected CList combatantEntry;

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
    	this.attackTarget = new Vector3(this.gridPosition.x + 0.5f, this.gridPosition.y + 0.25f, 0f);
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
}
