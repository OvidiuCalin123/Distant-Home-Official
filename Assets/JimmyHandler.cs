using UnityEngine;

public class JimmyHandler : MonoBehaviour
{
    public Animator anim;
    public Transform teleportPos;

    public PlayerMovement player;
    public GameObject sewageEntrance;

    public bool teleportOnce;
    public bool kickOnce;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        teleportOnce = true;
        kickOnce = true;
    }

    public void stopKick()
    {
        anim.SetBool("canKick", false);
        sewageEntrance.GetComponent<Collider2D>().enabled = true;
        gameObject.GetComponent<JimmyHandler>().enabled = false;
    }

    public void playLidKicked()
    {

        sewageEntrance.transform.Find("sweageLid").GetComponent<Animator>().SetBool("canBeKicked", true);

    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.name == "Jimmy")
        {

            if (gameObject.GetComponent<InteractableNPCCommon>().dialogue.GetComponent<NpcDialogueCommon>().npcQuestMarker != null)
            {

                Debug.Log("BRO WAHT");
                if (gameObject.GetComponent<InteractableNPCCommon>().dialogue.GetComponent<NpcDialogueCommon>().npcQuestMarker.activeSelf == false && teleportOnce)
                {
                    player.teleportPlayerToPos.npc = gameObject;
                    player.teleportPlayerToPos.screenPos = 0.64f;
                    player.teleportPlayerToPos.playerTeleportPos = new Vector3(85.36f, -4.13f, gameObject.transform.position.z);
                    player.teleportPlayerToPos.gameObject.SetActive(true);
                    teleportOnce = false;
                    gameObject.GetComponent<InteractableNPCCommon>().dialogue.GetComponent<NpcDialogueCommon>().CloseDialogue();
                }
            }

            if (kickOnce && player.teleportPlayerToPos.gameObject.activeSelf == false && !teleportOnce)
            {
                kickOnce = false;
                anim.SetBool("canKick", true);
            }
        }
    }
}
