using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	private GameObject player;

	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, this.transform.position.z);
	}
	
	// Update is called once per frame
	void Update ()
	{
		this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, this.transform.position.z);
	}
}
