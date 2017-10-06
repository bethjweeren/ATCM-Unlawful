using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertEntry : MonoBehaviour {
    public Text alertText;
    public Image panelBackground;
    float startY;
    float startTime;
    bool killing;
    int maxLifetime = 10;
    int maxCapacity = 3;
    int targetPosition;
    float height = 25;
    float dropSpeed = 50;   //25 pixels in .5 seconds

	// Use this for initialization
	void Start () {
        startTime = Time.time;
        startY = transform.position.y;
        targetPosition = 0;
        Bump();
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y > (-25) * targetPosition + startY)
        {
            float y = transform.position.y - dropSpeed * Time.deltaTime;
            if(y < (-25) * targetPosition + startY)
            {
                y = (-25) * targetPosition + startY;
            }
            transform.position = new Vector2(transform.position.x, y);
        }
        if (!killing && (targetPosition > maxCapacity || Time.time - startTime > maxLifetime))
        {
            killing = true;
            StartCoroutine("Kill");
        }
	}

    public void Bump()
    {
        targetPosition += 1;
    }

    private IEnumerator Kill()
    {
        float killStartTime = Time.time;
        while(Time.time - killStartTime < 0.5f)
        {
            float a = this.panelBackground.color.a - 0.06f;
            if( a < 0)
            {
                a = 0;
            }
            this.panelBackground.color = new Color(panelBackground.color.r, panelBackground.color.g, panelBackground.color.b, a);
            yield return new WaitForEndOfFrame();
        }
        Provider.GetInstance().alertSystem.RemoveAlert(this);
        Destroy(this.gameObject);
    }
}
