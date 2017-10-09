using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertEntry : MonoBehaviour {
    public Text alertText;
    public Image panelBackground;
    RectTransform myTransform;
    float startTime;
    bool killing;
    int maxLifetime = 10;
    int maxCapacity = 2;
    int targetPosition;
    float height = 25;
    float dropSpeed = 50;   //25 pixels in .5 seconds

	// Use this for initialization
	void Start () {
        myTransform = GetComponent<RectTransform>();
        startTime = Time.time;
        targetPosition = -1;
        Bump();
	}
	
	// Update is called once per frame
	void Update () {
		if (myTransform.position.y > (-25) * targetPosition)
        {
            float y = myTransform.localPosition.y - dropSpeed * Time.deltaTime;
            if(y < (-25) * targetPosition)
            {
                y = (-25) * targetPosition;
            }
            myTransform.localPosition = new Vector2(0, y);
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
