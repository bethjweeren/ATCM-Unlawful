using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class HueShift : MonoBehaviour {

    public Color finalColor;
    public float period;
    SpriteRenderer renderer;

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        renderer.color = Color.Lerp(Color.white, finalColor, Mathf.PingPong(Time.time, period));
	}
}
