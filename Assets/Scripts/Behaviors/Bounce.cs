using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour {
    public float speed;
    public float dist;

	void Update () {
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, Mathf.PingPong(speed * Time.time, dist) - (dist/2));
	}
}
