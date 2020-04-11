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
    public int nodeId;
    public string text;
    public int skillCheckDifficulty;
    public string itemGained;
    public int itemGainedAmount;
    public string itemLost;
    public int itemLostAmount;
    public List<string> enemyType;
    public List<string> overworldEvent;
    public List<int> effect;
    public string arena;

    public List<OptionNode> options;

    public DialogueNode()
    {
        options = new List<OptionNode>();
        enemyType = new List<string>();
        overworldEvent = new List<string>();
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
