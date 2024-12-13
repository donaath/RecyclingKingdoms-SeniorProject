using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;
using System.Diagnostics;
using UnityEngine.PlayerLoop;

public class TimerModeManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject howToPlayScreen; // Panel with instructions
    public Button startButton;         // Start button
    public TMP_Text countdownText;     // Countdown display text
    public TMP_Text timerText;         // Timer display text
    public GameObject[] itemPrefabs;   // Array of item prefabs (recyclable and non-recyclable)
    public GameObject pauseMenu; // Reference to the pause menu

    [Header("Game Settings")]
    public float gameTime = 60f;       // Total game time in seconds
    private int recycleStreak = 0;     // Counter for consecutive recyclable items clicked
    public float minDistanceBetweenItems = 1.5f; // Min distance to prevent overlapping
    private List<Vector3> spawnedPositions = new List<Vector3>(); // Track spawned item positions
    private bool warningSoundPlayed = false; // Flag to ensure warning sound plays once

    [Header("AR Settings")]
    [SerializeField] ARRaycastManager raycastManager; // AR Raycast Manager
    [SerializeField] Camera arCamera;                // AR camera for raycasting
    [SerializeField] private ARAnchorManager anchorManager;// Reference to ARAnchorManager

    [Header("Spawn Area Settings")]
    public float spawnRadius = 3f; // Circle radius for spawning

    [Header("Audio Settings")]
    public AudioClip countdownSound3;  // Sound for "3"
    public AudioClip countdownSound2;  // Sound for "2"
    public AudioClip countdownSound1;  // Sound for "1"
    public AudioClip timerTickSound;    // Sound for each timer tick
    public AudioClip warningSound;      // Sound for the last 10-second warning
    public AudioSource audioSource;    // Audio source for playing sounds
    public AudioSource warningAudioSource;
    public AudioSource tickAudioSource;
    public AudioClip comboBonusSound; // Sound for combo bonus
    public AudioClip penaltySound;    // Sound for penalty

    private GameObject spawnedObject; // To track the spawned object
    public bool isGameRunning = true; // Control when the game is running
    public static bool isPaused = false; // Static variable to track pause state
    private float startTime; // Declare startTime as a class variable
    private int score = 0;  // Player's score
    public TMP_Text scoreText;  // Reference to the score UI element

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        pauseMenu.SetActive(false); // Ensure pause menu is hidden at the start
        scoreText.gameObject.SetActive(false);  // Hide score text initially

        // Create audio sources for specific sounds
        warningAudioSource = gameObject.AddComponent<AudioSource>();
        tickAudioSource = gameObject.AddComponent<AudioSource>();

        // Show How to Play screen and assign the button's click listener
        howToPlayScreen.SetActive(true);
        startButton.onClick.AddListener(StartButtonPressed);
    }

    public void StartButtonPressed()
    {
        UnityEngine.Debug.Log("Start button pressed!"); // Log message for debugging
        howToPlayScreen.SetActive(false);   // Hide instructions screen
        countdownText.gameObject.SetActive(true); // Show countdown
        StartCoroutine(StartCountdown());   // Start countdown coroutine
    }

    private IEnumerator StartCountdown()
    {
        // Play countdown number 3
        countdownText.text = "3"; // Update countdown text
        if (countdownSound3 != null && !audioSource.isPlaying) // Play sound for 3
        {
            audioSource.Stop(); // Stop any previously playing sound
            audioSource.PlayOneShot(countdownSound3); // Play countdown sound for 3
        }
        yield return new WaitForSeconds(1); // Fixed wait time to align with countdown text

        // Play countdown number 2
        countdownText.text = "2"; // Update countdown text
        if (countdownSound2 != null && !audioSource.isPlaying) // Play sound for 2
        {
            audioSource.Stop(); // Stop any previously playing sound
            audioSource.PlayOneShot(countdownSound2); // Play countdown sound for 2
        }
        yield return new WaitForSeconds(1); // Fixed wait time to align with countdown text

        // Play countdown number 1
        countdownText.text = "1"; // Update countdown text
        if (countdownSound1 != null && !audioSource.isPlaying) // Play sound for 1
        {
            audioSource.Stop(); // Stop any previously playing sound
            audioSource.PlayOneShot(countdownSound1); // Play countdown sound for 1
        }
        yield return new WaitForSeconds(1); // Fixed wait time to align with countdown text

        countdownText.gameObject.SetActive(false); // Hide countdown text after the countdown
        StartGame(); // Start the game after the countdown finishes
    }

    private void StartGame()
    {
        warningSoundPlayed = false; // Reset warning flag for the new game
        isGameRunning = true;
        startTime = Time.time; // Initialize startTime when the game starts

        scoreText.gameObject.SetActive(true);  // Show score text
        scoreText.text = "Score: 0";  // Initialize score text

        InvokeRepeating("SpawnARItem", 0, 1f); // Spawn items every 1 seconds
        StartCoroutine(GameTimer());         // Start the main game timer
        pauseMenu.SetActive(true);// Ensure pause menu can be used during the game
    }

    private IEnumerator GameTimer()
    {
        while (gameTime > 0 && isGameRunning)
        {
            float previousTime = Mathf.Floor(gameTime); // Store the rounded value before decrement
            gameTime -= Time.deltaTime; // Decrement using delta time
            float currentTime = Mathf.Floor(gameTime); // Rounded value after decrement

            // Update UI only when the displayed time changes
            if (currentTime != previousTime)
            {
                if (gameTime <= 10 && !warningSoundPlayed)
                {
                    timerText.color = Color.red; // Change timer color for last 10 seconds

                    if (warningSound != null && !warningAudioSource.isPlaying)
                    {
                        warningAudioSource.PlayOneShot(warningSound); // Play warning sound
                    }

                    warningSoundPlayed = true; // Prevent repeated plays
                }
                else if (gameTime > 10 && timerTickSound != null && !tickAudioSource.isPlaying)
                {
                    tickAudioSource.PlayOneShot(timerTickSound); // Play timer tick sound
                }

                timerText.text = $"Time: {currentTime.ToString("F0")}"; // Update timer display
            }

            yield return null; // Wait until the next frame
        }
        EndGame();
    }

    // Method called when a recyclable item is clicked
    public void OnRecyclableItemClicked()
    {
        if (!isGameRunning) return;

        recycleStreak++;
        score += 10;  // Add point
        scoreText.text = $"Score: {score}";  // Update score UI

        if (recycleStreak == 3)
        {
            gameTime += 5; // Add 5 seconds for a streak of 3 recyclables
            audioSource.PlayOneShot(comboBonusSound); // Play combo sound
            recycleStreak = 0; // Reset streak
        }
        else if (recycleStreak > 3)
        {
            gameTime += 5; //increase the bonus time after multiple combos
        }

        UpdateTimerDisplay();
    }

    // Method called when a non-recyclable item is clicked
    public void OnNonRecyclableItemClicked()
    {
        if (!isGameRunning) return;

        gameTime -= 5; // Deduct 5 seconds 
        recycleStreak = 0; // Reset streak

        score -= 5;  // Deduct points 
        if (score < 0) score = 0;  // Ensure score doesn't go negative
        scoreText.text = $"Score: {score}";  // Update score UI

        audioSource.PlayOneShot(penaltySound); // Play penalty sound
        UpdateTimerDisplay();
    }

    // Update the timer UI display
    private void UpdateTimerDisplay()
    {
        timerText.text = $"Time: {gameTime.ToString("F0")}";
    }

    private void EndGame()
    {
        isGameRunning = false;
        CancelInvoke("SpawnARItem");  // Stop spawning items
        spawnedPositions.Clear();  // Clear tracked positions

        timerText.text = "Time's Up!";  // Display end of game message
        scoreText.text = $"Score: {score}";

        // Stop the ticking sound when the game ends
        if (tickAudioSource.isPlaying)
        {
            tickAudioSource.Stop();
        }

        // Move TimerText slightly above the center
        StartCoroutine(MoveTextToCenter(timerText.GetComponent<RectTransform>(), new Vector2(0, 50)));

        // Move ScoreText slightly below the center
        StartCoroutine(MoveTextToCenter(scoreText.GetComponent<RectTransform>(), new Vector2(0, -50)));

        UnityEngine.Debug.Log("Game Over!");

    // Show the final score
    UnityEngine.Debug.Log($"Final Score: {score}");
        scoreText.text = $"Final Score: {score}";
    }

    // Coroutine to move UI element to the center with vertical offset
    private IEnumerator MoveTextToCenter(RectTransform textTransform, Vector2 offset)
    {
        // Define target position (center of the parent canvas + offset)
        Vector2 targetPosition = offset;

        // Get the starting position
        Vector2 startPosition = textTransform.anchoredPosition;

        // Define the duration for the animation
        float duration = 1f;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            textTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / duration);
            yield return null;
        }

        // Ensure it snaps to the final position
        textTransform.anchoredPosition = targetPosition;
    }

    private IEnumerator FadeInText(TextMeshProUGUI text, float duration)
    {
        Color originalColor = text.color;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, t / duration);
            yield return null;
        }
        text.color = originalColor; // Ensure final color is set
    }

    public void SpawnARItem()
    {
        Vector3 randomPosition;
        int maxAttempts = 10;
        int attempts = 0;
        bool validPosition = false;

        do
        {
            // Step 1: Generate a random position within the circular spawn area
            float angle = UnityEngine.Random.Range(0f, 2 * Mathf.PI);
            float distance = UnityEngine.Random.Range(0f, spawnRadius);
            float x = Mathf.Cos(angle) * distance;
            float z = Mathf.Sin(angle) * distance;

            randomPosition = new Vector3(x, 0, z);

            // Step 2: Check if position is far enough from existing items
            validPosition = true;
            foreach (Vector3 pos in spawnedPositions)
            {
                if (Vector3.Distance(randomPosition, pos) < minDistanceBetweenItems)
                {
                    validPosition = false;
                    break;
                }
            }
            attempts++;
        } while (!validPosition && attempts < maxAttempts);


        // Step 3: Spawn the item
        int randomIndex = UnityEngine.Random.Range(0, itemPrefabs.Length);
        GameObject item = Instantiate(itemPrefabs[randomIndex], randomPosition, Quaternion.identity);
        item.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);

        // Track the position of the spawned item
        spawnedPositions.Add(randomPosition);
    }

    private void OnDrawGizmos()
    {
        if (!isGameRunning) return;

        // Draw a circle to visualize the spawn area
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(Vector3.zero, spawnRadius);
    }
}
   