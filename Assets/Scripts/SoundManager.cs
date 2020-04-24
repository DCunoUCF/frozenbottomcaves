// SoundManager
// Written By: Christopher Walen

// Has a very basic rotating queue to keep playing whatever tracks we send to it.
// If you want it to loop a single track, just add one track to the queue and it'll play it forever.
// Music tracks can be added, removed, and unloaded. Loaded tracks that are removed from the play queue
// are put into the 'loadedMusic' queue such that if it needs to be played again, we're not reloading it.
// More functionality to come...

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SoundManager : MonoBehaviour
{
	public float musicVolume;
	public float effectsVolume;
	public bool musicMute;
	public bool effectsMute;

	public Queue<AudioClip> musicQueue;
	public Queue<AudioClip> loadedMusicQueue;
	public Queue<AudioClip> effectQueue;
	public Queue<AudioClip> loadedEffectQueue;

	public AudioSource musicChannel;
	public AudioSource effectChannel;

    private AudioClip winJingle;
    private AudioClip loseJingle;
    public AudioClip hit;
    public AudioClip miss;
    public AudioClip clash;
    public AudioClip pieceLanding1;
    public AudioClip pieceLanding2;
    public AudioClip pieceLanding3;
    public AudioClip pieceLanding4;
    public AudioClip collide;
    public AudioClip fall;
    public AudioClip thud;
    public AudioClip dogYelp;
    public AudioClip hiss;
    public AudioClip rumble;
    public AudioClip cackle;
    public AudioClip loudCackle;
    public AudioClip wolfHowl;
    public AudioClip wolfSnarl;
    public AudioClip wolfBite;
    public AudioClip wolfWhine;
    public AudioClip personHowl;
    public AudioClip gnomeOof;
    public AudioClip tidalWave;
    public AudioClip metalThud;

    //============   Constructors   ============//

    public SoundManager(AudioSource mc, AudioSource ec)
    {
        this.musicChannel = mc;
        this.effectChannel = ec;
    }

	//============   Unity Methods   ============//

	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}

    // Start is called before the first frame update
    void Start()
    {
        // Default volume options
        //this.musicVolume = 0.2f; // CHANGED TO 0 TO PRESERVE MY SANITY
        //this.effectsVolume = 0.5f;
        //this.musicMute = true;
        //this.effectsMute = false;
        this.updateFromSaveData();
        SaveData.updateSettings(.2f, .5f, false, false);
        // Create the music queue and effect queue
        this.musicQueue = new Queue<AudioClip>();
        this.effectQueue = new Queue<AudioClip>();
        this.loadedMusicQueue = new Queue<AudioClip>();
        this.loadedEffectQueue = new Queue<AudioClip>();

        // Read in volume options from game manager
        	// TODO: read in volume options

        // setMusicFromDirectory("ForestOverworldMusic");
        setForestMusic();

        // Make testing music queue
            // AddTrackToQueue("Serenity");
            // AddTrackToQueue("Forest_of_the_Elves");

  		// Debug.Log("Printing out original queue");
		// foreach (AudioClip ac in this.musicQueue)
  		//       {
  		//       	Debug.Log("Found a track in the queue! It's name is '"+ac.name+"'");
  		//       }

        // Debug.Log("Attempting to remove 'Serenity'");
        // RemoveTrackFromQueue("Serenity");

        // Debug.Log("Printing out modified queue");
        // foreach (AudioClip ac in this.musicQueue)
        // {
        // 	Debug.Log("Found a track in the queue! It's name is '"+ac.name+"'");
        // }

        // Debug.Log("Attempting to add 'Into_Oblivion'");
            // AddTrackToQueue("Into_Oblivion");
        // Debug.Log("Attempting to add 'Serenity'");
        // AddTrackToQueue("Serenity");

        // Debug.Log("Printing out final queue");
        // foreach (AudioClip ac in this.musicQueue)
        // {
        // 	Debug.Log("Found a track in the queue! It's name is '"+ac.name+"'");
        // }

        // Find and load every sound effect into soundEffectQueue
        this.winJingle = Resources.Load<AudioClip>("Sound/Effects/WinJelly");
        this.loseJingle = Resources.Load<AudioClip>("Sound/Effects/LossJelly");
        this.hit = Resources.Load<AudioClip>("Sound/Effects/Hit");
        this.miss = Resources.Load<AudioClip>("Sound/Effects/Miss");
        this.clash = Resources.Load<AudioClip>("Sound/Effects/Clash");
        this.pieceLanding1 = Resources.Load<AudioClip>("Sound/Effects/Piece_Landing1");
        this.pieceLanding2 = Resources.Load<AudioClip>("Sound/Effects/Piece_Landing2");
        this.pieceLanding3 = Resources.Load<AudioClip>("Sound/Effects/Piece_Landing3");
        this.pieceLanding4 = Resources.Load<AudioClip>("Sound/Effects/Piece_Landing4");
        this.collide = Resources.Load<AudioClip>("Sound/Effects/Collide");
        this.fall = Resources.Load<AudioClip>("Sound/Effects/Fall");
        this.thud = Resources.Load<AudioClip>("Sound/Effects/Thud");
        this.dogYelp = Resources.Load<AudioClip>("Sound/Effects/DogYelp");
        this.hiss = Resources.Load<AudioClip>("Sound/Effects/Hiss");
        this.rumble = Resources.Load<AudioClip>("Sound/Effects/Rumble");
        this.cackle = Resources.Load<AudioClip>("Sound/Effects/Cackle");
        this.loudCackle = Resources.Load<AudioClip>("Sound/Effects/LoudCackle");
        this.wolfHowl = Resources.Load<AudioClip>("Sound/Effects/WolfHowl");
        this.wolfSnarl = Resources.Load<AudioClip>("Sound/Effects/WolfSnarl");
        this.wolfBite = Resources.Load<AudioClip>("Sound/Effects/WolfBite");
        this.wolfWhine = Resources.Load<AudioClip>("Sound/Effects/WolfWhine");
        this.personHowl = Resources.Load<AudioClip>("Sound/Effects/PersonHowl");
        this.gnomeOof = Resources.Load<AudioClip>("Sound/Effects/GnomeOof");
        this.tidalWave = Resources.Load<AudioClip>("Sound/Effects/TidalWave");
        this.metalThud = Resources.Load<AudioClip>("Sound/Effects/MetalThud");

        // Start playing music
        this.musicChannel.loop = true; // Default -> yes
        this.musicChannel.clip = musicQueue.Peek();
        this.musicChannel.Play(0);
    }

    // Update is called once per frame
    void Update()
    {
    	// Handle music mute and volume
        if (this.musicMute)
        	this.musicChannel.volume = 0;
        else
        {
        	if (this.musicChannel.volume != this.musicVolume)
        		this.musicChannel.volume = this.musicVolume;
        }

        // Handle effects mute and volume
        if (this.effectsMute)
        	this.effectChannel.volume = 0;
        else
        {
        	if (this.effectChannel.volume != this.effectsVolume)
        		this.effectChannel.volume = this.effectsVolume;
        }

        // Change music tracks if need be
        if (!this.musicChannel.isPlaying)
        {
        	AudioClip temp = this.musicQueue.Dequeue();
        	this.musicQueue.Enqueue(temp);
        	this.musicChannel.clip = this.musicQueue.Peek();
        	this.musicChannel.Play(0);
        }

        // Play sound effects here
    }

    //=============   Music Track Methods   =============//
    public void playWinJingle()
    {
        this.effectChannel.PlayOneShot(this.winJingle, this.effectsVolume);
    }

    public void playLoseJingle()
    {
        this.effectChannel.PlayOneShot(this.loseJingle, this.effectsVolume);
    }

    public void setBattleMusic()
    {
        FreeAllMusicTracks();

        AddTrackToQueue("ForestBattleMusic/The_Great_Battle");

        Debug.Log(this.musicQueue.ToString());
    }

    public void setForestMusic()
    {
        FreeAllMusicTracks();

        AddTrackToQueue("ForestOverworldMusic/Serenity");
        AddTrackToQueue("ForestOverworldMusic/Into_Oblivion");
        AddTrackToQueue("ForestOverworldMusic/Forest_of_the_Elves");
    }

    public void setMusicFromDirectory(string folder)
    {
        FreeAllMusicTracks();

        var info = new DirectoryInfo("Assets/Resources/Sound/Music/"+folder);

        foreach (FileInfo file in info.GetFiles())
        {
            string fName = file.Name.Substring(0, file.Name.Length-4);
            // Debug.Log(fName);
            AddTrackToQueue(folder+"/"+fName);
        }

        Debug.Log(this.musicQueue.ToString());
    }

    public void updateMusicList()
    {
        // Check for battleworld and switch music if we're there
        switch (SceneManager.GetActiveScene().name)
        {
            case "Battleworld":
                FreeAllMusicTracks();

                AddTrackToQueue("The_Great_Battle");

                Debug.Log(this.musicQueue.ToString());
                break;
            default:
                FreeAllMusicTracks();

                AddTrackToQueue("Serenity");
                AddTrackToQueue("Into_Oblivion");
                AddTrackToQueue("Forest_of_the_Elves");
                break;
        }
    }

    public void setMusicVolume(float vol)
    {
        this.musicVolume = vol;
        this.musicChannel.volume = vol;
        Debug.Log("The music channel is now playing at "+this.musicChannel.volume);
    }

    public float getMusicVolume()
    {
        return this.musicVolume;
    }

    public void setEffectVolume(float vol)
    {
        this.effectsVolume = vol;
        this.effectChannel.volume = vol;
    }

    public float getEffectVolume()
    {
        return this.effectsVolume;
    }

    public void setMusicMute(bool mute)
    {
        this.musicMute = mute;
        this.musicChannel.mute = mute;
    }

    public bool getMusicMute()
    {
        return this.musicMute;
    }

    public void setEffectMute(bool mute)
    {
        this.effectsMute = mute;
        this.effectChannel.mute = mute;
    }

    public bool getEffectMute()
    {
        return this.effectsMute;
    }

    public void setAudioChannels(AudioSource mc, AudioSource ec)
    {
        this.musicChannel = mc;
        this.effectChannel = ec;
    }

    public void setMusicChannel(AudioSource mc)
    {
        this.musicChannel = mc;
    }

    public void setEffectChannel(AudioSource ec)
    {
        this.effectChannel = ec;
    }

    public void LoadTrack(string trackName)
    {
    	this.loadedMusicQueue.Enqueue(Resources.Load<AudioClip>("Sound/Music/"+trackName));
    }

    // Checks loaded queue first before fresh-loading asset
    public void AddTrackToQueue(string trackName)
    {
    	AudioClip track = null;

    	for (int i = 0; i < this.loadedMusicQueue.Count; i++)
    	{
    		if (this.loadedMusicQueue.Peek().name == trackName)
    			track = this.loadedMusicQueue.Dequeue();
    		else
    			this.loadedMusicQueue.Enqueue(this.loadedMusicQueue.Dequeue());
    	}

    	if (track != null)
    	{
    		// Debug.Log("Loaded from load-queue!");
    		this.musicQueue.Enqueue(track);
    	}
    	else
    	{
    		// Debug.Log("Freshly loaded from assets!");

	    	this.musicQueue.Enqueue(Resources.Load<AudioClip>("Sound/Music/"+trackName));
    	}
    }

	// Use this sparingly as it has to go through the whole queue comparing strings
    // Moves the named track into the loaded queue
    public void RemoveTrackFromQueue(string trackName)
    {
		// Go through each track in the music queue
		// and move the one we want to remove to the loaded queue,
		// cycling through all the other tracks
		for (int i = 0; i < musicQueue.Count; i++)
		{
			if (this.musicQueue.Peek().name != trackName)
				this.musicQueue.Enqueue(this.musicQueue.Dequeue());
			else
			{
				Debug.Log(this.musicQueue.Peek().ToString());
				this.loadedMusicQueue.Enqueue(this.musicQueue.Dequeue());
			}
		}
    }

    public void PlayNextTrack()
    {
    	this.musicChannel.Stop();
    	this.musicQueue.Enqueue(this.musicQueue.Dequeue());
    	this.musicChannel.clip = this.musicQueue.Peek();
    	this.musicChannel.Play(0);
    }

    public void EnableMusicLoop()
    {
    	this.musicChannel.loop = true;
    }

    public void DisableMusicLoop()
    {
    	this.musicChannel.loop = false;
    }

    public void ToggleMusicLoop()
    {
    	this.musicChannel.loop = !this.musicChannel.loop;
    }

    public void FreeAllMusicTracks()
    {
    	for (int i = 0; i < this.musicQueue.Count; i++)
    	{
    		Resources.UnloadAsset(this.musicQueue.Dequeue());
    	}

    	for (int i = 0; i < this.loadedMusicQueue.Count; i++)
    	{
    		Resources.UnloadAsset(this.loadedMusicQueue.Dequeue());
    	}

    	for (int i = 0; i < this.effectQueue.Count; i++)
    	{
    		Resources.UnloadAsset(this.effectQueue.Dequeue());
    	}

        this.musicQueue.Clear();
        this.loadedMusicQueue.Clear();
        this.effectQueue.Clear();
    }

    //=============   Sound Effect Methods   =============//

    // TODO:
    // Need to figure out if effects should be done differently

    //
    public void AddEffectToQueue(string effectName)
    {
    	this.effectQueue.Enqueue(Resources.Load<AudioClip>("Sound/Effects/"+effectName));
    }

    public void updateFromSaveData()
    {
        this.musicVolume = SaveData.musicVolume;
        this.effectsVolume = SaveData.effectsVolume;
        this.musicMute = SaveData.musicMute;
        this.effectsMute = SaveData.effectsMute;
    }
}
