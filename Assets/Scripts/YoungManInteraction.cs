using UnityEngine;

public class YoungManInteraction : MonoBehaviour
{
    [SerializeField] private GameObject player;              // Reference to the player GameObject
    [SerializeField] private float detectionAngle = 45f;     // Angle for detection
    [SerializeField] private float detectionDistance = 5f;   // Distance for detection
    [SerializeField] private Dialogue dialogue;              // Reference to the Dialogue scriptable object

    private bool hasInteracted = false;

    void Update()
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;
        float angle = Vector3.Angle(directionToPlayer, transform.forward);

        // Check if the player is within detection range, angle, and hasn't interacted yet
        if (!hasInteracted && angle <= detectionAngle && directionToPlayer.magnitude <= detectionDistance)
        {
            hasInteracted = true;  // Ensure the interaction triggers only once
            TriggerDialogue();
        }
    }

    private void TriggerDialogue()
    {
        // Start the dialogue through the DialogueManager
        FindObjectOfType<DialogueManager1>().StartDialogue(dialogue);
    }
}

