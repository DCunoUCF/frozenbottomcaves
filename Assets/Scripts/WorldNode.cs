using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlagType
{
	NONE,
	BATTLE, STRSKILL, INTSKILL, AGISKILL, HPCHANGE, ITEMLOST, ITEMGAINED, HPMAXCHANGE, SAVE, LOAD
}

public enum GridType
{
    Random = 0, Forest, Cave, Ice, Castle, Boss
}

public enum EncounterEnemy
{
	None,
	goblin, slime_G, gnoll, lich, troll, nixie, gnoll_logger, wolf_black, spider_small, spider_queen, bandit_scout, bandit_thug, bandit_leader
}


// TODO: Make a struct that contains all the node information, 
	// and update the list in WorldNode to be a list of those structs

public class WorldNode : MonoBehaviour
{
	public List<int> NodeIDs;
	public List<FlagType> NodeTypes;
    public BattleClassList battleClassList = new BattleClassList();
	public List<itemEvent> NodeItems;
    public List<itemLoseEvent> NodeItemsLose;
    public List<int> HealthChange;
    public List<int> MaxHealthChange;
    public List<bool> SaveProvisions;
    public List<int> SkillCheckDifficulty;
}

[System.Serializable]
public class BattleClass
{
    public GridType grid;
    public string arena;
    public List<EncounterEnemy> nodeEnemies;

    public BattleClass DialogueNodeToBattleClass(DialogueNode curNode)
    {
        BattleClass curBattleClass = new BattleClass();
        string gridString = curNode.grid;

        curBattleClass.grid = (GridType) GridType.Parse(typeof(GridType), curNode.grid, true);

        curBattleClass.arena = curNode.arena;

        for (int i = 0; i < curNode.enemyType.Count; i++)
        {
            curBattleClass.nodeEnemies.Add((EncounterEnemy)EncounterEnemy.Parse(typeof(EncounterEnemy), curNode.enemyType[i], true));

            /*if (curBattleClass.nodeEnemies[i] == null)
                curBattleClass.nodeEnemies[i] = EncounterEnemy.None;*/
        }

        return curBattleClass;
    }
}

[System.Serializable]
public class BattleClassList
{
    public List<BattleClass> list;
}

[System.Serializable]
public class itemEvent
{
    public Item.ItemType item;
    public int count;
}

[System.Serializable]
public class itemLoseEvent
{
    public Item.ItemType item;
    public int count;
}
