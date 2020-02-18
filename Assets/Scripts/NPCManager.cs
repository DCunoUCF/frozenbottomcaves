using System.Collections;
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

	NPCManager(BattleManager battle)
	{
		this.bm = battle;
	}

	//==========   Unity Methods   ==========//

    // Start is called before the first frame update
    // The contents of Start() may be moved to the constructor
    void Start()
    {
    	// Instantiate our sub-managers
        this.enemyM = new EnemyManager();
        this.companionM = new CompanionManager();

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

    public void makeDecisions()
    {
    	// HERE the lists will probably need to be updated, so that we're going off of the most
    	// recent positions for CList members. This is for both managers.

    	enemyM.makeDecisions();
    	companionM.makeDecisions();

    	// Since the lists actually contain the GameObjects, we should be able to just change
    	// the GameObject's state from inside these managers, so this should handle all the
    	// decisions for us.
    }

   	// May need to change type to CList
    public void removeEnemy(GameObject entity)
    {
    	// TODO: pass param along so that enemy manager knows who to delete
    	enemyM.removeEnemy(entity);
    }

   	// May need to change type to CList
    public void removeCompanion(GameObject entity)
    {
    	// TODO: pass param along so that companion manager knows who to delete
    	companionM.removeCompanion(entity);
    }
}
