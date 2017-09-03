using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArea : MonoBehaviour
{

	public Camera areaCamera;

	// Use this for initialization
	void Start ()
	{
		
	}

	public Camera getCamera()
	{
		return areaCamera;
	}

	//Called when player enters area
	void OnTriggerEnter(Collider other)
	{
		//if (other.tag == "Player")
		//	areaCamera.
	}

	//Called when player enters area
	void OnTriggerExit(Collider other)
	{

	}
}
