using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueNavigation : MonoBehaviour
{
    public int dialogueState = 1;

    public GameObject dialogueText1;
    public GameObject dialogueText2;
    public GameObject dialogueText3;
    public GameObject dialogueText4;
    public GameObject dialogueText5;

    private Camera mainCamera;
    public bool playerNearNPC;

    public GameObject DialogueBox;
    public GameObject optionalMarker;
    public GameObject optionalGameObject;

    void Start()
    {
        mainCamera = Camera.main;
        UpdateDialogueText();
    }

    void Update()
    {
        if (playerNearNPC)
        {
            DialogueBox.SetActive(true);

            if (optionalMarker != null)
            {
                optionalMarker.SetActive(false);
            }
            if (optionalGameObject != null)
            {
                optionalGameObject.SetActive(false);
            }

            UpdateDialogueText();
        }
        else
        {
            //DialogueBox.SetActive(false);

            if (optionalGameObject != null)
            {
                optionalGameObject.SetActive(true);
            }
        }
    }

    void UpdateDialogueText()
    {

        switch (dialogueState)
        {
            case 1:
                dialogueText1.gameObject.SetActive(true);
                dialogueText2.gameObject.SetActive(false);
                dialogueText3.gameObject.SetActive(false);
                dialogueText4.gameObject.SetActive(false);
                dialogueText5.gameObject.SetActive(false);
                break;
            case 2:
                dialogueText2.gameObject.SetActive(true);
                dialogueText1.gameObject.SetActive(false);
                dialogueText3.gameObject.SetActive(false);
                dialogueText4.gameObject.SetActive(false);
                dialogueText5.gameObject.SetActive(false);
                break;
            case 3:
                dialogueText3.gameObject.SetActive(true);
                dialogueText1.gameObject.SetActive(false);
                dialogueText2.gameObject.SetActive(false);
                dialogueText4.gameObject.SetActive(false);
                dialogueText5.gameObject.SetActive(false);
                break;
            case 4:
                dialogueText4.gameObject.SetActive(true);
                dialogueText1.gameObject.SetActive(false);
                dialogueText2.gameObject.SetActive(false);
                dialogueText3.gameObject.SetActive(false);
                dialogueText5.gameObject.SetActive(false);
                break;
            case 5:
                dialogueText5.gameObject.SetActive(true);
                dialogueText1.gameObject.SetActive(false);
                dialogueText2.gameObject.SetActive(false);
                dialogueText3.gameObject.SetActive(false);
                dialogueText4.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }
}
