using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemQuest : MonoBehaviour
{
    public enum ItemType
    {
        Plastic,
        Paper,
        Organic
    }

    public ItemType itemType;  // Define the type of the item (Plastic, Paper, or Organic)
    private Vector3 initialPosition; // Store the initial position of the item for resetting
    private bool isDragging = false;

    private Renderer itemRenderer; // Renderer to change item color
    private Color originalColor; // Store the original color of the item

    private static int organicItemCount = 0; // Counter for collected organic items
    private const int targetOrganicCount = 4; // Required number of organic items to complete the game

    private void Start()
    {
        initialPosition = transform.position; // Save the starting position
        itemRenderer = GetComponent<Renderer>(); // Get the Renderer component
        originalColor = itemRenderer.material.color; // Save the original color
    }

    private void Update()
    {
        // Handle dragging
        if (isDragging)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z; // Depth correction
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = new Vector3(worldPosition.x, worldPosition.y, transform.position.z); // Move in world space
        }
    }

    private void OnMouseDown()
    {
        // Start dragging
        isDragging = true;
    }

    private void OnMouseUp()
    {
        // Stop dragging
        isDragging = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("OrganicBin"))
        {
            if (itemType == ItemType.Organic)
            {
                Debug.Log("Correct item dropped in Organic Bin!");
                ChangeItemColor(Color.green); // Set to green for correct feedback
                IncreaseOrganicCounter(); // Count only organic items
                DestroyItemAfterFeedback(); // Handle item destruction
            }
            else
            {
                Debug.Log("Wrong item dropped in Organic Bin!");
                ChangeItemColor(Color.red); // Set to red for wrong feedback
                Invoke(nameof(ResetPosition), 0.5f); // Reset after feedback
            }
        }
    }

    private void ResetPosition()
    {
        transform.position = initialPosition; // Reset the item to its starting position
        ChangeItemColor(originalColor); // Reset the color to the original
        Debug.Log("Item reset to its initial position.");
    }

    private void ChangeItemColor(Color color)
    {
        itemRenderer.material.color = color; // Change the material's color
    }

    private void IncreaseOrganicCounter()
    {
        organicItemCount++; // Increment the organic item count
        Debug.Log($"Organic items collected: {organicItemCount}/{targetOrganicCount}");

        if (organicItemCount >= targetOrganicCount)
        {
            Debug.Log("All required organic items collected! Moving to the next level...");
            //Invoke(nameof(LoadNextScene), 0.75f); // Short delay before loading the next scene
            SceneManager.LoadScene("NextLevelScene"); // Replace with your actual scene name

        }
    }

    private void DestroyItemAfterFeedback()
    {
        Destroy(gameObject, 0.5f); // Destroy the item after feedback
    }

    //private void LoadNextScene()
    //{
    //    SceneManager.LoadScene("NextLevelScene"); // Replace with your actual scene name
    //}
}
