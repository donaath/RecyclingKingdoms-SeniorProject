using UnityEngine;
using UnityEngine.SceneManagement; // Import Scene Management namespace

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        Plastic,
        Paper
    }

    public ItemType itemType;  // Define the type of the item (Plastic or Paper)
    private Vector3 initialPosition; // Store the initial position of the item for resetting
    private bool isDragging = false;

    private Renderer itemRenderer; // Renderer to change item color
    private Color originalColor; // Store the original color of the item

    private static int itemCount = 4; // Static variable to track the number of items in the scene

    private void Start()
    {
        initialPosition = transform.position; // Save the starting position
        itemRenderer = GetComponent<Renderer>(); // Get the Renderer component
        originalColor = itemRenderer.material.color; // Save the original color
    //    itemCount++; // Increment the item count when a new item is created
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
        if (other.CompareTag("PlasticBin"))
        {
            if (itemType == ItemType.Plastic)
            {
                Debug.Log("Correct item dropped in Plastic Bin!");
                ChangeItemColor(Color.green); // Set to green
                DestroyItemAfterFeedback(); // Handle item destruction
            }
            else
            {
                Debug.Log("Wrong item dropped in Plastic Bin!");
                ChangeItemColor(Color.red); // Set to red
                Invoke(nameof(ResetPosition), 0.5f); // Reset after feedback
            }
        }
        else if (other.CompareTag("PaperBin"))
        {
            if (itemType == ItemType.Paper)
            {
                Debug.Log("Correct item dropped in Paper Bin!");
                ChangeItemColor(Color.green); // Set to green
                DestroyItemAfterFeedback(); // Handle item destruction
            }
            else
            {
                Debug.Log("Wrong item dropped in Paper Bin!");
                ChangeItemColor(Color.red); // Set to red
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

    private void DestroyItemAfterFeedback()
    {
        Destroy(gameObject, 0.5f); // Destroy the item after feedback
        itemCount--; // Decrease the item count

        if (itemCount <= 0)
        {
            Debug.Log("All items sorted! Moving to the next level...");
            SceneManager.LoadScene("NextLevelScene"); // Replace with your actual scene name
        }
    }

    //private void LoadNextScene()
    //{
    //    SceneManager.LoadScene("NextLevelScene"); // Replace with your actual scene name
    //}
}
