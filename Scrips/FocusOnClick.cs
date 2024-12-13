using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusOnClick : MonoBehaviour
{
    public Camera mainCamera;
    public float focusDistance = 5f;
    public float smoothSpeed = 0.125f;

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = mainCamera.transform.position;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Check if the clicked object is not a door
                if (!hit.transform.CompareTag("Door"))
                {
                    Vector3 direction = (mainCamera.transform.position - hit.transform.position).normalized;
                    Vector3 targetPosition = hit.transform.position + direction * focusDistance;
                    StartCoroutine(SmoothMove(targetPosition));
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.A)) // Press 'A' to reset the camera position
        {
            StartCoroutine(SmoothMove(originalPosition));
        }
    }

    System.Collections.IEnumerator SmoothMove(Vector3 targetPosition)
    {
        while (Vector3.Distance(mainCamera.transform.position, targetPosition) > 0.01f)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, smoothSpeed);
            yield return null;
        }
    }
}
