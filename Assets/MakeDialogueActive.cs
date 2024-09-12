using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeDialogueActive : MonoBehaviour
{
    private Camera mainCamera;
    public bool playerNearNPC;

    public GameObject DialogueBox;

    // Start is called before the first frame update
    void Start()
    {
        // Cache the main camera
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerNearNPC)
        {
            DialogueBox.SetActive(true);
        }
        else
        {
            DialogueBox.SetActive(false);
        }
    }

    
}
