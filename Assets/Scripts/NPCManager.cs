﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
	public BattleManager bm;

	private EnemyManager enemyM;
	private CompanionManager companionM;

	private List<CList> combatantList;
	private List<CList> friendlyList;
	private List<CList> companionList;
	private List<CList> enemyList;

	//==========   Constructor   ==========//

	public NPCManager(BattleManager battle)
	{
		this.bm = battle;
	}

	//==========   Unity Methods   ==========//

    // Start is called before the first frame update
    // The contents of Start() may be moved to the constructor
    void Start()
    {
    	// Instantiate our sub-managers
        this.enemyM = new EnemyManager(this);
        this.companionM = new CompanionManager(this);

        this.updateLocalLists();

        // CompanionManager gets a list of all companions, and a list of all enemies
        this.companionM.setEntityLists(this.companionList, this.enemyList);
        // EnemyManager gets a list of all enemies, and a list of all companions plus the player
        this.enemyM.setEntityLists(this.enemyList, this.friendlyList);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //==========   AI Methods   ==========//

    private void updateLists()
    {
    	// Grab fresh copies of the lists from the BattleManager
    	this.updateLocalLists();

    	// Update manager lists to reflect fresh copies
    	this.updateManagerLists();
    }

    // TODO
    private void updateLocalLists()
    {
    	// Take list of combatants and split into two lists
        this.combatantList = bm.combatantList;

        	// FriendlyList
        	this.friendlyList = new List<CList>();
        	// foreach FRIENDLY in this.combatantList
        		// this.friendlyList.Add(FRIENDLY)
        	this.companionList = this.friendlyList;
        	// this.companionList.Remove(PLAYER);

        	// EnemyList
        	this.enemyList = new List<CList>();
        	// foreach ENEMY in this.combatantList
        		// this.enemyList.Add(ENEMY)
    }

    public void importEnemyList(List<CList> eList)
    {
    	this.enemyList = eList;
    }

    public void importCompanionList(List<CList> cList)
    {
    	this.companionList = cList;
    }

    private void updateManagerLists()
    {
    	this.companionM.setEntityLists(this.companionList, this.enemyList);
    	this.enemyM.setEntityLists(this.enemyList, this.friendlyList);
    }

    private void exportLists()
    {
    	this.exportEnemyList();
    	this.exportCompanionList();
    }

    // TODO
    private void exportEnemyList()
    {
    	// foreach ENEMY in this.enemyList
    		// foreach CList in bm.combatantList
    			// if ENEMY.id == CList.id
    				// Copy over decisions to bm.combatantList
    }

    // TODO
    private void exportCompanionList()
    {
    	// foreach COMPANION in this.companionList
    		// foreach CList in bm.combatantList
    			// if COMPANION.id == CList.id
    				// Copy over decisions to bm.combatantList
    }

    public void makeDecisions()
    {
    	// Grab our fresh copies and update our managers
    	this.updateLists();

    	// Make the decisions
    	enemyM.makeDecisions();
    	companionM.makeDecisions();

    	// Update the correct CList peoples in the BattleManager
    	this.exportLists();
    }
}
