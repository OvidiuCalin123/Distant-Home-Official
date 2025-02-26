using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePuzzleConductor : MonoBehaviour
{
    public float minX = 676.53f; // Left boundary
    public float maxX = 680.01f;  // Right boundary

    private float offsetZ; // Offset to maintain depth

    void Start()
    {
        offsetZ = Camera.main.WorldToScreenPoint(transform.position).z;
    }

    void OnMouseDrag()
    {
        // Get the current mouse position in world space
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = offsetZ; // Maintain the Z position

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Restrict movement to the X-axis and clamp within boundaries
        float clampedX = Mathf.Clamp(worldPosition.x, minX, maxX);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
}
