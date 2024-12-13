using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    public TimerModeManager timerModeManager; // Reference to the TimerModeManager script

    // Pauses the game and displays the pause menu
    public void Pause()
    {
        TimerModeManager.isPaused = true; // Set the pause flag to true
        Time.timeScale = 0f; // Stop the game time
        pauseMenu.SetActive(true); // Show the pause menu

        // Stop all sounds in the TimerModeManager
        if (timerModeManager != null)
        {
            if (timerModeManager.audioSource.isPlaying) timerModeManager.audioSource.Pause();
            if (timerModeManager.warningAudioSource.isPlaying) timerModeManager.warningAudioSource.Pause();
            if (timerModeManager.tickAudioSource.isPlaying) timerModeManager.tickAudioSource.Pause();
        }
    }

    // Resumes the game and hides the pause menu
    public void Resume()
    {
        TimerModeManager.isPaused = false; // Set the pause flag to false
        Time.timeScale = 1f; // Resume the game time
        pauseMenu.SetActive(false); // Hide the pause menu

        // Resume all sounds in the TimerModeManager
        if (timerModeManager != null)
        {
            if (timerModeManager.audioSource != null) timerModeManager.audioSource.UnPause();
            if (timerModeManager.warningAudioSource != null) timerModeManager.warningAudioSource.UnPause();
            if (timerModeManager.tickAudioSource != null) timerModeManager.tickAudioSource.UnPause();
        }
    }

    // Quits the current game session and loads the home scene
    public void Quit()
    {
        SceneManager.LoadScene("Main menu"); // Loads the specified scene
        Time.timeScale = 1f; // Reset game time to normal
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reloads the current scene
        Time.timeScale = 1f; // Reset the time scale back to normal
    }
}
