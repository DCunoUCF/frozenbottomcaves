using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Drawing;
using System;

public class CharacterSelection : MonoBehaviour
{
    public PlayerClass player;
    private string path = "Assets/Resources/CharacterStats/";

    public void writeStats(string filename)
    {
        path += filename;
        print(path);
        StreamReader reader = new StreamReader(path);
        for (int i = 0; i < 4; i++)
            print(reader.ReadLine());
        string charactertofind = reader.ReadLine();
        int hp = int.Parse(reader.ReadLine());
        int[] stats = Array.ConvertAll(reader.ReadLine().Split(' '), int.Parse);
        int[] ability1 = Array.ConvertAll(reader.ReadLine().Split(' '), int.Parse);
        print(charactertofind);
        print(hp);
        print(stats);
        print(ability1);
        reader.Close();
    }
}
