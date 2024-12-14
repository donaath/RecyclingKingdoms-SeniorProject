using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager1 : MonoBehaviour
{
    public Image characterIcon;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueText;
    public Animator animator;

    public GameObject joystickLeft;
    public GameObject joystickRight;

    private Queue<DialogueLine> dialogueLines;
    private bool hasStartedDialogue = false;
    private int currentLineIndex = 0;

    void Start()
    {
        dialogueLines = new Queue<DialogueLine>();
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
    }

    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        GoToARScene();
    }

        private void GoToARScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        Debug.Log($"Going to AR Scene from: {currentScene}");

        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.PreviousScene = currentScene;
        }

        SceneManager.LoadSceneAsync(11);
    }
}
