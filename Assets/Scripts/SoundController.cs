using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//To be used with Animations only
//How to set up:
//Step 1: Make a Sounds object with all the audio sources (footsteps sounds)
//Step 2: Put this script inside the Sounds object
//Step 3: Drag and drop the footsteps/audio sources into the Audio Sources array
//Step 4: Move this script inside the NPC object so the animator will find it
public class SoundController : MonoBehaviour
{
	public AudioSource[] audioSources;
	public bool soundEnabled = true;
	private AudioSource currentAudio;
	// Use this for initialization
	void Start ()
	{
		foreach (AudioSource ac in audioSources)
		{
			ac.loop = false;
			ac.playOnAwake = false;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void ToggleRandom()
	{
		if (soundEnabled)
		{
			currentAudio = audioSources[Mathf.RoundToInt(Random.Range(0, audioSources.Length))];
			currentAudio.Play();
		}
	}
}
