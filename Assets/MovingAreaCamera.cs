using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingAreaCamera : MonoBehaviour {

	public GameObject cameraAreaLimit;
	private GameObject player;
	private Camera thisCamera;
	private Renderer areaRenderer;
	private float xSize, ySize, xCameraHalfSize, yCameraHalfSize;

	// Use this for initialization
	void Start()
	{
		xSize = areaRenderer.bounds.size.x/2;
		ySize = areaRenderer.bounds.size.y/2;

		xCameraHalfSize = (thisCamera.ViewportToWorldPoint(new Vector3(1, 1, thisCamera.nearClipPlane)).x - thisCamera.ViewportToWorldPoint(new Vector3(0, 0, thisCamera.nearClipPlane)).x)/2;
		yCameraHalfSize = (thisCamera.ViewportToWorldPoint(new Vector3(1, 1, thisCamera.nearClipPlane)).y - thisCamera.ViewportToWorldPoint(new Vector3(0, 0, thisCamera.nearClipPlane)).y)/2;

		thisCamera = GetComponent<Camera>();
		player = GameObject.FindGameObjectWithTag("Player");
		areaRenderer = cameraAreaLimit.GetComponent<Renderer>();

		Vector3 oldPos = transform.position;
		//Move to player position
		transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
		bool checkNorth = thisCamera.ViewportToWorldPoint(new Vector3(1, 1, thisCamera.nearClipPlane)).y < (cameraAreaLimit.transform.position.y + ySize);
		bool checkSouth = thisCamera.ViewportToWorldPoint(new Vector3(0, 0, thisCamera.nearClipPlane)).y > (cameraAreaLimit.transform.position.y - ySize);
		bool checkEast = thisCamera.ViewportToWorldPoint(new Vector3(1, 1, thisCamera.nearClipPlane)).x < (cameraAreaLimit.transform.position.x + xSize);
		bool checkWest = thisCamera.ViewportToWorldPoint(new Vector3(0, 0, thisCamera.nearClipPlane)).x > (cameraAreaLimit.transform.position.x - xSize);
		//If outside north, move to most northern part of area
		if (!checkNorth)
			transform.position = new Vector3(transform.position.x, (cameraAreaLimit.transform.position.y + ySize) - yCameraHalfSize, transform.position.z);
		//If outside south, move to most southern part of area
		if (!checkSouth)
			transform.position = new Vector3(transform.position.x, (cameraAreaLimit.transform.position.y - ySize) + yCameraHalfSize, transform.position.z);
		//If outside east, move to most eastern part of area
		if (!checkEast)
			transform.position = new Vector3((cameraAreaLimit.transform.position.x + xSize) - xCameraHalfSize, transform.position.y, transform.position.z);
		//If outside west, move to most western part of area
		if (!checkWest)
			transform.position = new Vector3((cameraAreaLimit.transform.position.x - xSize) + xCameraHalfSize, transform.position.y, transform.position.z);
	}

	void Awake()
	{
		thisCamera = GetComponent<Camera>();
		player = GameObject.FindGameObjectWithTag("Player");
		areaRenderer = cameraAreaLimit.GetComponent<Renderer>();

	}

	void OnEnable()
	{
		Vector3 oldPos = transform.position;
		//Move to player position
		transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
		bool checkNorth = thisCamera.ViewportToWorldPoint(new Vector3(1, 1, thisCamera.nearClipPlane)).y < (cameraAreaLimit.transform.position.y + ySize);
		bool checkSouth = thisCamera.ViewportToWorldPoint(new Vector3(0, 0, thisCamera.nearClipPlane)).y > (cameraAreaLimit.transform.position.y - ySize);
		bool checkEast = thisCamera.ViewportToWorldPoint(new Vector3(1, 1, thisCamera.nearClipPlane)).x < (cameraAreaLimit.transform.position.x + xSize);
		bool checkWest = thisCamera.ViewportToWorldPoint(new Vector3(0, 0, thisCamera.nearClipPlane)).x > (cameraAreaLimit.transform.position.x - xSize);
		//If outside north, move to most northern part of area
		if (!checkNorth)
			transform.position = new Vector3(transform.position.x, (cameraAreaLimit.transform.position.y + ySize)-yCameraHalfSize, transform.position.z);
		//If outside south, move to most southern part of area
		if (!checkSouth)
			transform.position = new Vector3(transform.position.x, (cameraAreaLimit.transform.position.y - ySize)+yCameraHalfSize, transform.position.z);
		//If outside east, move to most eastern part of area
		if (!checkEast)
			transform.position = new Vector3((cameraAreaLimit.transform.position.x + xSize)-xCameraHalfSize, transform.position.y, transform.position.z);
		//If outside west, move to most western part of area
		if (!checkWest)
			transform.position = new Vector3((cameraAreaLimit.transform.position.x - xSize)+xCameraHalfSize, transform.position.y, transform.position.z);
	}

	// Update is called once per frame
	void Update()
	{
		if (thisCamera.enabled)
		{
			Vector3 oldPos = transform.position;
			//Move to player position
			transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
			bool checkNorth = thisCamera.ViewportToWorldPoint(new Vector3(1, 1, thisCamera.nearClipPlane)).y < (cameraAreaLimit.transform.position.y + ySize);
			bool checkSouth = thisCamera.ViewportToWorldPoint(new Vector3(0, 0, thisCamera.nearClipPlane)).y > (cameraAreaLimit.transform.position.y - ySize);
			bool checkEast = thisCamera.ViewportToWorldPoint(new Vector3(1, 1, thisCamera.nearClipPlane)).x < (cameraAreaLimit.transform.position.x + xSize);
			bool checkWest = thisCamera.ViewportToWorldPoint(new Vector3(0, 0, thisCamera.nearClipPlane)).x > (cameraAreaLimit.transform.position.x - xSize);
			//print("check north" + checkNorth);
			//print("check south" + checkSouth);
			//print("check east" + checkEast);
			//print("check west" + checkWest);
			//If not within the boundaries, move back
			if (!checkNorth || !checkSouth)
				transform.position = new Vector3(transform.position.x, oldPos.y, transform.position.z);
			if (!checkEast || !checkWest)
				transform.position = new Vector3(oldPos.x, transform.position.y, transform.position.z);
		}
	}
}
