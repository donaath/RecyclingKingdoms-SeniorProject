using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    // Pauses the game and displays the pause menu
     public void Pause() {
        pauseMenu.SetActive(true); // Shows the pause menu UI
        Time.timeScale = 0; //frezz the game

    }
    // Quits the current game session and loads the home scene
    public void Quit() {
        SceneManager.LoadScene("homeScene"); // Loads the specified scene
        Time.timeScale = 1; //reset game time 

    }
    // Resumes the game by continuing the game where it stop
    public void Resume() {
        pauseMenu.SetActive(false); // hide game menue
        Time.timeScale = 1; // resume the game where player stops


    }
    // Restarts the current game scene
    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);// Reloads the current scene
        Time.timeScale = 1;// Resets game time


    }


}
