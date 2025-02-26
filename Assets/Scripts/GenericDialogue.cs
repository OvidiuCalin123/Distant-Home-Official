using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GenericDialogue : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public string NpcName;
    public string[] randomDialogues;
    public float typingSpeed = 0.03f;
    public int maxCharactersPerPage = 80;

    [Header("Quest Settings")]
    public bool hasQuest;
    public bool __questGiven;
    public string dialogueTextBeforeQuestGiven;
    public string dialogueTextAfterQuestGiven;
    public bool questComplete;
    public string dialogueTextQuestCompleteGetReward;
    public int getRewardAfterDialoguePage;
    public string dialogueTextQuestCompleteAfterReward;
    public bool gotItem;

    public Act2QuestManager act2QuestManager;

    [Header("World Space Elements")]
    public GameObject dialogueBubble;
    public TextMeshPro dialogueTextMesh;

    private List<string> dialoguePages = new List<string>();
    public int currentPage = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    public bool playerInRange = false;
    private string activeDialogue;

    void Start()
    {
        activeDialogue = "";
        gotItem = false;
        dialogueBubble.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && !dialogueBubble.activeSelf)
        {
            SetupDialogue();
        }
        else if (dialogueBubble.activeSelf && Input.GetKeyDown(KeyCode.E) && !isTyping)
        {
            ShowNextPage();
        }
    }

    // Determine the correct dialogue before splitting into pages
    private void SetupDialogue()
    {
        if (hasQuest)
        {
            questComplete = act2QuestManager.isQuestCompletedForNpc(NpcName);

            if (questComplete)
            {
                if(activeDialogue == dialogueTextAfterQuestGiven || activeDialogue == dialogueTextBeforeQuestGiven || activeDialogue == "" && questComplete)
                {
                    activeDialogue = dialogueTextQuestCompleteGetReward;
                }
            }
            else
            {
                if (__questGiven)
                {
                    activeDialogue = dialogueTextAfterQuestGiven;
                }
                else
                {
                    activeDialogue = dialogueTextBeforeQuestGiven;
                }
                
            }
        }
        else
        {
            activeDialogue = randomDialogues[0];
            Debug.Log(activeDialogue);
        }

        SplitTextIntoPages();
        StartDialogue();
    }

    // Splits dialogue into pages
    private void SplitTextIntoPages()
    {
        dialoguePages.Clear();
        for (int i = 0; i < activeDialogue.Length; i += maxCharactersPerPage)
        {
            dialoguePages.Add(activeDialogue.Substring(i, Mathf.Min(maxCharactersPerPage, activeDialogue.Length - i)));
        }
    }

    // Start the dialogue
    private void StartDialogue()
    {
        if (dialoguePages.Count == 0) return;

        
        dialogueBubble.SetActive(true);
        currentPage = 0;
        ShowPage();
    }

    // Show the current dialogue page
    private void ShowPage()
    {
        if (currentPage < dialoguePages.Count)
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeText(dialoguePages[currentPage]));
        }
        else
        {
            CloseDialogue();
        }
    }

    // Move to the next page
    private void ShowNextPage()
    {
        currentPage++;
        ShowPage();
    }

    // Typing effect
    IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueTextMesh.text = "";

        foreach (char letter in text)
        {
            dialogueTextMesh.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    // Close dialogue
    private void CloseDialogue()
    {
        dialogueBubble.SetActive(false);
        currentPage = 0;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if ((activeDialogue == dialogueTextBeforeQuestGiven) && hasQuest)
            {
                __questGiven = true;
            }

            if (activeDialogue == dialogueTextQuestCompleteGetReward && questComplete)
            {
                activeDialogue = dialogueTextQuestCompleteAfterReward;
            }

            playerInRange = false;
            dialogueBubble.SetActive(false);
            currentPage = 0;
        }
    }

}
