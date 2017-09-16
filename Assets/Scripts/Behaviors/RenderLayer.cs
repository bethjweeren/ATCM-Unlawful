﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class RenderLayer : MonoBehaviour {
    private SpriteRenderer myRenderer;
    private Collider2D myCollider;
    private bool OnAboveLayer;

	// Use this for initialization
	void Start () {
        myRenderer = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<Collider2D>();
        OnAboveLayer = true;
        myRenderer.sortingLayerName = "AbovePlayer";
    }
	
	// Update is called once per frame
	void Update () {
		if(OnAboveLayer && PlayerController.playerY < transform.position.y + myCollider.offset.y)
        {
            OnAboveLayer = false;
            myRenderer.sortingLayerName = "Below Player";
        }
        else if(!OnAboveLayer && PlayerController.playerY > transform.position.y + myCollider.offset.y)
        {
            OnAboveLayer = true;
            myRenderer.sortingLayerName = "Above Player";
        }
	}
}
