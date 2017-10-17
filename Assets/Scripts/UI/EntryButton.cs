using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryButton : AutoJournalEntry {
    public string clueID;
    public Clue_Manager manager;

    public void Select()
    {
        manager.ChooseClue(clueID);
    }
}
