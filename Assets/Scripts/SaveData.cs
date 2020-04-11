using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Using this for saving settings for now
public static class SaveData
{

    // Player stuff, planning on using character creation 
    // and filling in appropriate fields w/ this data
    // will need quest if that's changing throughout
    //public string character;
    //public int health;
    //public int[] stats;

    //// Overworld stuff
    //public int node; // node where the game was saved
    //public string previousText; // need some way to save all prev text for history

    // Inventory stuff

    public static float musicVolume = .5f;
    public static float effectsVolume = .5f;
    public static bool musicMute = false;
    public static bool effectsMute = false;
    public static bool hpBar = true;
    public static bool dmgNum = true;

    public static void updateSettings(float mV, float eV, bool mM, bool eM)
    {
        musicVolume = mV;
        effectsVolume = eV;
        musicMute = mM;
        effectsMute = eM;
    }
}
