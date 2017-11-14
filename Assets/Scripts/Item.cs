using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour, IInteractable
{

	public GameObject linkedItem;
	public ItemsManager itemsManager;
	public PlayerController player;
	public PickupText pickUpText;
	public int dayToSpawn;
	public int hourToSpawn;
	public GameObject itemSpawner;
    public string[] tags;
    public string clueID;
    ClueFile itemClue;

    void Start()
    {
        foreach(ClueFile clue in DialogueSystem.Instance().items)
        {
            bool match = true;
            foreach(string tag in tags)
            {
                if (!clue.tags.Contains(tag.ToUpper()))
                {
                    match = false;
                }
            }
            if (match)
            {
                itemClue = clue;
            }
        }
    }

	public void Interact()
	{
        if (itemsManager.items.Count < 6)
		{
            //pickUpText.ChangeText(gameObject.name);
            //StartCoroutine("PickupAlert");
            Provider.GetInstance().alertSystem.CreateAlert(gameObject.name + " added to Inventory (I)");
            DialogueSystem.Instance().CreateJournalEntry(itemClue.journalSummary, CharacterID.VICTIM, clueID.ToUpper());
            itemsManager.AddItem(linkedItem);
			itemSpawner.GetComponent<ItemSpawner>().itemsToSpawn.Remove(gameObject);
            DialogueSystem.Instance().PlayItemPickup(itemClue.content);
            Destroy(gameObject);
		}
		else
		{
			pickUpText.ChangeTextAlert();
			Provider.GetInstance().player.EndInteraction();
		}
	}

    IEnumerator PickupAlert()
    {
        Provider.GetInstance().alertSystem.CreateAlert(gameObject.name + " added to Inventory (I)");
        yield return new WaitForSeconds(0.1f);
        DialogueSystem.Instance().CreateJournalEntry(itemClue.journalSummary, CharacterID.VICTIM, clueID.ToUpper());
    }
}
