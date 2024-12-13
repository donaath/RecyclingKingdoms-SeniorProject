using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine;

public class ARItemInteraction : MonoBehaviour
{
    [SerializeField] private ARRaycastManager raycastManager; // AR Raycast Manager for raycasting
    [SerializeField] private Camera arCamera;                // AR Camera for raycasting
    [SerializeField] private LayerMask itemLayerMask;        // Layer mask for item layers
    [SerializeField] private TimerModeManager timerModeManager;

    void Update()
    {
        if (!timerModeManager.isGameRunning || Input.touchCount == 0) return;
        {
            Touch touch = Input.GetTouch(0);

            // Check if the user touched a valid AR plane or object
            if (touch.phase == TouchPhase.Began)
                if (touch.phase == TouchPhase.Began)
                {
                    Ray ray = arCamera.ScreenPointToRay(touch.position);
                    RaycastHit hit;

                    // Only consider objects in the ARItems layer
                    int layerMask = 1 << LayerMask.NameToLayer("ARItems");

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                    {
                        UnityEngine.Debug.Log($"Raycast hit: {hit.collider.name}");

                        // Handle item interaction
                        Item item = hit.collider.GetComponent<Item>();
                        if (item != null)
                        {
                            item.OnItemTapped();
                        }
                    }
                    else
                    {
                        UnityEngine.Debug.Log("No AR item hit.");
                    }
                }
        }
    }

private void TrySelectItem(Vector2 touchPosition)
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        raycastManager.Raycast(touchPosition, hits, TrackableType.AllTypes); // Raycast all trackables

        if (hits.Count > 0)
        {
            ARRaycastHit hit = hits[0];  // First hit

            GameObject selectedObject = hit.trackable.gameObject;

            // Ensure we're hitting an object with the correct layer
            if (selectedObject != null && selectedObject.layer == LayerMask.NameToLayer("ItemLayer"))
            {
                Item item = selectedObject.GetComponent<Item>();
                if (item != null)
                {
                    item.OnItemTapped(); // Handle item interaction
                }
            }
        }
    }
}