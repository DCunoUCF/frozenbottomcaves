using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager// : MonoBehaviour
{
    private BattleManager bm;
	private NPCManager myManager;

	private List<CList> enemyList;
	private List<CList> targetList;

	//==========   Constructors   ==========//

	public EnemyManager(NPCManager manager)
	{
		this.myManager = manager;
        this.bm = manager.bm;
	}

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

    public void setEntityLists(List<CList> eList, List<CList> tList)
    {
    	this.enemyList = eList;
    	this.targetList = tList;
    }

    // TODO
    public void makeDecisions()
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
        Debug.Log("Here's the enemyList!");
        int i = 1;
        foreach (CList e in this.enemyList)
        {
            Debug.Log("Enemy "+i+": "+e.entity.name);
            i++;
        }

        foreach (CList e in this.enemyList)
        {
            Enemy comp;

            if (e.entity.GetComponent<Enemy>() != null)
            {
                comp = e.entity.GetComponent<Enemy>();
                comp.setCombatantEntry(e);
                comp.setBattleManager(this.bm);
                comp.init();
            }
            else
            {
                e.entity.AddComponent<Enemy>();
                e.entity.GetComponent<Enemy>().setEnemyClass(EnemyClass.Dunce);
                e.entity.GetComponent<Enemy>().setCombatantEntry(e);
                e.entity.GetComponent<Enemy>().setBattleManager(this.bm);
                e.entity.GetComponent<Enemy>().init();

                comp = e.entity.GetComponent<Enemy>();
            }

            // e.entity.GetComponent<EnemyDunce>().moveRandomly();
            comp.decide();
            comp.ehb.updateBar(comp.getHealth());
            e.hp = comp.getHealth();

            int decision = comp.getDecision();
            if (decision == 1)
            {
                e.movTar = comp.getMoveVector();
                e.atkTar = null;
                e.move = true;
                e.attack = -999;
            }
            else if (decision == 2)
            {
                e.move = false;
                e.attack = 1;
                e.attackDmg = comp.getDamage();
                e.atkTar = comp.getAttackVector();
                e.movTar = new Vector3();
            }
        }

    	// Export our list of decided peoples to the NPCManager
    	this.myManager.importEnemyList(this.enemyList);
    }
}