using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum Names { BLACK, BLUE, BROWN, GREEN, PURPLE, RED, YELLOW, BODY, INVALID }

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
    public Dictionary<Names, Character> CharacterDict = new Dictionary<Names, Character>();
    public Dictionary<string, Names> NameDict = new Dictionary<string, Names>();

    public Characters()
    {
        CharacterDict.Add(Names.BLACK, new Character("Black", "#191919"));
        NameDict.Add("BLACK", Names.BLACK);

        CharacterDict.Add(Names.BLUE, new Character("Blue", "#193BFF"));
        NameDict.Add("BLUE", Names.BLUE);

        CharacterDict.Add(Names.BROWN, new Character("Brown", "#664028"));
        NameDict.Add("BROWN", Names.BROWN);

        CharacterDict.Add(Names.GREEN, new Character("Green", "#11B211"));
        NameDict.Add("GREEN", Names.GREEN);

        CharacterDict.Add(Names.PURPLE, new Character("Purple", "#AF11A5"));
        NameDict.Add("PURPLE", Names.PURPLE);

        CharacterDict.Add(Names.RED, new Character("Red", "#FF1919"));
        NameDict.Add("RED", Names.RED);

        CharacterDict.Add(Names.YELLOW, new Character("Yellow", "#FFFF32"));
        NameDict.Add("YELLOW", Names.YELLOW);

        CharacterDict.Add(Names.BODY, new Character("Mr. Boddy", "#AD7F61"));
        NameDict.Add("BODY", Names.BODY);

        CharacterDict.Add(Names.INVALID, new Character("Missingno", "#FFFFFF"));
    }

    public Names StringToName(string name)
    {
        Names value = Names.INVALID;
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

    public Character NameToCharacter(Names name)
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
