using UnityEngine;

public class Act2QuestManager : MonoBehaviour
{


    [Header("Vladimir Quests")]

    public GameObject RichWindow;
    public GameObject WineSplash;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isQuestCompletedForNpc(string NpcName)
    {

        if(NpcName == "Vladimir")
        {
            if (RichWindow.GetComponent<Animator>().GetBool("canOpen") && WineSplash.activeSelf)
            {
                return true;
            }
        }

        return false;
    }
}
