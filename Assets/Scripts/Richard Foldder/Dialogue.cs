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

