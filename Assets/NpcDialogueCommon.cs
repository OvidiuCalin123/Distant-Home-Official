using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NpcDialogueCommon : MonoBehaviour
{
    public TextMeshProUGUI txt1;
    public TextMeshProUGUI txt1_0;

    public string text1;
    public string text1_0;

    public string text2;
    public string text2_0;

    public string text3;
    public string text3_0;

    public string text4;
    public string text4_0;

    public int holdDialoguePage = 1;

    public TypewriterEffect typeEffect;

    public GameObject npcQuestMarker;

    public PlayerMovement player;
    public bool addItemOnce;

    public List<string> alreadyGotItemsFromNpcsWithTags;

    public QuestSystem questSystem;

    public GameObject QuestItemInterface;

    public bool showOnlyOnce;

    public GameObject NPC;
    public Act1WorldInter act1;

    

    private void Start()
    {
        showOnlyOnce = true;
        holdDialoguePage = 1;
    }

    private void OnEnable()
    {
        holdDialoguePage = 1;
        txt1.text = text1;
        txt1_0.text = text1_0;

        typeEffect.storedText1.text = text1;
        typeEffect.storedText2.text = text1_0;

        NextDialogue();
    }

    public void CloseDialogue()
    {
        if (questSystem.endConditionQuest1_3 && showOnlyOnce && NPC.tag == "newspaper_guy")
        {
            showOnlyOnce = false;
            QuestItemInterface.SetActive(true);

            player.playerInventory.addNewInventoryItem(player.playerInventory.getAvailableItemByTag("halfTicket"));
            act1.ItemInterfaceTicket1.SetActive(true);
            act1.ItemInterfaceTicket2.SetActive(false);
            player.soundManager.playGenericItemRub();
            NPC.GetComponent<InteractableNPCCommon>().dontAllowDialogue = true;

        }
        if (NPC.tag == "GuyReadingNews")
        {
            NPC.GetComponent<ReadingNewsPaper>().enableAnimation();

        }


        holdDialoguePage = 1;
        gameObject.SetActive(false);
    }

    public void addItemOnlyOnce()
    {
        if (addItemOnce)
        {
            if (NPC.tag == "newspaper_guy")
            {
                if (!alreadyGotItemsFromNpcsWithTags.Contains("newspaper_guy"))
                {
                    foreach (GameObject item in player.playerInventory.availabelItemsItems)
                    {
                        if (item.tag == "Newspaper")
                        {
                            player.soundManager.playGenericItemRub();
                            player.playerInventory.addNewInventoryItem(item);
                            alreadyGotItemsFromNpcsWithTags.Add("newspaper_guy");
                            break;
                        }
                    }
                }
            }
            addItemOnce = false;
        }
    }

    public void addQuestOnlyOnce()
    {
        if (NPC.tag == "newspaper_guy")
        {
            questSystem.startConditionQuest1_3 = true;
        }

        if (NPC.tag == "TicketGuy")
        {
            questSystem.startConditionQuest1_2 = true;
        }
    }

    public void NextDialogue()
    {
        StopAllCoroutines();

        typeEffect.nextDialogue = true;

        if (holdDialoguePage == 1)
        {

            StartCoroutine(typeEffect.PlayText_(text1, text1_0));
        }
        if (holdDialoguePage == 2)
        {
            StartCoroutine(typeEffect.PlayText_(text2, text2_0));
        }
        else if(holdDialoguePage == 3)
        {
            StartCoroutine(typeEffect.PlayText_(text3, text3_0));
        }
        else if(holdDialoguePage >= 4)
        {
            npcQuestMarker.SetActive(false);

            addItemOnlyOnce();
            addQuestOnlyOnce();

            holdDialoguePage = 0;

            StartCoroutine(typeEffect.PlayText_(text4, text4_0));
        }

        holdDialoguePage += 1;
    }

}
