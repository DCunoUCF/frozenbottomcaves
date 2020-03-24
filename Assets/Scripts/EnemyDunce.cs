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

    	// Set initial positions
    	this.gridPosition = new Vector3(0, 0, 0);
    	this.attackTarget = new Vector3(0, 0, 0);
    	this.moveTarget = new Vector3(0, 0, 0);

    	// Generate ID
    	this.generateID();

    	if (this.combatantEntry == null)
    	{
    		Debug.Log("E#"+this.enemyId+" says AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAHHHHHHHHHHHHHHHHHHHHHHHHHHH");
    		this.kill();
    	}
    }

    protected override void move()
    {
    	Debug.Log("E#"+this.enemyId+" is moving!");
    }

    protected override void attack()
    {
    	Debug.Log("E#"+this.enemyId+" is attacking!");
    }

    public override void decide()
    {
    	Debug.Log("E#"+this.enemyId+" has been called upon to think!");

    	if (Random.value < .5f)
    		this.move();
    	else
    		this.attack();
    }

}
