﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    public string CharacterID;

    [HideInInspector]
    public string Description;

    public CharacterInfo(string characterID, string description)
    {
        this.CharacterID = characterID;
        this.Description = description;
    }
}
