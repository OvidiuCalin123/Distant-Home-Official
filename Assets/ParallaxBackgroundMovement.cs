using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackgroundMovement : MonoBehaviour
{
    public Material backgroundMaterial;
    public float scrollSpeed = 0.1f;

    void Update()
    {
        if (backgroundMaterial != null)
        {
            // Update the scroll speed dynamically
            backgroundMaterial.SetFloat("_ScrollSpeed", scrollSpeed);
        }
    }
}
