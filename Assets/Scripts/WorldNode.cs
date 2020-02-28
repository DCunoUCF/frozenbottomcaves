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
    Random = 0, Forest, Cave, Ice, Castle, Boss
}

public enum EncounterEnemy
{
	None,
	goblin, slime_G, gnoll, lich
}

public enum ItemType
{
	None,
	DwarfKey
}

// TODO: Make a struct that contains all the node information, 
	// and update the list in WorldNode to be a list of those structs

public class WorldNode : MonoBehaviour
{
	public List<int> NodeIDs;
	public List<FlagType> NodeTypes;
    public BattleClassList battleClassList = new BattleClassList();
	public List<ItemType> NodeItems;
}

[System.Serializable]
public class BattleClass
{
    public GridType grid;
    public string arena;
    public List<EncounterEnemy> nodeEnemies;
}

[System.Serializable]
public class BattleClassList
{
    public List<BattleClass> list;
}
