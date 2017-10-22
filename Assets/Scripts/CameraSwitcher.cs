using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Controls camera switching for one specific area
//Put this script under any area camera
//To-Do: Change the name of this class to AreaSwitcher or something
//since it also changes player speed and disables the roof
public class CameraSwitcher : MonoBehaviour
{
	//Camera to switch to and camera to switch from when enter area
	public GameObject areaCameraObject;
	public Camera mainCamera; //Use GameObjects not actual Camera objects because MovingAreaCamera script uses OnEnable
	public GameObject roof; //Optional, should NOT be collideable
	public GameObject interior; //Optional if area doesn't have roof
	public GameObject blackoutOverlay; //Optional
	public GameObject fade;
	private float fadeIncrement = .05f; //Lower the number, slower/more gradual the fade to and from black
	private float slowDown = 1.5f; //How much to slow down player on entry. This variable is private because it's a pain to change for each one, and it helps keeps things consistent
	private PlayerController playerController;
	private List<SpriteRenderer> stuffToCover = new List<SpriteRenderer>();
	private CanvasRenderer fadeRenderer;

	// Use this for initialization
	void Start()
	{
		fadeRenderer = fade.GetComponent<CanvasRenderer>();
		if (roof != null)
			roof.SetActive(true); //So we don't need these enabled in the scene because they get in the way
		if (interior != null)
		{
			foreach (SpriteRenderer sr in interior.GetComponentsInChildren<SpriteRenderer>())
			{
				stuffToCover.Add(sr);
			}
		}
		if (interior != null && stuffToCover != null)
		{
			foreach (SpriteRenderer sr in stuffToCover)
			{
				sr.enabled = false;
			}
		}
	}

	//Called when player enters area
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag.Equals("Player"))
		{
			StartCoroutine(FadeToArea(other));
		}
		//The following probably isn't necessary because the RenderLayer script should automatically hide moving things like NPCs
		/*
		else
		{
			if (interior != null && stuffToCover != null)
			{
				SpriteRenderer sr = other.GetComponent<SpriteRenderer>();
				stuffToCover.Add(sr);
				sr.enabled = false;
			}
		}
		*/
	}

	//Called when player exits area
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag.Equals("Player"))
		{
			StartCoroutine(FadeToMain(other));
		}
		//The following probably isn't necessary because the RenderLayer script should automatically hide moving things like NPCs
		/*
		else
		{
			if (interior != null && stuffToCover != null)
			{
				SpriteRenderer sr = other.GetComponent<SpriteRenderer>();
				stuffToCover.Remove(sr);
				sr.enabled = true;
			}
		}
		*/
	}

	IEnumerator FadeToArea(Collider2D other)
	{
		//Fade to black
		for (float f = 0f; f <= 1f; f += fadeIncrement)
		{
			fadeRenderer.SetAlpha(f);
			//Color c = fadeImage.material.color;
			//c.a = f;
			//fadeImage.material.color = c;
			yield return null;
		}

		//Wait, then switch to area camera
		//yield return new WaitForSeconds(.1f);
		playerController = other.GetComponent<PlayerController>();
		mainCamera.enabled = false;
		areaCameraObject.SetActive(true);

		if (roof != null)
			roof.SetActive(false); //Roof disappears and player can enter.
		if (blackoutOverlay != null)
			blackoutOverlay.SetActive(true); //Blackout appears
		playerController.SetSpeed(playerController.speed - slowDown); //Slow player down when they enter area
		if (interior != null && stuffToCover != null)
		{
			for (int i = 0; i < stuffToCover.Count; i++)
			{
				SpriteRenderer sr = stuffToCover[i];
				if (sr != null)
					sr.enabled = true;
				else
					stuffToCover.Remove(sr);
			}
		}

		//Fade to area camera
		for (float f = 1f; f >= 0; f -= fadeIncrement)
		{
			fadeRenderer.SetAlpha(f);
			//Color c = fadeImage.material.color;
			//c.a = f;
			//fadeImage.material.color = c;
			yield return null;
		}
	}

	IEnumerator FadeToMain(Collider2D other)
	{
		//Fade to black
		for (float f = 0f; f <= 1f; f += fadeIncrement)
		{
			fadeRenderer.SetAlpha(f);
			yield return null;
		}

		//Wait, then switch to main camera
		//yield return new WaitForSeconds(.1f);
		playerController = other.GetComponent<PlayerController>();
		mainCamera.enabled = true;
		areaCameraObject.SetActive(false);

		if (roof != null)
			roof.SetActive(true); //Roof covers area again.
		if (blackoutOverlay != null)
			blackoutOverlay.SetActive(false); //Blackout disappears again
		playerController.SetSpeed(playerController.speed + slowDown); //Bring player back to normal speed when they leave
		if (interior != null && stuffToCover != null)
		{
			for (int i = 0; i < stuffToCover.Count; i++)
			{
				SpriteRenderer sr = stuffToCover[i];
				if (sr != null)
					sr.enabled = false;
				else
					stuffToCover.Remove(sr);
			}
		}

		//Fade to main camera
		for (float f = 1f; f >= 0; f -= fadeIncrement)
		{
			fadeRenderer.SetAlpha(f);
			yield return null;
		}
	}
}
