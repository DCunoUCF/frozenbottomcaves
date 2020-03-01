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
    	//
    	Debug.Log("Oy blyat! E#"+this.enemyId+" am dead");
    }

    public void dealDamage(int dam)
    {
    	this.health -= dam;

    	if (this.health <= 0)
    		kill();
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
}
