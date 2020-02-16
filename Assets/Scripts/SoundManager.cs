// SoundManager
// Written By: Christopher Walen

// Has a very basic rotating queue to keep playing whatever tracks we send to it.
// If you want it to loop a single track, just add one track to the queue and it'll play it forever.
// Still working on removing tracks from the queue and unloading them at the same time.
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
	public Queue<AudioClip> effectQueue;

	public AudioSource musicChannel;
	public AudioSource effectChannel;

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

        // Read in volume options from game manager
        	// TODO: read in volume options

        // Make testing music queue
        this.musicQueue.Enqueue(Resources.Load<AudioClip>("Music/Serenity"));
        this.musicQueue.Enqueue(Resources.Load<AudioClip>("Music/Forest_of_the_Elves"));

        Debug.Log("Printing out original queue");
		foreach (AudioClip ac in this.musicQueue)
        {
        	Debug.Log("Found a track in the queue! It's name is "+ac.ToString());
        }

        // Debug.Log("Attempting to remove 'Serenity'");
        // RemoveTrackFromQueue("Serenity");

        // Debug.Log("Printing out modified queue");
        // foreach (AudioClip ac in this.musicQueue)
        // {
        // 	Debug.Log("Found a track in the queue! It's name is "+ac.ToString());
        // }

        Debug.Log("Attempting to add 'Into_Oblivion'");
        AddTrackToQueue("Into_Oblivion");

        Debug.Log("Printing out final queue");
        foreach (AudioClip ac in this.musicQueue)
        {
        	Debug.Log("Found a track in the queue! It's name is "+ac.ToString());
        }

        // Start playing music
        this.musicChannel.loop = false;
        this.musicChannel.clip = musicQueue.Peek();
        this.musicChannel.Play(0);
    }

    void AddTrackToQueue(string trackName)
    {
    	this.musicQueue.Enqueue(Resources.Load<AudioClip>("Music/"+trackName));
    }

    // Use this sparingly as it has to go through the whole queue comparing strings
    // It's also not working right now so please ignore it
    	// TODO: Fix this damn method
    void RemoveTrackFromQueue(string trackName)
    {
		Queue<AudioClip> tempQueue = new Queue<AudioClip>();

		foreach (AudioClip ac in this.musicQueue)
		{
			if (this.musicQueue.Peek().ToString() != trackName)
				tempQueue.Enqueue(this.musicQueue.Dequeue());
		}

		foreach (AudioClip ac in this.musicQueue)
			this.musicQueue.Enqueue(tempQueue.Dequeue());
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
}
