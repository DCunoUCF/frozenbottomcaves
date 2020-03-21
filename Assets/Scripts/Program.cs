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

    // Function that loads up Dialogue text files
    public Dialogue LoadFile(string filename)
    {

        string path = "Assets/Resources/tutorial.txt";

        //Re-import the file to update the reference in the editor
        AssetDatabase.ImportAsset(path);

        TextAsset textAsset = (TextAsset)Resources.Load("tutorial");

        String[] lines = textAsset.text.Split('\n');
        String line;
        int i;

        Dialogue dialogueList = new Dialogue();

        // Key data to be obtained from file
        int id;
        String text;
        int options;

        for (i = 0; i < lines.Length; i++)
        {
            line = lines[i];

            // Continue if line begins with any of these characters
            if (line == null || line == "" || line[0] == '\r' || line[0] == '\n' || line[0] == '#')
                continue;

            DialogueNode dialogue = new DialogueNode();

            // Dialogue Id
            id = parseId(line);
            i++;
            line = lines[i];

            // Dialogue Text
            text = parseText(line);
            i++;
            line = lines[i];

            // Add it to Dialogue Node
            dialogue.addDialogue(text, id);

            // Number of Options
            options = parseId(line);
            i++;
            line = lines[i];

            for (int j = 0; j < options; j++)
            {
                // Option Text
                text = parseText(line);
                i++;
                line = lines[i];

                // Option Id
                id = parseId(line);
                i++;
                line = lines[i];

                // Add Option to Dialogue Node
                dialogue.addOption(text, id);
            }

            // Add Dialogue Node to our Dialogue List
            dialogueList.addNode(dialogue);
        }

        return dialogueList;
    }

    // Function to extract ids from text file
    public static int parseId(String line)
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
        data = line;

        // Jump to relevant delimeter
        index = data.IndexOf(":");

        // Store the modified raw to text in data
        data = data.Substring(index);

        // Append it to string builder so that we can manipulate it
        temp.Append(data);

        // Only append to our buffer if it is a number
        for (int i = 0; i < temp.Length; i++)
        {
            if (Char.IsDigit(temp[i]) || temp[i] == '-')
                buffer.Append(temp[i]);
        }

        return Int32.Parse(buffer.ToString());
    }

    // Function to extract text from file
    public static string parseText(String line)
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
        data = line;

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

