using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Dialogue
{
    public DialogueNode[] nodes;

    public Dialogue()
    {
        nodes = new DialogueNode[1000]; // Hardcoded size. Make bigger if game bigger
    }

    public void addNode(int index, DialogueNode node)
    {
        nodes[index] = node;
    }
}

public class DialogueNode
{
    // Common Data
    public int nodeId;
    public string text;
    public int skillCheckDifficulty;
    public string arena;

    // Items
    public List<string> itemGained;
    public List<int> itemGainedAmount;
    public List<string> itemLost;
    public List<int> itemLostAmount;

    // Enemies
    public List<string> enemyType;

    // Events
    public List<string> overworldEvent;
    public List<int> effect;
    

    public List<OptionNode> options;

    public DialogueNode()
    {
        // Options
        options = new List<OptionNode>();

        // Enemies
        enemyType = new List<string>();

        // Items
        itemGained = new List<string>();
        itemGainedAmount = new List<int>();
        itemLost = new List<string>();
        itemLostAmount = new List<int>();

        // Events
        overworldEvent = new List<string>();

        // Effect
        effect = new List<int>();
    }

    public void addDialogue(string text, int id)
    {
        this.text = text;
        this.nodeId = id;
    }

    public void addOption(string text, int dest)
    {
        OptionNode op = null;

        op = new OptionNode(text, dest);
        this.options.Add(op);
    }

    public void addOption(OptionNode op)
    {
        this.options.Add(op);
    }
}

public class OptionNode
{
    public string text;
    public string itemReq;
    public int itemReqAmount;

    public int destId;

    public OptionNode() { }

    public OptionNode(string text, int dest)
    {
        this.text = text;
        this.destId = dest;
    }
}
