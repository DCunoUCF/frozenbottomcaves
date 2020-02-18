using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionManager : MonoBehaviour
{
	private List<CList> companionList;
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

    // These will be very similar to the EnemyManager methods of the same name

    public void setEntityLists(List<CList> cList, List<CList> tList)
    {
    	this.companionList = cList;
    	this.targetList = tList;
    }

    public void makeDecisions()
    {
    	// See EnemyManager.makeDecisions() for more information on how this will
    	// be written.
    }

    // May be changed to CList type
    public void removeCompanion(GameObject entity)
    {

    }
}
