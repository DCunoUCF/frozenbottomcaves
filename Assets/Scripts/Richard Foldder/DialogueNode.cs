using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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

