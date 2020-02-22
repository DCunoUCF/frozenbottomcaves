using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlagType
{
	None,
	Battle, Event, Item
}

public enum EncounterEnemy
{
	None,
	Goblin, Slime
}

public enum ItemType
{
	None,
	DwarfKey
}

public class WorldNode : MonoBehaviour
{
	public List<int> NodeIDs;
	public List<FlagType> NodeTypes;
	public List<List<EncounterEnemy>> NodeEnemies;
	public List<ItemType> NodeItems;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
