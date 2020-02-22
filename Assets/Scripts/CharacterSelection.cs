using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Drawing;
using System;

public static class CharacterSelection
{
    //public PlayerClass player;
    //private string path = "Assets/Resources/CharacterStats/";

    public static PlayerClass writeStats(string filename)
    {
        string path = "Assets/Resources/CharacterStats/";
        List<Point> ability1list = new List<Point>();
        List<Point> ability2list = new List<Point>();
        path += filename;
        Debug.Log(path);
        StreamReader reader = new StreamReader(path);
        string characterName = reader.ReadLine();
        string charactertofind = reader.ReadLine();
        int hp = int.Parse(reader.ReadLine());
        int[] stats = Array.ConvertAll(reader.ReadLine().Split(' '), int.Parse);
        int[] ability1info = Array.ConvertAll(reader.ReadLine().Split(' '), int.Parse);
        int[] ability1 = Array.ConvertAll(reader.ReadLine().Split(' '), int.Parse);
        int[] ability2info = Array.ConvertAll(reader.ReadLine().Split(' '), int.Parse);
        int[] ability2 = Array.ConvertAll(reader.ReadLine().Split(' '), int.Parse);

        //Debug.Log((characterName));
        //Debug.Log((charactertofind));
        //Debug.Log((hp));
        //Debug.Log((stats));
        //for (int i = 0; i < stats.Length; i++)
        //    Debug.Log(stats[i]);
        //for (int i = 0; i < ability1info.Length; i++)
        //    Debug.Log(ability1info[i]);

        for (int i = 0; i < ability1.Length; i+=2)
        {
            ability1list.Add(new Point(ability1[i], ability1[i + 1]));
        }
        for (int i = 0; i < ability2.Length; i += 2)
        {
            ability2list.Add(new Point(ability2[i], ability2[i + 1]));
        }
        //foreach (Point p in ability1list)
        //    Debug.Log(p);

        //Debug.Log((ability1));
        reader.Close();

        return new PlayerClass(characterName, charactertofind, hp, stats, ability1info, ability1list, ability2info, ability2list);
    }
}
