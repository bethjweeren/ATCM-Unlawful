using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSoundSwitcher : MonoBehaviour
{

	private SoundController sc;

	// Use this for initialization
	void Start()
	{
		sc = GetComponentInParent<SoundController>();
	}

	void OnTriggerExit2D(Collider2D other)
	{
		Collider2D[] overlappingColliders = new Collider2D[20];
		ContactFilter2D cf = new ContactFilter2D();
		cf.NoFilter();
		GetComponent<Collider2D>().OverlapCollider(cf, overlappingColliders);

		foreach (Collider2D oc in overlappingColliders)
		{
			if (oc != null && oc.gameObject.CompareTag("BGDirt"))
			{
				sc.currentClips = sc.dirtSteps;
				break;
			}
		}
		foreach (Collider2D oc in overlappingColliders)
		{
			if (oc != null && oc.gameObject.CompareTag("BGCobblestone"))
			{
				sc.currentClips = sc.cobblestoneSteps;
				break;
			}
		}
		foreach (Collider2D oc in overlappingColliders)
		{
			if (oc != null && oc.gameObject.CompareTag("BGWood"))
			{
				sc.currentClips = sc.woodSteps;
				break;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Collider2D[] overlappingColliders = new Collider2D[20];
		ContactFilter2D cf = new ContactFilter2D();
		cf.NoFilter();
		GetComponent<Collider2D>().OverlapCollider(cf, overlappingColliders);

		foreach (Collider2D oc in overlappingColliders)
		{
			if (oc != null && oc.gameObject.CompareTag("BGDirt"))
			{
				sc.currentClips = sc.dirtSteps;
				break;
			}
		}
		foreach (Collider2D oc in overlappingColliders)
		{
			if (oc != null && oc.gameObject.CompareTag("BGCobblestone"))
			{
				sc.currentClips = sc.cobblestoneSteps;
				break;
			}
		}
		foreach (Collider2D oc in overlappingColliders)
		{
			if (oc != null && oc.gameObject.CompareTag("BGWood"))
			{
				sc.currentClips = sc.woodSteps;
				break;
			}
		}
		/*
		if (other.gameObject.CompareTag("BGWood"))
		{
			print("collision enter wood");
			currentClips = woodSteps;
		}
		else if (other.gameObject.CompareTag("BGCobblestone"))
		{
			print("collision enter stone");
			currentClips = cobblestoneSteps;
		}
		else if (other.gameObject.CompareTag("BGDirt"))
		{
			print("collision enter dirt");
			currentClips = dirtSteps;
		}
		*/
	}
}