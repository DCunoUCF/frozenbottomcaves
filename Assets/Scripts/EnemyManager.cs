using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	private List<CList> enemyList;
	private List<CList> targetList;

	//==========   Unity Methods   ==========//

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //==========   Combat Methods   ==========//

    void setEntityLists(List<CList> eList, List<CList> tList)
    {
    	this.enemyList = eList;
    	this.targetList = tList;
    }

    void makeDecisions()
    {
    	// foreach ENEMY in this.enemyList
    		// if this.targetList.Count == 0
    			// move randomly
    		// else
    			// closestTarget = this.targetList.At(0);
    			// foreach POT_TARGET in this.targetList
    				// if (distance to POT_TARGET) < (distance to closestTarget)
    					// closestTarget = POT_TARGET
    			// choose move towards closestTarget or attack towards closestTarget
    }

    // TODO: Add param for specific enemy to delete
    void removeEnemy()
    {

    }
}
