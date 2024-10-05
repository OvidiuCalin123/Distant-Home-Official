using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For UI components

public class StealManager : MonoBehaviour
{
    public float rotationSpeed = 30f;  // Speed of the rotation
    public float maxRotation = 45f;    // Max rotation angle in degrees

    public GameObject rotateObject;    // The GameObject to be rotated (assigned in the inspector)

    public float leftLimit = -200f;    // Limit for left side restriction (x axis) for the GameObject
    public float rightLimit = 9999f;   // Arbitrary large number to signify no right limit

    public float offsetX = 50f;        // Offset on the X axis to add to the mouse position

    private RectTransform rectTransform;  // RectTransform of the UI hand
    private float currentRotationZ = 0f;  // Track the current Z rotation of the hand
    private bool canRotateUp = false;     // Track if rotating up is allowed
    private bool isRotatingDown = false;  // Flag to track if S is pressed for rotating down

    // Start is called before the first frame update
    void Start()
    {
        // Get the RectTransform for the UI hand
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move the hand object to follow the mouse, but stay within the restricted area
        FollowMouse();

        // Rotate the specified object based on W and S key input
        HandleRotation();
    }

    void FollowMouse()
    {
        // Convert mouse position to local position in the parent canvas space
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent as RectTransform, Input.mousePosition, null, out localPoint);

        // Add the x offset to the mouse position
        localPoint.x += offsetX;

        // Restrict GameObject's position on the x-axis based on leftLimit and rightLimit
        if (localPoint.x < leftLimit)
        {
            localPoint.x = leftLimit; // Don't allow the GameObject to move past the left limit
        }

        // Optionally, you can add a right limit, but it's set very large for now
        if (localPoint.x > rightLimit)
        {
            localPoint.x = rightLimit; // Don't allow the GameObject to move past the right limit
        }

        // Set the GameObject's position (hand) to follow the restricted mouse position in the UI space
        rectTransform.localPosition = localPoint;
    }

    void HandleRotation()
    {
        if (rotateObject == null)
        {
            Debug.LogWarning("No object assigned to rotate. Please assign a GameObject in the inspector.");
            return;
        }

        // Rotate counterclockwise (downwards) when S is pressed
        if (Input.GetKey(KeyCode.S))
        {
            currentRotationZ += rotationSpeed * Time.deltaTime;
            isRotatingDown = true;       // Flag that we are rotating down
            canRotateUp = true;          // Allow the hand to rotate up
        }
        // Rotate clockwise (upwards) when W is pressed, but only if it's rotated down first
        else if (Input.GetKey(KeyCode.W) && canRotateUp)
        {
            currentRotationZ -= rotationSpeed * Time.deltaTime;
            isRotatingDown = false;      // No longer rotating down
        }

        // Clamp the rotation to prevent exceeding the maxRotation (downwards only)
        currentRotationZ = Mathf.Clamp(currentRotationZ, 0f, maxRotation);

        // Apply the new rotation to the rotateObject
        rotateObject.transform.rotation = Quaternion.Euler(0, 0, currentRotationZ);
    }
}
