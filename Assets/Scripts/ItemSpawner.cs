using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
	public List<GameObject> itemsToSpawn; //These objects NEED to have the Item script or else this script will break
	private Time_Manager timeManager;
	private float timeBetweenChecks = .1f;
	// Use this for initialization
	void Start()
	{
        DesignateItems();

		timeManager = GameObject.FindObjectOfType<Time_Manager>();
		foreach (GameObject obj in itemsToSpawn) //Start off with everything deactivated
		{
			obj.SetActive(false);
		}
		StartCoroutine(CheckToSpawn());
	}

    void DesignateItems()
    {
        GameObject weapon = null;
        GameObject leftGlove = null;
        GameObject rightGlove = null;
        GameObject[] opportunities = new GameObject[5];

        foreach(GameObject item in itemsToSpawn)
        {
            Item script = item.GetComponent<Item>();
            if(script.clueID.ToUpper() == "ITEMBLACK")
            {
                opportunities[0] = item;
            }
            else if (script.clueID.ToUpper() == "ITEMBLUE")
            {
                opportunities[1] = item;
            }
            else if (script.clueID.ToUpper() == "ITEMGREEN")
            {
                opportunities[2] = item;
            }
            else if (script.clueID.ToUpper() == "ITEMRED")
            {
                opportunities[3] = item;
            }
            else if (script.clueID.ToUpper() == "ITEMYELLOW")
            {
                opportunities[4] = item;
            }
            else if (script.clueID.ToUpper() == "WEAPON")
            {
                weapon = item;
            }
            else if(script.clueID.ToUpper() == "GLOVE")
            {
                List<string> scriptTags = new List<string>(script.tags);
                if(scriptTags.Contains("left") || scriptTags.Contains("Left") || scriptTags.Contains("LEFT"))
                {
                    leftGlove = item;
                }
                else
                {
                    rightGlove = item;
                }
            }
        }

        
        if(DialogueSystem.Instance().WasKilledWithRightHand())
        {
            if (leftGlove != null)
            {
                itemsToSpawn.Remove(leftGlove);
                Destroy(leftGlove);
            }
            
        }
        else
        {
            if (rightGlove != null)
            {
                itemsToSpawn.Remove(rightGlove);
                Destroy(rightGlove);
            }
        }

        //Remove killer and one other person's alibi item
        Scenario scenario = DialogueSystem.Instance().scenario;
        Suspect other = scenario.killer;
        while (other == scenario.killer)
        {
            other = (Suspect)Random.Range(0, 5);
        }
        itemsToSpawn.Remove(opportunities[(int)other]);
        Destroy(opportunities[(int)other]);

        itemsToSpawn.Remove(opportunities[(int)scenario.killer]);
        Destroy(opportunities[(int)scenario.killer]);

    }

	//If an item should spawn, spawn it. (Er, I mean enable it)
	IEnumerator CheckToSpawn()
	{
		foreach (GameObject obj in itemsToSpawn)
		{
			Item i = obj.GetComponent<Item>();
			//If the object actually exists
			if (i != null)
			{
				//Check if it's the right time to spawn it
				if ((i.dayToSpawn <= timeManager.day) && (i.hourToSpawn <= timeManager.hour))
				{
					obj.SetActive(true); //Spawn the object (Well the object's already there; you're just enabling it.)
				}
			}
			else
				print("The item " + obj + " does not have the Item script, so it can't be spawned!");
		}

		//Wait the time between checks (for efficiency reasons)
		yield return new WaitForSeconds(timeBetweenChecks);
		StartCoroutine(CheckToSpawn());
	}
}