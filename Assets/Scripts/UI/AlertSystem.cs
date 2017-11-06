using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertSystem : MonoBehaviour {
    private Object alertLock = new Object();
    public GameObject alertFab;
    public GameObject alertTray;
    List<AlertEntry> activeAlerts;
    Queue<string> alertQueue;
    public AudioSource alertPlayer;
    public AudioClip alertSound;

	// Use this for initialization
	void Start () {
        activeAlerts = new List<AlertEntry>();
        alertQueue = new Queue<string>();
	}
	
	// Update is called once per frame
	void Update () {
        UpdateAlerts();
	}

    public void CreateAlert(string message)
    {
        lock (alertLock)
        {
            alertQueue.Enqueue(message);
        }
    }

    void UpdateAlerts()
    {
        if(alertQueue.Count > 0)
        {
            string message;

            lock (alertLock)
            {
                message = alertQueue.Dequeue();
            }

            alertPlayer.PlayOneShot(alertSound, 0.8f);
            GameObject alert = (GameObject)Instantiate(alertFab, transform.position, Quaternion.identity, alertTray.transform);
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
