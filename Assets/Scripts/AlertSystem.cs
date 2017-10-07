using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertSystem : MonoBehaviour {
    private Object alertLock = new Object();
    public GameObject alertFab;
    List<AlertEntry> activeAlerts;
    public AudioSource alertPlayer;
    public AudioClip alertSound;

	// Use this for initialization
	void Start () {
        activeAlerts = new List<AlertEntry>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            CreateAlert(Random.value.ToString());
        }
	}

    public void CreateAlert(string message)
    {
        lock (alertLock)
        {
            alertPlayer.PlayOneShot(alertSound, 0.8f);
            GameObject alert = (GameObject)Instantiate(alertFab, transform.position, Quaternion.identity, transform.parent);
            AlertEntry alertScript = alert.GetComponent<AlertEntry>();
            alertScript.alertText.text = message;

            foreach (AlertEntry a in activeAlerts)
            {
                a.Bump();
            }
            activeAlerts.Add(alertScript);
        }
    }

    public void RemoveAlert(AlertEntry alert)
    {
        activeAlerts.Remove(alert);
    }
}
