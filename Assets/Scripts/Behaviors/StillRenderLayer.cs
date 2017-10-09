using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]//, typeof(Collider2D))]
public class StillRenderLayer : MonoBehaviour {
	public bool useCenterOfCollider = false;
    private SpriteRenderer myRenderer;
    private Collider2D myCollider;

	// Use this for initialization
	void Start () {
        myRenderer = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<Collider2D>();
		myRenderer.sortingLayerName = "Default";
		if ((myCollider != null) && useCenterOfCollider)
			myRenderer.sortingOrder = Mathf.FloorToInt((myCollider.bounds.center.y) * -100);
		else
			myRenderer.sortingOrder = Mathf.FloorToInt((transform.position.y - (myRenderer.bounds.size.y/2)) * -100);
	}
}
