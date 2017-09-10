using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls camera switching for one specific area
//Put this script under any area camera
//To-Do: Change the name of this class to AreaSwitcher or something
//since it also changes player speed and disables the roof
public class CameraSwitcher : MonoBehaviour
{
	//Camera to switch to and camera to switch from when enter area
	public Camera areaCamera, mainCamera;
	public GameObject roof; //Optional, should NOT be collideable
	private float slowDown = 1.5f; //This variable is private because it's a pain to change for each one, and it helps keeps things consistent
	private PlayerController playerController;

	// Use this for initialization
	void Start ()
	{
		//derp de derp
	}

	//Called when player enters area
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag.Equals("Player"))
		{
			playerController = other.GetComponent<PlayerController>();
			areaCamera.enabled = true;
			mainCamera.enabled = false;
			if (roof != null)
				roof.SetActive(false); //Roof disappears and player can enter.
			playerController.SetSpeed(playerController.speed - slowDown); //Slow player down when they enter area
		}
	}

	//Called when player exits area
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag.Equals("Player"))
		{
			areaCamera.enabled = false;
			mainCamera.enabled = true;
			if (roof != null)
				roof.SetActive(true); //Roof covers area again.
			playerController.SetSpeed(playerController.speed + slowDown); //Bring player back to normal speed when they leave
		}
	}
}
