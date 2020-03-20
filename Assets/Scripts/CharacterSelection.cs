using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Drawing;
using System;
using System.Text.RegularExpressions;


public static class CharacterSelection
{
    //public PlayerClass player;
    //private string path = "Assets/Resources/CharacterStats/";

    public static PlayerClass writeStats(string filename)
    {
        PlayerClass pc = new PlayerClass();

        string path = "./Assets/Resources/CharacterStats/";

        string path2 = "CharacterStats/";
        path2 += filename;

        List<Point> ability1list = new List<Point>();
        List<Point> ability2list = new List<Point>();

        path += filename;
        path += ".txt";
        TextAsset textFile = Resources.Load(path2) as TextAsset;
        if (textFile == null)
            Debug.Log("text not found");

        string[] lines = textFile.text.Split('\n');

        string characterNameTemp = lines[0];
        string characterName = characterNameTemp.Substring(0, characterNameTemp.Length - 1); // Have to trim off carriage char

        string charactertofindTemp = lines[1];
        string charactertofind = charactertofindTemp.Substring(0, charactertofindTemp.Length - 1); // Trim off carriage char

        string bio = lines[2];
        string quest = lines[3];

        int hp = int.Parse(lines[4]);
        int[] stats = Array.ConvertAll(lines[5].Split(' '), int.Parse);
        int[] ability1info = Array.ConvertAll(lines[6].Split(' '), int.Parse);
        int[] ability1 = Array.ConvertAll(lines[7].Split(' '), int.Parse);
        int[] ability2info = Array.ConvertAll(lines[8].Split(' '), int.Parse);
        int[] ability2 = Array.ConvertAll(lines[9].Split(' '), int.Parse);



        //Debug.Log(path);
        //StreamReader reader = new StreamReader(path);

        //string characterName = reader.ReadLine();
        //string charactertofind = reader.ReadLine();
        //int hp = int.Parse(reader.ReadLine());
        //int[] stats = Array.ConvertAll(reader.ReadLine().Split(' '), int.Parse);
        //int[] ability1info = Array.ConvertAll(reader.ReadLine().Split(' '), int.Parse);
        //int[] ability1 = Array.ConvertAll(reader.ReadLine().Split(' '), int.Parse);
        //int[] ability2info = Array.ConvertAll(reader.ReadLine().Split(' '), int.Parse);
        //int[] ability2 = Array.ConvertAll(reader.ReadLine().Split(' '), int.Parse);

        //for (int i = 0; i < stats.Length; i++)
        //    Debug.Log(stats[i]);
        //for (int i = 0; i < ability1info.Length; i++)
        //    Debug.Log(ability1info[i]);

        for (int i = 0; i < ability1.Length; i += 2)
        {
            ability1list.Add(new Point(ability1[i], ability1[i + 1]));
        }
        for (int i = 0; i < ability2.Length; i += 2)
        {
            ability2list.Add(new Point(ability2[i], ability2[i + 1]));
        }

        pc.name = characterName;
        pc.clonename = charactertofind;
        pc.health = hp;
        pc.attributes = stats;
        pc.skill1info = ability1info;
        pc.skill1 = ability1list;
        pc.skill2info = ability2info;
        pc.skill2 = ability2list;

        //reader.Close();

        return pc;
        //return new PlayerClass(characterName, charactertofind, hp, stats, ability1info, ability1list, ability2info, ability2list);
    }
}
