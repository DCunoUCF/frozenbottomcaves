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

    public void addNode(int idx, DialogueNode node)
    {
        nodes[idx] = node;
    }
}

public class DialogueNode
{
    public int nodeId;
    public string text;
    public List<OptionNode> options;

    public DialogueNode()
    {
        options = new List<OptionNode>();
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
}

public class OptionNode
{
    public string text;
    public int destId;

    public OptionNode() { }

    public OptionNode(string text, int dest)
    {
        this.text = text;
        this.destId = dest;
    }
}

