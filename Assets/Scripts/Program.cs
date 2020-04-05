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

    // Function that loads up Dialogue text files
    public Dialogue LoadFile(string filename)
    {
        TextAsset textAsset = (TextAsset)Resources.Load(filename);

        lines = textAsset.text.Split('\n');
        line = null;

        Dialogue dialogueList = new Dialogue();

        // Key data to be obtained from file
        int id;
        String text;
        int options;

        lineNumber = 0;

        while(lineNumber < lines.Length)
        { 
            line = lines[lineNumber++];

            // Continue if line begins with any of these characters
            if (line == null || line == "" || line[0] == '\r' || line[0] == '\n' || line[0] == '#')
                continue;

            DialogueNode dialogue = new DialogueNode();

            // Dialogue Id
            id = parseId(line);

            // Dialogue Text
            text = parseText(line);
            Debug.Log("Main dialogue ID: " + id + " text: " + text);
            // Add it to Dialogue Node
            dialogue.addDialogue(text, id);

            // Number of Options
            options = parseId(line);

            for (int j = 0; j < options; j++)
            {
                // Option Text
                text = parseText(line);

                // Option Id
                id = parseId(line);

            Debug.Log("Option " + (j+1) + ": destId:" + id + " text: " + text);
                // Add Option to Dialogue Node
                dialogue.addOption(text, id);
            }

            // Add Dialogue Node to our Dialogue List
            dialogueList.addNode(dialogue);
        }

        return dialogueList;
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

