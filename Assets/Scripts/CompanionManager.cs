using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionManager : MonoBehaviour
{
	private NPCManager myManager;

	private List<CList> companionList;
	private List<CList> targetList;

	//==========   Constructors   ==========//

	public CompanionManager(NPCManager manager)
	{
		this.myManager = manager;
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

    // These will be very similar to the EnemyManager methods of the same name

    public void setEntityLists(List<CList> cList, List<CList> tList)
    {
    	this.companionList = cList;
    	this.targetList = tList;
    }

    // TODO
    public void makeDecisions()
    {
    	// See EnemyManager.makeDecisions() for more information on how this will
    	// be written.

    	// Export our list of decided peoples to the NPCManager
    	this.myManager.importCompanionList(this.companionList);
    }
}
