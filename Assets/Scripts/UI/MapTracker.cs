using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTracker : MonoBehaviour {

	public GameObject Player;
	public GameObject playerPoint;
	public GameObject mapOrigin, mapOrigin1, mapOrigin2;
	public int currentMap = 1;

	void Start(){
		mapOrigin = mapOrigin1;
	}

	public void UpdateMap(){
		playerPoint.transform.position = new Vector2 (mapOrigin.transform.position.x + (Player.transform.position.x *3.5f) , mapOrigin.transform.position.y + (Player.transform.position.y *3.7f));
	}

	public void SwitchMap(){
		if (currentMap == 1) {
			mapOrigin = mapOrigin2;
			currentMap = 2;
		} else {
			mapOrigin = mapOrigin1;
			currentMap = 1;
		}
	}
}
