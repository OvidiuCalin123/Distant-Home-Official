using TMPro;
using UnityEngine;

public class QuestSystem : MonoBehaviour
{
    [Header("Quest 1")]

    public bool endConditionQuest1_1;
    public bool endConditionQuest1_2;
    public bool endConditionQuest1_3;
    public bool endConditionQuest1_4;

    public bool startConditionQuest1_1;
    public bool startConditionQuest1_2;
    public bool startConditionQuest1_3;
    public bool startConditionQuest1_4;

    public TextMeshProUGUI quest1_Text1;
    public TextMeshProUGUI quest1_Text2;
    public TextMeshProUGUI quest1_Text3;
    public TextMeshProUGUI quest1_Text4;


    [Header("Quest 2")]

    public bool endConditionQuest2_1;
    public bool endConditionQuest2_2;
    public bool endConditionQuest2_3;
    public bool endConditionQuest2_4;

    public bool startConditionQuest2_1;
    public bool startConditionQuest2_2;
    public bool startConditionQuest2_3;
    public bool startConditionQuest2_4;

    public TextMeshProUGUI quest2_Text1;
    public TextMeshProUGUI quest2_Text2;
    public TextMeshProUGUI quest2_Text3;
    public TextMeshProUGUI quest2_Text4;


    [Header("Quest 3")]

    public bool endConditionQuest3_1;
    public bool endConditionQuest3_2;
    public bool endConditionQuest3_3;
    public bool endConditionQuest3_4;

    public bool startConditionQuest3_1;
    public bool startConditionQuest3_2;
    public bool startConditionQuest3_3;
    public bool startConditionQuest3_4;

    public TextMeshProUGUI quest3_Text1;
    public TextMeshProUGUI quest3_Text2;
    public TextMeshProUGUI quest3_Text3;
    public TextMeshProUGUI quest3_Text4;

    private void endTask(TextMeshProUGUI task, bool isTaskFinished)
    {
        if (isTaskFinished)
        {
            task.text = string.Format("<s>{0}", task.text);
        }
        
    }

    private void startTask(TextMeshProUGUI task, bool isTaskReady)
    {
        if (isTaskReady)
        {
            task.gameObject.SetActive(true);
        }

    }

    private void endTaskSystem()
    { 
        endTask(quest1_Text1, endConditionQuest1_1);
        endTask(quest1_Text2, endConditionQuest1_2);
        endTask(quest1_Text3, endConditionQuest1_3);
        endTask(quest1_Text4, endConditionQuest1_4);

        endTask(quest2_Text1, endConditionQuest2_1);
        endTask(quest2_Text2, endConditionQuest2_2);
        endTask(quest2_Text3, endConditionQuest2_3);
        endTask(quest2_Text4, endConditionQuest2_4);

        endTask(quest3_Text1, endConditionQuest3_1);
        endTask(quest3_Text2, endConditionQuest3_2);
        endTask(quest3_Text3, endConditionQuest3_3);
        endTask(quest3_Text4, endConditionQuest3_4);

    }

    private void startTaskSystem()
    {
        startTask(quest1_Text1, startConditionQuest1_1);
        startTask(quest1_Text2, startConditionQuest1_2);
        startTask(quest1_Text3, startConditionQuest1_3);
        startTask(quest1_Text4, startConditionQuest1_4);

        startTask(quest2_Text1, startConditionQuest2_1);
        startTask(quest2_Text2, startConditionQuest2_2);
        startTask(quest2_Text3, startConditionQuest2_3);
        startTask(quest2_Text4, startConditionQuest2_4);

        startTask(quest3_Text1, startConditionQuest3_1);
        startTask(quest3_Text2, startConditionQuest3_2);
        startTask(quest3_Text3, startConditionQuest3_3);
        startTask(quest3_Text4, startConditionQuest3_4);

    }

    void Start()
    {
        
    }

    void Update()
    {
        startTaskSystem();
        endTaskSystem();

    }
}
