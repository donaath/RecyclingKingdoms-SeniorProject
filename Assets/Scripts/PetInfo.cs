using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetInfo : MonoBehaviour
{
    public string PetID;

    [HideInInspector]
    public string Description;

    public PetInfo(string petID, string description)
    {
        this.PetID = petID;
        this.Description = description;
    }
}
