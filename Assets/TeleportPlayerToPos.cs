using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayerToPos : MonoBehaviour
{
    public Animator anim;
    public PlayerMovement player;
    public CinemachineVirtualCamera vcam;
    public float screenPos;
    public float screenPosX;
    public Vector3 playerTeleportPos;
    public float animSpeed;

    public GameObject npc;
    public bool isUsingDoors;
    // Start is called before the first frame update
    void Start()
    {
        
        animSpeed = 1;
        anim = GetComponent<Animator>();
    }

    IEnumerator waitABitForFIXJitteringWhenChangingCamera()
    {
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("canTeleport", true);
    }

    public void startTeleporting()
    {

        if (player.clickedObject.name == "CoalLadder")
        {
            player.anim2d.SetBool("TrainIdle", true);
        }

        if (player.act2InteractionHandler.killer.activeSelf)
        {
            player.act2InteractionHandler.afterKillerSlappedBlackScreen.SetActive(true);
            player.anim2d.SetBool("TrainIdle", false);
            player.anim2d.SetBool("idleTied", true);
            player.teleportPlayerToPos.screenPos = 0.73f;
            player.teleportPlayerToPos.screenPosX = 0.4f;
        }
        if (isUsingDoors)
        {
            player.vcam.Follow = player.clickedObject.transform.Find("CameraPosition");
            isUsingDoors = false;
        }

        anim.speed = animSpeed;
        if (player.jumpOnBoxesPopUp != null)
        {
            if (player.playerClimbingBoxes == false)
            {
                player.jumpOnBoxesPopUp.SetActive(false);
                player.playerSitingOnBoxes = false;
            }
        }
        if(npc != null)
        {
            npc.transform.position = npc.transform.Find("teleportPos").position;
            npc = null;
        }
        player.ais[0].destination = playerTeleportPos;
        player.transform.position = playerTeleportPos;
        
        vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenY = screenPos;
        if(screenPosX != 0)
        {
            vcam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = screenPosX;
            screenPosX = 0;
        }
        StartCoroutine(waitABitForFIXJitteringWhenChangingCamera());
    }

    public void endTeleporting()
    {

        if (player.playerClimbingBoxes)
        {
            player.jumpOnBoxesPopUp.SetActive(true);
            player.playerSitingOnBoxes = true;
            player.playerClimbingBoxes = false;
        }
        anim.SetBool("canTeleport", false);
        gameObject.SetActive(false);
        if (player.clickedObject.GetComponent<Animator>())
        {
            player.clickedObject.GetComponent<Animator>().SetBool("canOpen", false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
