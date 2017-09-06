using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum CharacterID { INVALID, BLACK, BLUE, BROWN, GREEN, PURPLE, RED, YELLOW, PLAYER, VICTIM1, VICTIM2}

public struct Character
{
    public string name, color;
    public Sprite thumb;

    public Character(string n, string c, Sprite t)
    {
        name = n;
        color = c;
        thumb = t;
    }

    public Character(string n, string c)
    {
        name = n;
        color = c;
        thumb = Characters.defaultThumb;
    }
}

public class Characters
{
    public static Sprite defaultThumb = Resources.Load<Sprite>("Sprites/NonPlayer/Error_thumb");
    public Dictionary<CharacterID, Character> CharacterDict = new Dictionary<CharacterID, Character>();
    public Dictionary<string, CharacterID> NameDict = new Dictionary<string, CharacterID>();

    public Characters()
    {
        CharacterDict.Add(CharacterID.BLACK, new Character("Friar Black", "#191919", Resources.Load<Sprite>("Sprites/NonPlayer/Black_thumb")));
        NameDict.Add("BLACK", CharacterID.BLACK);

        CharacterDict.Add(CharacterID.BLUE, new Character("Blue", "#193BFF", Resources.Load<Sprite>("Sprites/NonPlayer/Blue_thumb")));
        NameDict.Add("BLUE", CharacterID.BLUE);

        CharacterDict.Add(CharacterID.BROWN, new Character("The Beggar", "#664028"));
        NameDict.Add("BROWN", CharacterID.BROWN);

        CharacterDict.Add(CharacterID.GREEN, new Character("Greensworth Greenston", "#11B211", Resources.Load<Sprite>("Sprites/NonPlayer/Green_thumb")));
        NameDict.Add("GREEN", CharacterID.GREEN);

        CharacterDict.Add(CharacterID.PURPLE, new Character("Purple", "#AF11A5"));
        NameDict.Add("PURPLE", CharacterID.PURPLE);

        CharacterDict.Add(CharacterID.RED, new Character("Red", "#FF1919", Resources.Load<Sprite>("Sprites/NonPlayer/Red_thumb")));
        NameDict.Add("RED", CharacterID.RED);

        CharacterDict.Add(CharacterID.YELLOW, new Character("Yellow", "#FFFF32", Resources.Load<Sprite>("Sprites/NonPlayer/Yellow_thumb")));
        NameDict.Add("YELLOW", CharacterID.YELLOW);

        CharacterDict.Add(CharacterID.VICTIM1, new Character("Mr. Boddy", "#AD7F61"));
        NameDict.Add("VICTIM1", CharacterID.VICTIM1);

        CharacterDict.Add(CharacterID.INVALID, new Character("Missingno", "#FFFFFF"));
    }

    public CharacterID NameToID(string name)
    {
        CharacterID value = CharacterID.INVALID;
        try
        {
            if(!NameDict.TryGetValue(name, out value))
            {
                Debug.Log("Invalid dialogue name lookup: " + name);
                value = CharacterID.INVALID;
            }
            
        }
        catch
        {
            Debug.Log("Characters dictionary emtpy");
        }
        return value;
    }

    public Character IDToCharacter(CharacterID name)
    {
        Character value = new Character("Missingno", "#FFFFFF");
        try
        {
            if(!CharacterDict.TryGetValue(name, out value))
            {
                Debug.Log("Error looking up nickname: " + name.ToString());
                value = new Character("Missingno", "#FFFFFF");
            }
        }
        catch
        {
            Debug.Log("Nicnkame dictionary empty");
        }
        return value;
    }
}
