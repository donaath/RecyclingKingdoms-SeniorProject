using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SkipVideo : MonoBehaviour
{
    public Button Button;

    private void Start()
    {
        if (Button == null)
        {
            Debug.LogError("Button is not assigned in the Inspector.");
            return;
        }

        // Ensure this GameObject is active before starting the coroutine
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(EnableButtonAfterDelay());
        }
        else
        {
            Debug.LogError("Skip GameObject is inactive. Coroutine cannot start.");
        }
    }

    private IEnumerator EnableButtonAfterDelay()
    {
        yield return new WaitForSeconds(10.0f); // Wait for 10 seconds
        Button.gameObject.SetActive(true); // Enable the button after the delay
    }

    public void Skip()
    {
        SceneManager.LoadScene(8);
    }
}
