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

    [Header("Quest1")]

    public GameObject quest1_0;
    public GameObject quest1_1;
    public GameObject quest1_2;
    public GameObject quest1_3;

    public GameObject quest1_0_Finished;
    public GameObject quest1_1_Finished;
    public GameObject quest1_2_Finished;
    public GameObject quest1_3_Finished;

    [Header("Quest2")]

    public GameObject quest2_1;
    public GameObject quest2_2;
    public GameObject quest2_3;
    public GameObject quest2_4;

    public GameObject quest2_1_Finished;
    public GameObject quest2_2_Finished;
    public GameObject quest2_3_Finished;
    public GameObject quest2_4_Finished;

    [Header("Quest3")]

    public GameObject quest3_1;
    public GameObject quest3_2;
    public GameObject quest3_3;
    public GameObject quest3_4;

    public GameObject quest3_1_Finished;
    public GameObject quest3_2_Finished;
    public GameObject quest3_3_Finished;
    public GameObject quest3_4_Finished;

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

            if (transform.parent.transform.parent.tag == "TicketGuy" && quest1_1.activeSelf == false)
            {
                quest1_1.SetActive(true);
            }

            if (transform.parent.transform.parent.tag == "newspaper_guy" && quest1_2.activeSelf == false)
            {
                quest1_2.SetActive(true);
            }
        }

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
        }
        else
        {

            DialogueBox.SetActive(false);

            if (optionalGameObject != null)
            {
                optionalGameObject.SetActive(true);
            }
            
        }

    }
}
