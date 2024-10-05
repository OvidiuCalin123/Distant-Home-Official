using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateStealPocketPoints : MonoBehaviour
{
    public RectTransform point;  // Reference to the point UI element (RectTransform)
    private RectTransform parentRectTransform;  // RectTransform of the UI element this script is attached to

    // Start is called before the first frame update
    void Start()
    {
        // Get the RectTransform of the UI element this script is attached to
        parentRectTransform = GetComponent<RectTransform>();

        // Move the point to a random position on the UI element's surface (only once)
        MovePointRandomly();
    }

    // Function to move the point randomly on the surface of the UI element
    void MovePointRandomly()
    {
        // Get the size of the parent UI element
        Vector2 sizeDelta = parentRectTransform.sizeDelta;

        // Generate random x and y positions within the bounds of the UI element
        float randomX = Random.Range(-sizeDelta.x / 2, sizeDelta.x / 2);
        float randomY = Random.Range(-sizeDelta.y / 2, sizeDelta.y / 2);

        // Update the point's anchored position relative to the parent UI element
        point.anchoredPosition = new Vector2(randomX, randomY);
    }
}
