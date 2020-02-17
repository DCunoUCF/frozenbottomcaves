﻿// SoundManager
// Written By: Christopher Walen

// Has a very basic rotating queue to keep playing whatever tracks we send to it.
// If you want it to loop a single track, just add one track to the queue and it'll play it forever.
// Music tracks can be added, removed, and unloaded. Loaded tracks that are removed from the play queue
// are put into the 'loadedMusic' queue such that if it needs to be played again, we're not reloading it.
// More functionality to come...

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	//============   Unity Methods   ============//

    // Start is called before the first frame update
    void Start()
    {
        // Default volume options
        this.musicVolume = 1.0f;
        this.effectsVolume = 1.0f;
        this.musicMute = false;
        this.effectsMute = false;

        // Create the music queue and effect queue
        this.musicQueue = new Queue<AudioClip>();
        this.effectQueue = new Queue<AudioClip>();
        this.loadedMusicQueue = new Queue<AudioClip>();
        this.loadedEffectQueue = new Queue<AudioClip>();

        // Read in volume options from game manager
        	// TODO: read in volume options

        // Make testing music queue
        AddTrackToQueue("Serenity");
        AddTrackToQueue("Forest_of_the_Elves");

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
        AddTrackToQueue("Into_Oblivion");
        // Debug.Log("Attempting to add 'Serenity'");
        // AddTrackToQueue("Serenity");

        // Debug.Log("Printing out final queue");
        // foreach (AudioClip ac in this.musicQueue)
        // {
        // 	Debug.Log("Found a track in the queue! It's name is '"+ac.name+"'");
        // }

        // Start playing music
        this.musicChannel.loop = false; // Default -> no 
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

    void LoadTrack(string trackName)
    {
    	this.loadedMusicQueue.Enqueue(Resources.Load<AudioClip>("Sound/Music/"+trackName));
    }

    // Checks loaded queue first before fresh-loading asset
    void AddTrackToQueue(string trackName)
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
    void RemoveTrackFromQueue(string trackName)
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

    void PlayNextTrack()
    {
    	this.musicChannel.Stop();
    	this.musicQueue.Enqueue(this.musicQueue.Dequeue());
    	this.musicChannel.clip = this.musicQueue.Peek();
    	this.musicChannel.Play(0);
    }

    void EnableMusicLoop()
    {
    	this.musicChannel.loop = true;
    }

    void DisableMusicLoop()
    {
    	this.musicChannel.loop = false;
    }

    void ToggleMusicLoop()
    {
    	this.musicChannel.loop = !this.musicChannel.loop;
    }

    void FreeAllMusicTracks()
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
    }

    //=============   Sound Effect Methods   =============//

    // TODO:
    // Need to figure out if effects should be done differently

    //
    void AddEffectToQueue(string effectName)
    {
    	this.effectQueue.Enqueue(Resources.Load<AudioClip>("Sound/Effects/"+effectName));
    }
}