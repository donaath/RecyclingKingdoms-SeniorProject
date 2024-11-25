using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWalk : MonoBehaviour
{
    public float moveSpeed = 2f; // Adjust the speed to your liking

    void Update()
    {
        // Move the character forward in the direction they are facing
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
}

