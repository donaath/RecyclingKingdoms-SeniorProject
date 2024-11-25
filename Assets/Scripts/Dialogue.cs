using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class DialogueLine
{
    public string characterName; // The name of the character speaking
    public Sprite characterIcon;  // The icon of the character
    [TextArea(3, 10)]
    public string sentence;        // The dialogue sentence
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>(); // List of dialogue lines
}


