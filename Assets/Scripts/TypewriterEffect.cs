using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    private TextMeshPro tmpText;  // Automatically gets the TextMeshPro component
    public float typingSpeed = 0.05f;  // Time between each character

    private string fullText;  // The complete text
    private bool hasStartedTyping = false;

    void Awake()
    {
        // Automatically assign the TextMeshPro component from the GameObject this script is attached to
        tmpText = GetComponent<TextMeshPro>();
    }

    void OnEnable()
    {
        // Start the typewriter effect when the object becomes active
        StartCoroutine(StartTypingEffect());
    }

    IEnumerator StartTypingEffect()
    {

        fullText = tmpText.text;  // Store the complete text from TextMeshPro component
        tmpText.text = "";  // Clear the text initially

        for (int i = 0; i < fullText.Length; i++)
        {
            tmpText.text += fullText[i];  // Append one letter at a time
            yield return new WaitForSeconds(typingSpeed);  // Wait before adding the next letter
        }
    }
}
