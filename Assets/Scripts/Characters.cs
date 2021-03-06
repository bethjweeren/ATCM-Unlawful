﻿using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum CharacterID { INVALID, BLACK, BLUE, BROWN, GREEN, PURPLE, RED, YELLOW, PLAYER, VICTIM, VICTIM2, RANDO, WEAPON, OTHER}

public struct Character
{
    public string name, color, identifier;
    public Sprite thumb;

    public Character(string n, string i, string c, Sprite t)
    {
        name = n;
        identifier = i;
        color = c;
        thumb = t;
    }

    public Character(string n, string i, string c)
    {
        name = n;
        identifier = i;
        color = c;
        thumb = Characters.defaultThumb;
    }
}

public class Characters
{
    private const string thumbPath = "Sprites/NonPlayer/";
    public static Sprite defaultThumb = Resources.Load<Sprite>(thumbPath + "Error_thumb");
    public Dictionary<CharacterID, Character> CharacterDict = new Dictionary<CharacterID, Character>();
    public Dictionary<string, CharacterID> NameDict = new Dictionary<string, CharacterID>();

    public Characters()
    {
        CharacterDict.Add(CharacterID.BLACK, new Character("Friar Noir", "BLACK", "#000000", Resources.Load<Sprite>(thumbPath + "Black_thumb")));
        NameDict.Add("BLACK", CharacterID.BLACK); //191919

        CharacterDict.Add(CharacterID.BLUE, new Character("Bleu", "BLUE", "#28b1ff", Resources.Load<Sprite>(thumbPath + "Blue_thumb")));
        NameDict.Add("BLUE", CharacterID.BLUE);

        CharacterDict.Add(CharacterID.BROWN, new Character("Beggar Marron", "BROWN", "#41270e", Resources.Load<Sprite>(thumbPath + "Brown_thumb")));
        NameDict.Add("BROWN", CharacterID.BROWN);

        CharacterDict.Add(CharacterID.GREEN, new Character("Vert", "GREEN", "#61ec52", Resources.Load<Sprite>(thumbPath + "Green_thumb")));
        NameDict.Add("GREEN", CharacterID.GREEN);

        CharacterDict.Add(CharacterID.PURPLE, new Character("Mayor Violet", "PURPLE", "#63046c", Resources.Load<Sprite>(thumbPath + "Purple_thumb")));
        NameDict.Add("PURPLE", CharacterID.PURPLE);

        CharacterDict.Add(CharacterID.RED, new Character("Rouge", "RED", "#d72c28", Resources.Load<Sprite>(thumbPath + "Red_thumb")));
        NameDict.Add("RED", CharacterID.RED);

        CharacterDict.Add(CharacterID.OTHER, new Character("Him", "OTHER", "#FF8411"));
        NameDict.Add("ORANGE", CharacterID.OTHER);

        CharacterDict.Add(CharacterID.YELLOW, new Character("Jaune", "YELLOW", "#fcff03", Resources.Load<Sprite>(thumbPath + "Yellow_thumb")));
        NameDict.Add("YELLOW", CharacterID.YELLOW);

        CharacterDict.Add(CharacterID.VICTIM, new Character("Dr. Mort", "VICTIM", "#B9977C"));
        NameDict.Add("VICTIM", CharacterID.VICTIM);

        CharacterDict.Add(CharacterID.INVALID, new Character("Missingno", "INVALID", "#FFFFFF"));

        CharacterDict.Add(CharacterID.PLAYER, new Character("Detective", "PLAYER", "#FFFFFF"));
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
        Character value = new Character("Missingno", "INVALID", "#FFFFFF");
        try
        {
            if(!CharacterDict.TryGetValue(name, out value))
            {
                Debug.Log("Error looking up nickname: " + name.ToString());
                value = new Character("Missingno", "INVALID", "#FFFFFF");
            }
        }
        catch
        {
            Debug.Log("Nicnkame dictionary empty");
        }
        return value;
    }

    public static bool IsSuspect(CharacterID id)
    {
        switch (id)
        {
            case CharacterID.BLACK:
            case CharacterID.BLUE:
            case CharacterID.GREEN:
            case CharacterID.RED:
            case CharacterID.YELLOW:
                return true;
            default:
                return false;
        }
    }
}
