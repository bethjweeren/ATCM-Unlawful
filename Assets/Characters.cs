using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum CharacterID { BLACK, BLUE, BROWN, GREEN, PURPLE, RED, YELLOW, BODY, INVALID }

public struct Character
{
    public string name, color;

    public Character(string n, string c)
    {
        name = n;
        color = c;
    }
}

public class Characters
{
    public Dictionary<CharacterID, Character> CharacterDict = new Dictionary<CharacterID, Character>();
    public Dictionary<string, CharacterID> NameDict = new Dictionary<string, CharacterID>();

    public Characters()
    {
        CharacterDict.Add(CharacterID.BLACK, new Character("Black", "#191919"));
        NameDict.Add("BLACK", CharacterID.BLACK);

        CharacterDict.Add(CharacterID.BLUE, new Character("Blue", "#193BFF"));
        NameDict.Add("BLUE", CharacterID.BLUE);

        CharacterDict.Add(CharacterID.BROWN, new Character("Brown", "#664028"));
        NameDict.Add("BROWN", CharacterID.BROWN);

        CharacterDict.Add(CharacterID.GREEN, new Character("Green", "#11B211"));
        NameDict.Add("GREEN", CharacterID.GREEN);

        CharacterDict.Add(CharacterID.PURPLE, new Character("Purple", "#AF11A5"));
        NameDict.Add("PURPLE", CharacterID.PURPLE);

        CharacterDict.Add(CharacterID.RED, new Character("Red", "#FF1919"));
        NameDict.Add("RED", CharacterID.RED);

        CharacterDict.Add(CharacterID.YELLOW, new Character("Yellow", "#FFFF32"));
        NameDict.Add("YELLOW", CharacterID.YELLOW);

        CharacterDict.Add(CharacterID.BODY, new Character("Mr. Boddy", "#AD7F61"));
        NameDict.Add("BODY", CharacterID.BODY);

        CharacterDict.Add(CharacterID.INVALID, new Character("Missingno", "#FFFFFF"));
    }

    public CharacterID NameToID(string name)
    {
        CharacterID value = CharacterID.INVALID;
        try
        {
            NameDict.TryGetValue(name, out value);
        }
        catch
        {
            Debug.Log("Invalid dialogue name lookup: " + name);
        }
        return value;
    }

    public Character IDToCharacter(CharacterID name)
    {
        Character value = new Character("Missingno", "#FFFFFF");
        try
        {
            CharacterDict.TryGetValue(name, out value);
        }
        catch
        {
            Debug.Log("Enum not assigned to character: " + name.ToString());
        }
        return value;
    }
}
