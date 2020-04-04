using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Dialogue
{
    public List<DialogueNode> nodes;

    public Dialogue()
    {
        nodes = new List<DialogueNode>();
    }

    public void addNode(DialogueNode node)
    {
        nodes.Add(node);
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

