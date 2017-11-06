using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//To be used with Animations only
//How to set up:
//Step 1: Make a Sounds object with all the audio sources (footsteps sounds)
//Step 2: Put this script inside the Sounds object
//Step 3: Drag and drop the footsteps/audio sources into the Audio Sources array
//Step 4: Move this script inside the NPC object so the animator will find it
[RequireComponent(typeof(AudioSource))]
public class SoundController : MonoBehaviour
{
	public AudioClip[] cobblestoneSteps, dirtSteps, woodSteps;
	public bool soundEnabled = true;
	private AudioSource audioSource;
	public AudioClip[] currentClips; //Current audio sources
	private AudioClip currentClip;

	// Use this for initialization
	void Start ()
	{
		audioSource = GetComponent<AudioSource>();

		Collider2D[] overlappingColliders = new Collider2D[20];
		ContactFilter2D cf = new ContactFilter2D();
		cf.NoFilter();
		GetComponent<Collider2D>().OverlapCollider(cf, overlappingColliders);

		currentClips = cobblestoneSteps;
		foreach (Collider2D oc in overlappingColliders)
		{

			if (oc != null && oc.gameObject.CompareTag("BGDirt"))
			{
				currentClips = dirtSteps;
				break;
			}
		}
		foreach (Collider2D oc in overlappingColliders)
		{

			if (oc != null && oc.gameObject.CompareTag("BGCobblestone"))
			{
				currentClips = cobblestoneSteps;
				break;
			}
		}
		foreach (Collider2D oc in overlappingColliders)
		{

			if (oc != null && oc.gameObject.CompareTag("BGWood"))
			{
				currentClips = woodSteps;
				break;
			}
		}
		audioSource.loop = false;
		audioSource.playOnAwake = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void ToggleRandom()
	{
		if (soundEnabled)
		{
			currentClip = currentClips[Mathf.RoundToInt(Random.Range(0, currentClips.Length))];
			audioSource.clip = currentClip;
			audioSource.Play();
		}
	}
}
