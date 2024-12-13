using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    public TimerModeManager timerModeManager; 

    public void Pause()
    {
        TimerModeManager.isPaused = true; 
        Time.timeScale = 0f; // Stop the game time
        pauseMenu.SetActive(true); 

        // Stop all sounds in the TimerModeManager
        if (timerModeManager != null)
        {
            if (timerModeManager.audioSource.isPlaying) timerModeManager.audioSource.Pause();
            if (timerModeManager.warningAudioSource.isPlaying) timerModeManager.warningAudioSource.Pause();
            if (timerModeManager.tickAudioSource.isPlaying) timerModeManager.tickAudioSource.Pause();
        }
    }

    
    public void Resume()
    {
        TimerModeManager.isPaused = false; 
        Time.timeScale = 1f;
        pauseMenu.SetActive(false); 

        
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
