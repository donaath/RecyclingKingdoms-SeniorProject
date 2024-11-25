using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Video;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public Image characterIcon;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueText;
    public Animator animator;

    public VideoPlayer videoPlayer;
    public Button skipButton;
    public GameObject rawImageGameObject;

    public GameObject joystickLeft;
    public GameObject joystickRight;

    public Button mapButton;

    private Queue<DialogueLine> dialogueLines;
    private bool hasStartedDialogue = false;
    private int currentLineIndex = 0;
    public int playVideoAfterLine = 8; // Line index to play the video

    void Start()
    {
        dialogueLines = new Queue<DialogueLine>();
        skipButton.gameObject.SetActive(false);
        skipButton.onClick.AddListener(SkipVideo);

        videoPlayer.Stop();
        videoPlayer.gameObject.SetActive(false);
        rawImageGameObject.SetActive(false);
 
    }

 

    public void StartDialogue(Dialogue dialogue)
    {
        if (!hasStartedDialogue)
        {
            animator.SetBool("IsOpen", true);
            dialogueLines.Clear();

            joystickLeft.SetActive(false);
            joystickRight.SetActive(false);

            foreach (DialogueLine line in dialogue.dialogueLines)
            {
                dialogueLines.Enqueue(line);
            }

            DisplayNextSentence();
            hasStartedDialogue = true;
        }
    }

    public void DisplayNextSentence()
    {
        if (dialogueLines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = dialogueLines.Dequeue();
        currentLineIndex++;

        characterName.text = line.characterName;
        characterIcon.sprite = line.characterIcon;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(line.sentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        if (currentLineIndex == playVideoAfterLine)
        {
            StartCoroutine(PlayVideo());
        }
    }

    private IEnumerator PlayVideo()
    {
        Debug.Log("Video playback starting...");

        // Activate the video UI components
        rawImageGameObject.SetActive(true);
        videoPlayer.gameObject.SetActive(true);

        // Prepare the video player
        videoPlayer.Prepare();
        yield return new WaitUntil(() => videoPlayer.isPrepared);
        videoPlayer.Play();

        // Wait for 10 seconds before enabling the skip button
        yield return new WaitForSeconds(10f);
        skipButton.gameObject.SetActive(true);
        Debug.Log("Skip button enabled after 10 seconds.");

        // Wait until the video finishes playing
        yield return new WaitUntil(() => !videoPlayer.isPlaying);

        // Deactivate UI components after the video ends
        skipButton.gameObject.SetActive(false);
        rawImageGameObject.SetActive(false);
        videoPlayer.gameObject.SetActive(false);
        Debug.Log("Video playback finished.");

        // Transition to the AR scene
        GoToARScene();
    }


    private void SkipVideo()
    {
        Debug.Log("Video skipped by user.");

        videoPlayer.Stop(); // Stop the video
        skipButton.gameObject.SetActive(false); // Hide the skip button
        rawImageGameObject.SetActive(false); // Hide the video UI
        videoPlayer.gameObject.SetActive(false); // Disable the VideoPlayer

        GoToARScene(); // Transition to the AR scene
    }


    private void GoToARScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        Debug.Log($"Going to AR Scene from: {currentScene}");

        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.PreviousScene = currentScene;
        }

        SceneManager.LoadSceneAsync("ArScene");
    }




    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        joystickLeft.SetActive(true);
        joystickRight.SetActive(true);
    }

  
   

   
   
}



