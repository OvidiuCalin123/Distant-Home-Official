using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNavigation : MonoBehaviour
{

    public int dialogueState = 1;

    public GameObject dialogue1;
    public GameObject dialogue2;
    public GameObject dialogue3;

    private Camera mainCamera;
    public bool playerNearNPC;

    public GameObject DialogueBox;
    public GameObject optionalMarker;
    public GameObject optionalGameObject;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if(dialogueState > 3)
        {
            dialogueState = 1;
        }

        if(dialogueState == 1)
        {
            dialogue1.SetActive(true);
            dialogue2.SetActive(false);
            dialogue3.SetActive(false);
        }
        else if(dialogueState == 2)
        {
            dialogue1.SetActive(false);
            dialogue2.SetActive(true);
            dialogue3.SetActive(false);
        }
        else if (dialogueState == 3)
        {
            dialogue1.SetActive(false);
            dialogue2.SetActive(false);
            dialogue3.SetActive(true);
        }

        if (playerNearNPC)
        {
            DialogueBox.SetActive(true);

            if (optionalMarker)
            {
                optionalMarker.SetActive(false);
            }
            if (optionalGameObject)
            {
                optionalGameObject.SetActive(false);
            }
        }
        else
        {

            DialogueBox.SetActive(false);

            optionalGameObject.SetActive(true);
        }


    }
}
