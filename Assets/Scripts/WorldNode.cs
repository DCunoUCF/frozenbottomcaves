using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlagType
{
	None = -1,
	Battle, Event, Item
}

public enum EncounterEnemy
{
	None = -1,
	Goblin, Slime
}

public enum ItemType
{
	None = -1,
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
