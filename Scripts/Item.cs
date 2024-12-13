using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemType itemType;  // Define if the item is recyclable or not
    public bool isRecyclable;  // Whether the item is recyclable or not
    private TimerModeManager modeManager;

    public enum ItemType
    {
        Recyclable,
        NonRecyclable
    }

    void Start()
    {
        modeManager = FindObjectOfType<TimerModeManager>();
    }

    // This method checks if the item is tapped using raycasting
    public void OnItemTapped()
    {
        if (modeManager == null) return; 

        if (isRecyclable)
        {
            modeManager.OnRecyclableItemClicked(); // Call method to handle recyclable item collection
        }
        else
        {
            modeManager.OnNonRecyclableItemClicked(); // Call method to handle non-recyclable item interaction
        }
        Destroy(gameObject); // Destroy the item after it has been clicked
    }
}
