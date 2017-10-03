using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//To be used with Animations only
public class SoundController : MonoBehaviour
{
	public AudioSource[] audioSources;
	private AudioSource currentAudio;
	private Random randomGenerator = new Random();
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
		currentAudio = audioSources[Mathf.RoundToInt(Random.Range(0, audioSources.Length))];
		currentAudio.Play();
	}
}
