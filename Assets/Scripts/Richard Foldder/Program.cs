using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

    public class Program
    {
        // Global File Reference Variable
        public static StreamReader sr;

        // Function that loads up Dialogue text files
        public Dialogue LoadFile(String filename)
        {
            // Initialize stream reader
            sr = new StreamReader(filename);

            Dialogue dialogueList = new Dialogue();
            
            // Key data to be obtained from file
            int id;
            String text;
            int options;

            while(sr.Peek() >= 0)
            {
                // If we come across carriage return or newline, loop again
                if ((char)sr.Peek() == '\r' || (char)sr.Peek() == '\n' || (char)sr.Peek() == '#')
                {
                    sr.ReadLine();
                    continue;
                }

                // Start up a new Dialogue
                DialogueNode dialogue = new DialogueNode();

                // Get the id and text from file
                id = parseId();
                text = parseText();

                // Add the extracted data into our dialogue node object
                dialogue.addDialogue(text, id);

                // Number of options to be added
                options = parseId();

                // Loop through and get the text and data from file
                // Add the option to our dialogue node
                for(int i = 0; i < options; i++)
                {
                    text = parseText();
                    id = parseId();
                    dialogue.addOption(text, id);
                }

                // Finally add it to our Dialogue List
                dialogueList.addNode(dialogue);
            }

            return dialogueList;
            
        }

        
        // Function to extract ids from text file
        public static int parseId()
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
            data = sr.ReadLine();

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
        public static string parseText()
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
            data = sr.ReadLine();

            // Jump to relevant delimeter
            index = data.IndexOf(":");

            // Store the modified raw to text in data
            data = data.Substring(index);

            // Append it to string builder so that we can manipulate it
            temp.Append(data);

            // Only append to our buffer if it is
            for (int i = 0; i < temp.Length;i++)
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

