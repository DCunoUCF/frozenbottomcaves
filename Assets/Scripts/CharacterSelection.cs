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

        pc.txtName = filename;

        string path = "./Assets/Resources/CharacterStats/";

        string path2 = "CharacterStats/";
        path2 += filename;

        List<Point> ability1list = new List<Point>();
        List<Point> ability2list = new List<Point>();
        List<Point> ability3list = new List<Point>();
        List<Point> ability4list = new List<Point>();

        path += filename;
        path += ".txt";
        TextAsset textFile = Resources.Load(path2) as TextAsset;
        if (textFile == null)
            Debug.Log("text not found");

        int i = 0;

        string[] lines = textFile.text.Split('\n');

        string characterNameTemp = lines[i++];
        pc.name = characterNameTemp.Substring(0, characterNameTemp.Length - 1); // Have to trim off carriage char
        
        string charactertofindTemp = lines[i++];
        pc.clonename = charactertofindTemp.Substring(0, charactertofindTemp.Length - 1); // Trim off carriage char

        pc.bio = lines[i++];
        pc.quest = lines[i++];

        int hp = int.Parse(lines[i++]);
        pc.setHealth(hp);

        int[] stats = Array.ConvertAll(lines[i++].Split(' '), int.Parse);
        pc.setStats(stats);

        // Skill information, need to add specific prefabs to load for each skill
        pc.skill1name = lines[i++];
        pc.skill1desc = lines[i++];
        string sk1path = lines[i++];
        pc.cd1 = int.Parse(lines[i++]);
        pc.skill1info = Array.ConvertAll(lines[i++].Split(' '), int.Parse);
        int[] ability1 = Array.ConvertAll(lines[i++].Split(' '), int.Parse);

        pc.skill2name = lines[i++];
        pc.skill2desc = lines[i++];
        string sk2path = lines[i++];
        pc.cd2 = int.Parse(lines[i++]);
        pc.skill2info = Array.ConvertAll(lines[i++].Split(' '), int.Parse);
        int[] ability2 = Array.ConvertAll(lines[i++].Split(' '), int.Parse);

        pc.skill3name = lines[i++];
        pc.skill3desc = lines[i++];
        string sk3path = lines[i++];
        pc.cd3 = int.Parse(lines[i++]);
        pc.skill3info = Array.ConvertAll(lines[i++].Split(' '), int.Parse);
        int[] ability3 = Array.ConvertAll(lines[i++].Split(' '), int.Parse);

        pc.skill4name = lines[i++];
        pc.skill4desc = lines[i++];
        string sk4path = lines[i++];
        pc.cd4 = int.Parse(lines[i++]);
        pc.skill4info = Array.ConvertAll(lines[i++].Split(' '), int.Parse);
        int[] ability4 = Array.ConvertAll(lines[i++].Split(' '), int.Parse);

        // Convert each abilityinfo arr into list of points
        for (int j = 0; j < ability1.Length; j += 2)
        {
            ability1list.Add(new Point(ability1[j], ability1[j + 1]));
        }
        for (int j = 0; j < ability2.Length; j += 2)
        {
            ability2list.Add(new Point(ability2[j], ability2[j + 1]));
        }
        for (int j = 0; j < ability3.Length; j += 2)
        {
            ability3list.Add(new Point(ability3[j], ability3[j + 1]));
        }
        for (int j = 0; j < ability4.Length; j += 2)
        {
            ability4list.Add(new Point(ability4[j], ability4[j + 1]));
        }

        pc.skill1 = ability1list;
        pc.skill2 = ability2list;
        pc.skill3 = ability3list;
        pc.skill4 = ability4list;

        pc.weapon01 = new Weapon(Weapon.WeaponType.KnightSword, "Knight Sword");
        pc.weapon02 = new Weapon(Weapon.WeaponType.KnightShield, "Knight Sheild");

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


        //pc.name = characterName;
        //pc.clonename = charactertofind;
        //pc.setHealth(hp);
        //pc.setStats(stats);
        //pc.skill1info = ability1info;
        //pc.skill1 = ability1list;
        //pc.skill2info = ability2info;
        //pc.skill2 = ability2list;
        //pc.bio = bio;
        //pc.quest = quest;
        //pc.weapon01 = new Weapon(Weapon.WeaponType.KnightSword);
        //pc.weapon02 = new Weapon(Weapon.WeaponType.KnightShield);
        //reader.Close();

        return pc;
        //return new PlayerClass(characterName, charactertofind, hp, stats, ability1info, ability1list, ability2info, ability2list);
    }
}
