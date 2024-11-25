using UnityEngine;
using System.Collections;

public class OldManInteraction : MonoBehaviour
{
    [SerializeField] private GameObject player;              // Reference to the player GameObject
    [SerializeField] private GameObject chair;               // Reference to the chair GameObject
    [SerializeField] private Animator oldManAnimator;        // Reference to the old man's Animator
    [SerializeField] private float detectionAngle = 45f;     // Angle for detection
    [SerializeField] private float detectionDistance = 5f;   // Distance for detection
    [SerializeField] private Dialogue dialogue;               // Reference to the Dialogue scriptable object

    private bool isChairActive = false;

    void Update()
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;
        float angle = Vector3.Angle(directionToPlayer, transform.forward);

        // Check if the player is within detection range and angle
        if (angle <= detectionAngle && directionToPlayer.magnitude <= detectionDistance)
        {
            if (!isChairActive)
            {
                isChairActive = true;
                oldManAnimator.SetBool("isTalking ", true); // Start talking animation
                StartCoroutine(ShowChairWithDelay(0.1f));  // Activate chair after a delay
                TriggerDialogue();  // Trigger dialogue
            }
        }
    }

    private IEnumerator ShowChairWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        chair.SetActive(true);

        // Delay before starting dialogue to ensure animation has time to play
        yield return new WaitForSeconds(0.5f);
        TriggerDialogue();
    }


    private void TriggerDialogue()
    {
        // Start the dialogue when the old man interacts
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
