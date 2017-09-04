using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls camera switching for one specific area
//Put this script under any area camera
public class CameraSwitcher : MonoBehaviour
{
	//Camera to switch to and camera to switch from when enter area
	public Camera areaCamera, mainCamera;

	// Use this for initialization
	void Start ()
	{
		//Derp de derp
	}

	//Called when player enters area
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag.Equals("Player"))
		{
			areaCamera.enabled = true;
			mainCamera.enabled = false;
		}
	}

	//Called when player exits area
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag.Equals("Player"))
		{
			areaCamera.enabled = false;
			mainCamera.enabled = true;
		}
	}
}
