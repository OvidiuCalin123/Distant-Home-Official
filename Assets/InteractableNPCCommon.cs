using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractableNPCCommon : MonoBehaviour
{

    [Header("Dialogue")]

    public GameObject dialogue;

    public bool hasDialogue = true;

    public string text1;
    public string text1_0;

    public string text2;
    public string text2_0;

    public string text3;
    public string text3_0;

    public string text4;
    public string text4_0;

    public bool dontAllowDialogue;

    // Start is called before the first frame update
    void Start()
    {
        dontAllowDialogue = false;
        hasDialogue = true;
        dialogue = GameObject.Find("Canvas").transform.Find("npcDialogue").gameObject;
    }

    public GameObject showDialogue()
    {

        if (hasDialogue && !dontAllowDialogue)
        {
            dialogue.GetComponent<NpcDialogueCommon>().text1 = text1;
            dialogue.GetComponent<NpcDialogueCommon>().text1_0 = text1_0;

            dialogue.GetComponent<NpcDialogueCommon>().text2 = text2;
            dialogue.GetComponent<NpcDialogueCommon>().text2_0 = text2_0;

            dialogue.GetComponent<NpcDialogueCommon>().text3 = text3;
            dialogue.GetComponent<NpcDialogueCommon>().text3_0 = text3_0;

            dialogue.GetComponent<NpcDialogueCommon>().text4 = text4;
            dialogue.GetComponent<NpcDialogueCommon>().text4_0 = text4_0;

            dialogue.gameObject.SetActive(true);

            if(transform.Find("questMarker") != null)
            {
                dialogue.GetComponent<NpcDialogueCommon>().npcQuestMarker = transform.Find("questMarker").gameObject;
            }
            
            dialogue.GetComponent<NpcDialogueCommon>().NPC = gameObject;
        }

        return dialogue;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
