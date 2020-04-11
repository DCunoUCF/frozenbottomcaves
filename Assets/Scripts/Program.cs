using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Program
{
    // Global File Reference Variable
    public static StreamReader sr;

    // Globals
    static String[] lines;
    static String line;
    static int lineNumber;

    public Dialogue LoadFile(string filename)
    {
        TextAsset textAsset = (TextAsset)Resources.Load(filename);

        lines = textAsset.text.Split('\n');
        line = null;
        lineNumber = 0;

        Dialogue dialogueList = new Dialogue();

        while (lineNumber < lines.Length)
        {
            line = lines[lineNumber];

            // Continue if line begins with any of these characters
            if (line == null || line == "" || line[0] == '\r' || line[0] == '\n' || line[0] == '#')
            {
                lineNumber++;
                continue;
            }

            DialogueNode dialogue = parseDialogueNode();

            dialogueList.addNode(dialogue.nodeId, dialogue);
        }

        return dialogueList;
    }


    // Function used to Build your DialogueNode
    public DialogueNode parseDialogueNode()
    {
        bool hasStatements = true;

        DialogueNode node = new DialogueNode();

        while (hasStatements)
        {
            string data = lines[lineNumber++];

            string prefix = ExtractUpToDelimeter(data, ':');

            // Parse Dialogue Node data
            if (prefix.ToLower() == "nodeid")
            {
                node.nodeId = parseId(data, ':');

                Debug.Log("Node: " + node.nodeId);
            }
            // Dialogue Text
            else if (prefix.ToLower() == "text")
            {
                node.text = parseText(data, ':');
            }
            else if(prefix.ToLower() == "event")
            {
                // Event
                string tempEvent;

                // Effect
                int tempEffect;

                // Item Gained / Lost
                string item;

                // Amount Gained / Lost
                int amount;

                prefix = parseText(data, ':');

                Debug.Log("Event prefix: " + prefix.ToLower());

                // Effect Events
                if (prefix.ToLower() == "hpchange" || prefix.ToLower() == "hpmaxchange" || prefix.ToLower() == "strchange" || prefix.ToLower() == "intchange" || prefix.ToLower() == "agichange")
                {
                    // Event
                    tempEvent = parseText(data, ':');
                    node.overworldEvent.Add(tempEvent);

                    // Effect
                    data = lines[lineNumber++];
                    tempEffect = parseId(data, ':');
                    node.effect.Add(tempEffect);

                    // Buffering the List to the correct indices
                    node.itemGained.Add("");
                    node.itemGainedAmount.Add(0);
                    node.itemLost.Add("");
                    node.itemLostAmount.Add(0);
                }
                // Gain Event
                else if (prefix.ToLower() == "itemgained")
                {
                    // Event
                    tempEvent = parseText(data, ':');
                    node.overworldEvent.Add(tempEvent);

                    // Item Gained
                    data = lines[lineNumber++];
                    item = parseText(data, ':');
                    node.itemGained.Add(item);

                    // Item Gained Amount
                    data = lines[lineNumber++];
                    amount = parseId(data, ':');
                    node.itemGainedAmount.Add(amount);

                    // Buffering the List to the correct indices
                    node.itemLost.Add("");
                    node.itemLostAmount.Add(0);
                    node.effect.Add(0);
                }
                // Loss Event
                else if (prefix.ToLower() == "itemlost")
                {
                    // Event
                    tempEvent = parseText(data, ':');
                    node.overworldEvent.Add(tempEvent);

                    // Item Lost
                    data = lines[lineNumber++];
                    item = parseText(data, ':');
                    node.itemLost.Add(item);

                    // Item Lost Amount
                    data = lines[lineNumber++];
                    amount = parseId(data, ':');
                    node.itemLostAmount.Add(amount);

                    // Buffering the List to the correct indices
                    node.itemGained.Add("");
                    node.itemGainedAmount.Add(0);
                    node.effect.Add(0);
                }
                else if (prefix.ToLower() == "strskill" || prefix.ToLower() == "intskill" || prefix.ToLower() == "agiskill")
                {
                    Debug.Log("Skill Event");
                    // Event
                    tempEvent = parseText(data, ':');
                    node.overworldEvent.Add(tempEvent);

                    Debug.Log("Type of Skill Event: " + tempEvent);

                    // Skill Check Difficulty
                    data = lines[lineNumber++];
                    node.skillCheckDifficulty = parseId(data, ':');

                    Debug.Log("Skill Check Difficulty: " + node.skillCheckDifficulty);

                    // Buffering the List to the correct indices
                    node.itemGained.Add("");
                    node.itemGainedAmount.Add(0);
                    node.itemLost.Add("");
                    node.itemLostAmount.Add(0);
                    node.effect.Add(0);
                }
                // BATTLE
                else if (prefix.ToLower() == "battle")
                {
                    Debug.Log("Battle Event at node: " + node.nodeId);
                    // Event
                    tempEvent = parseText(data, ':');
                    node.overworldEvent.Add(tempEvent);

                    Debug.Log("Event: " + tempEvent);
                    // Grid
                    data = lines[lineNumber++];
                    node.grid = parseText(data, ':');
                    
                    Debug.Log("Grid: " + node.grid);
                    // Arena
                    data = lines[lineNumber++];
                    node.arena = parseText(data, ':');
                    Debug.Log("Arena: " + node.arena);

                    // Enemy
                    data = lines[lineNumber++];
                    int numEnemies = parseId(data, ':');
                    string enemy;

                    for (int i = 0; i < numEnemies; i++)
                    {
                        data = lines[lineNumber++];
                        enemy = parseText(data, ':');
                        node.enemyType.Add(enemy);
                        Debug.Log("Enemy: " + node.enemyType[i]);
                    }

                    // Buffering the List to the correct indices
                    node.itemGained.Add("");
                    node.itemGainedAmount.Add(0);
                    node.itemLost.Add("");
                    node.itemLostAmount.Add(0);
                    node.effect.Add(0);
                }
                // Save Event
                else
                {
                    tempEvent = parseText(data, ':');
                    node.overworldEvent.Add(tempEvent);

                    // Buffering the List to the correct indices
                    node.itemGained.Add("");
                    node.itemGainedAmount.Add(0);
                    node.itemLost.Add("");
                    node.itemLostAmount.Add(0);
                    node.effect.Add(0);
                }
            }
            // Parse numevents
            else if (prefix.ToLower() == "numevents")
            {
                int numEvents = parseId(data, ':');

                // Event
                string tempEvent;

                // Effect
                int tempEffect;

                // Item Gained / Lost
                string item;

                // Amount Gained / Lost
                int amount;

                for (int i = 0; i < numEvents; i++)
                {
                    data = lines[lineNumber++];
                    prefix = parseText(data, ':');

                    // Effect Events
                    if (prefix.ToLower() == "hpchange" || prefix.ToLower() == "hpmaxchange" || prefix.ToLower() == "strchange" || prefix.ToLower() == "intchange" || prefix.ToLower() == "agichange")
                    {
                        // Event
                        tempEvent = parseText(data, ':');
                        node.overworldEvent.Add(tempEvent);

                        // Effect
                        data = lines[lineNumber++];
                        tempEffect = parseId(data, ':');
                        node.effect.Add(tempEffect);

                        // Buffering the List to the correct indices
                        node.itemGained.Add("");
                        node.itemGainedAmount.Add(0);
                        node.itemLost.Add("");
                        node.itemLostAmount.Add(0);
                    }
                    // Gain Event
                    else if (prefix.ToLower() == "itemgained")
                    {
                        // Event
                        tempEvent = parseText(data, ':');
                        node.overworldEvent.Add(tempEvent);

                        // Item Gained
                        data = lines[lineNumber++];
                        item = parseText(data, ':');
                        node.itemGained.Add(item);

                        // Item Gained Amount
                        data = lines[lineNumber++];
                        amount = parseId(data, ':');
                        node.itemGainedAmount.Add(amount);

                        // Buffering the List to the correct indices
                        node.itemLost.Add("");
                        node.itemLostAmount.Add(0);
                        node.effect.Add(0);
                    }
                    // Loss Event
                    else if (prefix.ToLower() == "itemlost")
                    {
                        // Event
                        tempEvent = parseText(data, ':');
                        node.overworldEvent.Add(tempEvent);

                        // Item Lost
                        data = lines[lineNumber++];
                        item = parseText(data, ':');
                        node.itemLost.Add(item);

                        // Item Lost Amount
                        data = lines[lineNumber++];
                        amount = parseId(data, ':');
                        node.itemLostAmount.Add(amount);

                        // Buffering the List to the correct indices
                        node.itemGained.Add("");
                        node.itemGainedAmount.Add(0);
                        node.effect.Add(0);
                    }
                    else if (prefix.ToLower() == "strskill" || prefix.ToLower() == "intskill" || prefix.ToLower() == "agiskill")
                    {
                        Debug.Log("Skill Event");
                        // Event
                        tempEvent = parseText(data, ':');
                        node.overworldEvent.Add(tempEvent);

                        Debug.Log("Type of Skill Event: " + tempEvent);

                        // Skill Check Difficulty
                        data = lines[lineNumber++];
                        node.skillCheckDifficulty = parseId(data, ':');

                        Debug.Log("Skill Check Difficulty: " + node.skillCheckDifficulty);

                        // Buffering the List to the correct indices
                        node.itemGained.Add("");
                        node.itemGainedAmount.Add(0);
                        node.itemLost.Add("");
                        node.itemLostAmount.Add(0);
                        node.effect.Add(0);
                    }
                    // BATTLE
                    else if (prefix.ToLower() == "battle")
                    {
                        Debug.Log("Battle Event at node: " + node.nodeId);
                        // Event
                        tempEvent = parseText(data, ':');
                        node.overworldEvent.Add(tempEvent);

                        Debug.Log("Event: " + tempEvent);
                        // Grid
                        data = lines[lineNumber++];
                        node.grid = parseText(data, ':');

                        Debug.Log("Grid: " + node.grid);
                        // Arena
                        data = lines[lineNumber++];
                        node.arena = parseText(data, ':');
                        Debug.Log("Arena: " + node.arena);

                        // Enemy
                        data = lines[lineNumber++];
                        int numEnemies = parseId(data, ':');
                        string enemy;

                        for (int j = 0; j < numEnemies; j++)
                        {
                            data = lines[lineNumber++];
                            enemy = parseText(data, ':');
                            node.enemyType.Add(enemy);
                            Debug.Log("Enemy: " + node.enemyType[i]);
                        }

                        // Buffering the List to the correct indices
                        node.itemGained.Add("");
                        node.itemGainedAmount.Add(0);
                        node.itemLost.Add("");
                        node.itemLostAmount.Add(0);
                        node.effect.Add(0);
                    }
                    // Save Event
                    else
                    {
                        tempEvent = parseText(data, ':');
                        node.overworldEvent.Add(tempEvent);

                        // Buffering the List to the correct indices
                        node.itemGained.Add("");
                        node.itemGainedAmount.Add(0);
                        node.itemLost.Add("");
                        node.itemLostAmount.Add(0);
                        node.effect.Add(0);
                    }
                }
            }
            // Parse Option Data
            else if (prefix.ToLower() == "options")
            {
                OptionNode op;

                // Number of options
                int optionCount = parseId(data, ':');

                Debug.Log("Number of Options: " + optionCount);

                for (int i = 0; i < optionCount; i++)
                {

                    op = new OptionNode();
                    data = lines[lineNumber++];
                    prefix = ExtractUpToDelimeter(data, ':');

                    Debug.Log("Parsing option: " + i);
                    Debug.Log("Next prefix: " + prefix);

                    // Richard's Code
                    /*// Keep looping until you come across destId field
                    while (prefix.ToLower() != "destid")
                    {
                        // Text
                        if (prefix.ToLower() == "text")
                        {
                            op.text = parseText(data, ':');
                        }
                        // Item Required
                        else if (prefix.ToLower() == "itemreq")
                        {
                            op.itemReq = parseText(data, ':');
                        }
                        // Item Required Amount
                        else if (prefix.ToLower() == "itemreqamount")
                        {
                            op.itemReqAmount = parseId(data, ':');
                        }
                        else
                        {
                            Console.WriteLine("Error in options");
                        }

                        data = lines[lineNumber++];
                        prefix = ExtractUpToDelimeter(data, ':');
                    }*/

                    // David's Modification
                    if (prefix.ToLower() == "text")
                    {
                        // Text
                        op.text = parseText(data, ':');
                        data = lines[lineNumber++];
                        prefix = ExtractUpToDelimeter(data, ':');

                        Debug.Log("Parsed text: " + op.text);
                        Debug.Log("Item Required or not?: " + prefix.ToLower());
                        
                        // Item Required
                        if (prefix.ToLower() == "itemreq")
                        {
                            Debug.Log("Item Required.");

                            op.itemReq = parseText(data, ':');
                            data = lines[lineNumber++];

                            op.itemReqAmount = parseId(data, ':');
                            data = lines[lineNumber++];

                            op.destId = parseId(data, ':');

                            Debug.Log("Item Required: " + op.itemReq + " Item Required Amount: " + op.itemReqAmount);
                            Debug.Log("DestID: " + op.destId);
                        }
                        // Dest ID
                        else
                        {
                            Debug.Log("Item Not Required.");

                            op.destId = parseId(data, ':');

                            op.itemReq = "";
                            op.itemReqAmount = 0;

                            Debug.Log("Item Required: " + op.itemReq + " Item Required Amount: " + op.itemReqAmount);
                            Debug.Log("DestID: " + op.destId);
                        }

                    }
                    else
                    {
                        Debug.Log("Error in option node: " + i);
                        Console.WriteLine("Error in options");
                    }

                    Debug.Log("Adding option: " + i);
                    // Add Option Node into our Dialogue Node
                    node.addOption(op);

                }

                // End of Dialogue Node block
                hasStatements = false;
            }
            else
            {
                Console.WriteLine("Not a field");
            }
        }

        return node;
    }

    // Function to extract text from file (uses statement)
    public static string parseText(string statement, char delimeter)
    {
        // String to hold the data we actually want.
        StringBuilder buffer = new StringBuilder();

        // String that holds the modified raw text
        StringBuilder temp = new StringBuilder();

        // Holds the actual raw text
        String data;

        // We use this to jump to to the delimeter in our .Substring function
        int index;

        // Grab line from text file
        data = statement;

        // Jump to relevant delimeter
        index = data.IndexOf(delimeter);

        // Store the modified raw text in data
        data = data.Substring(index+1);

        // Append it to string builder so that we can manipulate it
        temp.Append(data);

        // Only append to relevant characters to our buffer
        for (int i = (temp[0] == ' ') ? 1 : 0; i < temp.Length; i++)
        {
            if (temp[i] == '\n' || temp[i] == '\r' || temp[i] == ':')
                continue;

            buffer.Append(temp[i]);
        }

        return buffer.ToString();
    }


    // Function to extract ids from text file (uses statement)
    public static int parseId(string statement, char delimeter)
    {
        // String to hold the data we actually want.
        StringBuilder buffer = new StringBuilder();

        // String that holds the modified raw text
        StringBuilder temp = new StringBuilder();

        // Holds the actual raw text
        string data;

        // We use this to jump to to the delimeter in our .Substring function
        int index;

        // Grab line from text file
        data = statement;

        // Jump to relevant delimeter
        index = data.IndexOf(delimeter);

        // Store the modified raw to text in data
        data = data.Substring(index);

        // Append it to string builder so that we can manipulate it
        temp.Append(data);

        Debug.Log("Parsing ID, line: " + temp.ToString());

        // Only append to our buffer if it is a number
        for (int i = 0; i < temp.Length; i++)
        {
            if (Char.IsDigit(temp[i]) || temp[i] == '-')
            {
                buffer.Append(temp[i]);
            }
        }

        return Int32.Parse(buffer.ToString());
    }

    // Function that extracts text up to a specified delimeter
    public string ExtractUpToDelimeter(string statement, char delimeter)
    {
        // String to hold the data we actually want.
        StringBuilder buffer = new StringBuilder();

        // String that holds the modified raw text
        StringBuilder temp = new StringBuilder();

        // Holds the actual raw text
        String data = statement;

        // We use this to jump to to the delimeter in our .Substring function
        int index;

        // Jump to relevant delimeter
        index = data.IndexOf(delimeter);

        // Store the modified raw text in data
        data = data.Substring(0, index);

        // Append it to string builder so that we can manipulate it
        temp.Append(data);

        // Only append to relevant characters to our buffer
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i] == '\n' || temp[i] == '\r' || temp[i] == ':' || temp[i] == ' ')
                continue;

            buffer.Append(temp[i]);
        }

        return buffer.ToString();
    }


    // Function to extract ids from text file
    public static int parseId(String str)
    {
        // String to hold the data we actually want.
        StringBuilder buffer = new StringBuilder();

        // String that holds the modified raw text
        StringBuilder temp = new StringBuilder();

        // Holds the actual raw text
        String data;

        // We use this to jump to to the delimeter in our .Substring function
        int index;

        // Grab line from text file
        data = str;

        // Jump to relevant delimeter
        index = data.IndexOf(":");

        // Store the modified raw to text in data
        data = data.Substring(index);

        // Append it to string builder so that we can manipulate it
        temp.Append(data);

        // Only append to our buffer if it is a number
        for (int j = 0; j < temp.Length; j++)
        {
            if (Char.IsDigit(temp[j]) || temp[j] == '-')
                buffer.Append(temp[j]);
        }

        if (lineNumber < lines.Length)
            line = lines[lineNumber++];

        return Int32.Parse(buffer.ToString());
    }

    // Function to extract text from file
    public static string parseText(String str)
    {
        // String to hold the data we actually want.
        StringBuilder buffer = new StringBuilder();

        // String that holds the modified raw text
        StringBuilder temp = new StringBuilder();

        // Holds the actual raw text
        String data;

        // We use this to jump to to the delimeter in our .Substring function
        int index;

        // Grab line from text file
        data = str;

        // Jump to relevant delimeter
        index = data.IndexOf(":");

        // Store the modified raw to text in data
        data = data.Substring(index);

        // Append it to string builder so that we can manipulate it
        temp.Append(data);

        // Only append to our buffer if it is
        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i] == '\n' || temp[i] == '\r' || temp[i] == ':')
                continue;

            buffer.Append(temp[i]);
        }

        if (lineNumber < lines.Length)
            line = lines[lineNumber++];

        return buffer.ToString();
    }

    // Not used at the moment but could be useful in future
    public static void consumeFeeds()
    {
        if (sr.Peek() >= 0)
            return;

        if ((char)sr.Peek() == '\r')
            sr.Read();

        if ((char)sr.Peek() == '\n')
            sr.Read();
    }
}

