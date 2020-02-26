using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlagType
{
	None,
	Battle, Event, Item
}

public enum GridType
{
    Random = 0, Forest, Cave, Ice, Castle
}

public enum EncounterEnemy
{
	None,
	Goblin, Slime_G, Gnoll, Lich
}

public enum ItemType
{
	None,
	DwarfKey
}

public struct BattleStruct
{
    public GridType grid;
    public string arena;
	public List<EncounterEnemy> nodeEnemies;
}

// TODO: Make a struct that contains all the node information, 
	// and update the list in WorldNode to be a list of those structs

public class WorldNode : MonoBehaviour
{
	public List<int> NodeIDs;
	public List<FlagType> NodeTypes;
    public List<BattleStruct> battleStruct;
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
