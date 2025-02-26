using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine;

public class Act2WorldInter : MonoBehaviour
{
    private float defaultSize = 0.7f; // Default size for x and y
    private float minSize = 0.665f;    // Minimum size for x and y
    private float maxSize = 0.75f;    // Maximum size for x and y
    private float initialY;          // Initial Y position of the player
    private float threshold = 1.0f;  // Movement threshold for resizing
    private float smoothSpeed = 0.02f; // Smoothing speed for resizing
    public PlayerMovement player;
    public GameObject LightFromCandleLightEventZone;
    public GameObject eventCandleLightUICanvas;
    public GameObject candleDecour;

    public GameObject playerCatEyes;
    public GameObject doorPoor;
    public GameObject poorDarkness;
    public GameObject poorFloor;
    public GameObject poorCandles;
    public GameObject floorCollistionCandleEventCantMoveWithoutLight;
    public GameObject showSpiderBoxPlacement;
    public GameObject paperClip;

    public GameObject putLightHereColliderStorageRoom;

    public GameObject CandlePlayerStorage;

    private Vector3 targetScale; // Target scale for smooth resizing

    public GameObject lockPickMiniGame;
    public Collider2D lockedDoorMiddle;
    public Collider2D pickLockingDoorMiddle;

    public GameObject kitchenFork;
    public GameObject ropeHookOutside;
    public int paperTrashGot = 0;

    public bool isOnOutsideRope = false;
    public GameObject miniGameOutsideRope;
    public bool miniGameOutsidePoint1;
    public Collider2D miniGameOutsidePoint1Obj;
    public bool miniGameOutsidePoint2;
    public Collider2D miniGameOutsidePoint2Obj;

    public Collider2D trainOutsideFloorCollision;
    public Collider2D trainOutsideDoorCollision;

    public Collider2D trainOutsideFloorMidCollision;
    public Collider2D trainOutsideDoorMidCollision;

    public Collider2D girlWithRing;
    public GameObject CardMiniGameRich;

    public GameObject richGuyFat;
    public GameObject richAreaFog;

    public Act2QuestManager act2QuestManager;

    public GameObject RichLights;
    public GameObject RichLightsAfter;
    public GameObject RichPlayerPosDarkness;
    public GameObject RichWomanAtDoorPosAfterDarkness;
    public GameObject RichWomanAtDoor;
    public GameObject RichWomanAtDoorDialogue;

    public GameObject VBrute;
    public GameObject VBrutePos;

    public GameObject VBruteGlowEyes;
    public GameObject VBruteLightsON;

    public bool VladimirQuestDone;
    public bool WomanDoorQuestDone;
    private bool isWaiting = false;

    public GameObject HanoiPuzzleMini;
    public GameObject coalShovel;

    public GameObject FireCoalMeterSlider;
    public GameObject WaterCoalMeterSlider;

    public bool WaitABitAfterPuttingCoals;

    public bool canUseFireMeter;

    public GameObject steamMeterTongue;

    private float fireHigh = 2.8f;
    private float fireLow = -1.5f;
    private float waterHigh = 2.85f;
    private float waterLow = 1.5f;

    private float steamRotationLow = 244f;
    private float steamRotationHigh = -71f;

    public bool stopCoalMiniGame;
    public GameObject playerPositionUpTrain;
    public bool isPlayerOnTrain;
    public bool isPlayerInTunnel;

    public bool isTiedConductor;

    public GameObject killer;
    public GameObject afterKillerSlappedBlackScreen;

    public GameObject stoneConductorPlace;
    public GameObject WoodConductorPlace;
    public Collider2D PuzzleConductorPlace;

    public GameObject coalConductorPlank;
    public Collider2D bottleShards;

    public bool coalGot;

    void Start()
    {

        coalGot = false;
        isTiedConductor = false;
        isPlayerOnTrain = false;
        stopCoalMiniGame = false;
        canUseFireMeter = false;
        miniGameOutsidePoint1 = false;
        miniGameOutsidePoint2 = false;
        isOnOutsideRope = false;
        paperTrashGot = 0;
        // Record the initial Y position
        initialY = transform.position.y;

        // Set the initial target scale
        targetScale = new Vector3(defaultSize, defaultSize, transform.localScale.z);
        player = GetComponent<PlayerMovement>();

    }

    void Update()
    {
        // Normalize values between 0 and 1
        float fireLevel = Mathf.InverseLerp(fireLow, fireHigh, FireCoalMeterSlider.transform.position.y);
        float waterLevel = Mathf.InverseLerp(waterLow, waterHigh, WaterCoalMeterSlider.transform.position.y);

        float steamEffect = Mathf.Clamp01(fireLevel * (1 - (waterLevel * 0.5f))); // Water only reduces 50%

        // Adjust rotation based on steam effect
        float steamRotation = Mathf.Lerp(steamRotationLow, steamRotationHigh, steamEffect);
        steamMeterTongue.transform.rotation = Quaternion.Euler(0, 0, steamRotation);

        if (steamRotation < -70f)
        {
            stopCoalMiniGame = true;
        }

        if (WaitABitAfterPuttingCoals && !stopCoalMiniGame)
        {
            Vector2 currentPosition = FireCoalMeterSlider.transform.position;

            // Move towards the target y position gradually
            if (currentPosition.y > -1.5f)
            {
                FireCoalMeterSlider.transform.position = new Vector2(
                    currentPosition.x,
                    Mathf.MoveTowards(currentPosition.y, 1.25f, 0.1f * Time.deltaTime)
                );
            } 
            else
            {
                WaitABitAfterPuttingCoals = false;
            }
        }

        if (Input.GetKey(KeyCode.Escape) && HanoiPuzzleMini.activeSelf)
        {
            HanoiPuzzleMini.SetActive(false);
        }

        if (player.anim2d.GetBool("idleTied"))
        {
            isTiedConductor = true;
            if (Input.GetKey(KeyCode.D))
            {
                transform.rotation = Quaternion.Euler(0, 00, 0);
                player.anim2d.speed = 1;
                player.transform.position += Vector3.right * Time.deltaTime * 1.25f;
                player.anim2d.SetBool("moveTied", true);
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                player.anim2d.speed = 1;
                player.ais[0].destination = player.transform.position;
                player.transform.position += Vector3.right * Time.deltaTime * 1.25f;
                player.anim2d.SetBool("moveTied", false);
            }

            if (Input.GetKey(KeyCode.A))
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                player.anim2d.speed = 1;
                player.transform.position += Vector3.left * Time.deltaTime * 1.25f;
                player.anim2d.SetBool("moveTied", true);
            }
            else if (Input.GetKeyUp(KeyCode.A))
            {
                player.anim2d.speed = 1;
                player.ais[0].destination = player.transform.position;
                player.transform.position += Vector3.left * Time.deltaTime * 1.25f;
                player.anim2d.SetBool("moveTied", false);
            }
        }
        else
        {
            isTiedConductor = false;
        }

        if (isPlayerOnTrain && !isPlayerInTunnel)
        {
            if (Input.GetKey(KeyCode.D))
            {
                player.anim2d.speed = 1;
                player.transform.position += Vector3.right * Time.deltaTime * 2.5f;
                player.anim2d.SetBool("TrainWalk", true);
                player.anim2d.SetBool("TrainCrouch", false);
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                player.anim2d.speed = 1;
                player.ais[0].destination = player.transform.position;
                player.transform.position += Vector3.right * Time.deltaTime * 2.5f;
                player.anim2d.SetBool("TrainWalk", false);
                player.anim2d.SetBool("TrainCrouch", false);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                player.ais[0].destination = player.transform.position;
                player.anim2d.SetBool("TrainCrouch", true);
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                player.anim2d.speed = 1;
                player.ais[0].destination = player.transform.position;
                player.anim2d.SetBool("TrainCrouch", true);
            }
        }
        else if(isPlayerInTunnel)
        {
            player.ais[0].destination = player.transform.position;
            player.anim2d.SetBool("TrainWalk", false);
            player.anim2d.SetBool("TrainCrouch", false);
        }

        if (isOnOutsideRope)
        {
            trainOutsideFloorCollision.enabled = false;
            trainOutsideDoorCollision.enabled = false;
            player.ais[0].destination = player.transform.position;
            if (miniGameOutsidePoint1 == false && miniGameOutsidePoint2 == false)
            {
                if (Input.GetKey(KeyCode.A))
                {
                    player.transform.position += Vector3.left * Time.deltaTime * 2;
                    player.anim2d.SetBool("canClimbTrainRopeMove", true);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    player.transform.position += Vector3.right * Time.deltaTime * 2;
                    player.anim2d.SetBool("canClimbTrainRopeMove", true);
                }
                else
                {
                    player.anim2d.SetBool("canClimbTrainRopeMove", false);
                }
            }

            if (miniGameOutsidePoint1 == true || miniGameOutsidePoint2 == true)
            {
                miniGameOutsideRope.SetActive(true);
            }

        }

        if (eventCandleLightUICanvas.activeSelf)
        {
            CandlePlayerStorage.SetActive(false);
        }

        // Get the player's current Y position
        float currentY = transform.position.y;

        // Check if the player is within the threshold
        if (Mathf.Abs(currentY - initialY) <= threshold)
        {
            // Reset to default size if within the threshold
            targetScale = new Vector3(defaultSize, defaultSize, transform.localScale.z);
        }
        else
        {
            // Calculate the size adjustment based on movement direction
            float offsetY = initialY - currentY; // Inverted direction
            float newSize = Mathf.Clamp(defaultSize + (offsetY * 0.05f), minSize, maxSize);

            // Set the target scale for smooth resizing
            targetScale = new Vector3(newSize, newSize, transform.localScale.z);
        }

        if (VladimirQuestDone && WomanDoorQuestDone && !isWaiting)
        {
            isWaiting = true; // Start the waiting process
            RichLights.SetActive(false);
            player.teleportPlayerToPos.screenPos = 0.8f;
            player.teleportPlayerToPos.animSpeed = 1;
            player.teleportPlayerToPos.playerTeleportPos = RichPlayerPosDarkness.transform.position;
            player.teleportPlayerToPos.gameObject.SetActive(true);

            StartCoroutine(WaitAndActivateLights());
        }

        if (RichWomanAtDoorDialogue.GetComponent<GenericDialogue>().currentPage == 3 &&
            RichWomanAtDoorDialogue.GetComponent<GenericDialogue>().playerInRange == true)
        {
            RichWomanAtDoorDialogue.GetComponent<GenericDialogue>().playerInRange = false;
            RichWomanAtDoorDialogue.GetComponent<Collider2D>().enabled = false;
            RichWomanAtDoor.GetComponent<Animator>().SetBool("canWalk", true);
            VBrute.GetComponent<Animator>().SetBool("canWalk", true);
            VBrute.transform.position = new Vector2(VBrutePos.transform.position.x, VBrutePos.transform.position.y - 0.8f);
            VBruteGlowEyes.SetActive(false);
        }

        // Smoothly interpolate towards the target scale
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, smoothSpeed);
    }

    public void freezeAnimatorCrouch()
    {
        player.anim2d.speed = 0;
    }

    public void TrainCrouchFalse()
    {
        player.anim2d.SetBool("TrainCrouch", false);
    }

    private IEnumerator WaitAndActivateLights()
    {
        // Wait for 3 seconds
        yield return new WaitForSeconds(4f);
        RichWomanAtDoor.transform.position = RichWomanAtDoorPosAfterDarkness.transform.position;
        VBrute.transform.position = VBrutePos.transform.position;
        VBruteGlowEyes.SetActive(true);
        // Activate the lights after the wait
        RichLightsAfter.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        player.ais[0].destination = transform.position;
        player.anim2d.SetBool("canWalk", false);
        player.cinematic.SetActive(true);
        player.vcam.gameObject.transform.parent = null;

        player.MoveCamera(0.25f, 0.11f, false);
        RichWomanAtDoorDialogue.GetComponent<GenericDialogue>().playerInRange = true;
        RichWomanAtDoorDialogue.SetActive(true);
        //StartCoroutine(player.CallMoveCameraAfterDelay(12f));
    }

    public void handleAct2InteractiveWorldEvent()
    {
        if (player.clickedObject != null)
        {
            if (player.clickedObject.tag == "TrainDoor")
            {
                if (CandlePlayerStorage.activeSelf)
                {
                    CandlePlayerStorage.SetActive(false);
                    floorCollistionCandleEventCantMoveWithoutLight.GetComponent<Collider2D>().enabled = true;
                    floorCollistionCandleEventCantMoveWithoutLight.SetActive(true);
                    putLightHereColliderStorageRoom.SetActive(true);
                }

                player.clickedObject.GetComponent<Animator>().SetBool("canOpen", true);
                var doorLeadingTo = player.clickedObject.transform.Find("TeleportLocation");
                player.teleportPlayerToPos.animSpeed = 2;
                player.teleportPlayerToPos.screenPos = 0.8f;
                player.teleportPlayerToPos.playerTeleportPos = new Vector3(doorLeadingTo.gameObject.transform.position.x, doorLeadingTo.gameObject.transform.position.y, gameObject.transform.position.z);
                player.teleportPlayerToPos.gameObject.SetActive(true);
                player.soundManager.playTrainDoor();
                player.teleportPlayerToPos.isUsingDoors = true;
            }
            else if (player.clickedObject.tag == "CandleLightEvent")
            {
                Cursor.visible = false;
                LightFromCandleLightEventZone.SetActive(false);
                eventCandleLightUICanvas.SetActive(true);
            }
            else if (player.clickedObject.tag == "candlePickup")
            {
                player.soundManager.playGenericItemRub();
                player.playerInventory.addNewInventoryItem(player.playerInventory.getAvailableItemByTag("candlePickup"));
                Destroy(player.clickedObject);
            }
            else if (player.clickedObject.tag == "KitchenSoup")
            {
                player.soundManager.playGenericItemRub();
                player.playerInventory.addNewInventoryItem(player.playerInventory.getAvailableItemByTag("KitchenSoup"));
                Destroy(player.clickedObject);
            }
            else if (player.clickedObject.tag == "spiderBoxItem")
            {
                player.soundManager.playGenericItemRub();
                player.playerInventory.addNewInventoryItem(player.playerInventory.getAvailableItemByTag("spiderBoxItem"));
                Destroy(player.clickedObject);
            }
            else if (player.clickedObject.tag == "spiderBox")
            {
                player.anim2d.SetBool("canJump", true);
            }
            else if (player.clickedObject.tag == "poorSpider" && player.isOnSpiderBox)
            {
                player.soundManager.playGenericItemRub();
                player.playerInventory.addNewInventoryItem(player.playerInventory.getAvailableItemByTag("poorSpider"));
                Destroy(player.clickedObject);
            }
            else if (player.clickedObject.tag == "buttonFat")
            {
                player.soundManager.playGenericItemRub();
                player.playerInventory.addNewInventoryItem(player.playerInventory.getAvailableItemByTag("buttonFat"));
                Destroy(player.clickedObject);
            }
            else if (player.clickedObject.tag == "proofScarf")
            {
                player.soundManager.playGenericItemRub();
                player.playerInventory.addNewInventoryItem(player.playerInventory.getAvailableItemByTag("proofScarf"));
                Destroy(player.clickedObject);
            }
            else if (player.clickedObject.tag == "rope")
            {
                player.soundManager.playGenericItemRub();
                player.playerInventory.addNewInventoryItem(player.playerInventory.getAvailableItemByTag("rope"));
                Destroy(player.clickedObject);
            }
            else if (player.clickedObject.tag == "pickLockThis")
            {
                player.soundManager.playLockedDoor();
            }
            else if (player.clickedObject.tag == "kitchenFork")
            {
                if (player.clickedObject.GetComponent<Animator>() == null)
                {
                    if (player.clickedObject.transform.parent.GetComponent<Animator>().GetBool("canForkFall"))
                    {
                        player.soundManager.playGenericItemRub();
                        player.playerInventory.addNewInventoryItem(player.playerInventory.getAvailableItemByTag("kitchenFork"));
                        Destroy(player.clickedObject.transform.parent.gameObject);
                    }

                }


            }
            else if (player.clickedObject.tag == "paperTrash")
            {
                player.soundManager.playGenericItemRub();
                Destroy(player.clickedObject);

                if (paperTrashGot == 3)
                {
                    player.playerInventory.removeItemFromInventory("paperTrash");
                    player.soundManager.playRemoveItem();
                    player.playerInventory.addNewInventoryItem(player.playerInventory.getAvailableItemByTag("paperBall"));
                    return;
                }
                foreach (GameObject inventoryItem in player.playerInventory.inventoryItems)
                {
                    if (inventoryItem.tag == "paperTrash")
                    {
                        TextMeshProUGUI amountText = inventoryItem.transform.Find("amount").GetComponent<TextMeshProUGUI>();

                        string currentText = amountText.text;
                        int currentAmount = int.Parse(currentText.Substring(1));

                        if (currentAmount < 4)
                        {

                            player.soundManager.playGenericItemRub();
                            player.playerInventory.updateInventoryItemCountUp(inventoryItem);

                        }
                        paperTrashGot += 1;
                        return;
                    }
                }
                foreach (GameObject item in player.playerInventory.availabelItemsItems)
                {
                    if (item.tag == "paperTrash")
                    {

                        player.soundManager.playGenericItemRub();
                        player.playerInventory.addNewInventoryItem(item);
                        paperTrashGot += 1;
                        return;
                    }
                }

            }
            else if (player.clickedObject.tag == "outsideRopeClimb")
            {
                isOnOutsideRope = true;
                player.anim2d.SetBool("canClimbTrainRope", true);
            }
            else if (player.clickedObject.name == "RingHolder")
            {
                CardMiniGameRich.SetActive(true);
            }
            else if (player.clickedObject.name == "Tiger")
            {
                player.clickedObject.GetComponent<Animator>().SetBool("canpaw", true);
            }
            else if (player.clickedObject.name == "richFatGuyShoes")
            {
                player.clickedObject.transform.parent.gameObject.GetComponent<Animator>().SetBool("canFall", true);
                Destroy(player.clickedObject);
            }
            else if (player.clickedObject.name == "RichTableKnife")
            {
                if (richGuyFat.GetComponent<Animator>().GetBool("canFall"))
                {
                    player.soundManager.playGenericItemRub();
                    player.playerInventory.addNewInventoryItem(player.playerInventory.getAvailableItemByTag("knifeRichTable"));
                    Destroy(player.clickedObject);
                }
            }
            else if (player.clickedObject.name == "Vladimir")
            {
                GenericDialogue dialog = player.clickedObject.transform.Find("dialog").gameObject.GetComponent<GenericDialogue>();

                if (act2QuestManager.isQuestCompletedForNpc("Vladimir") && !dialog.gotItem)
                {
                    player.soundManager.playGenericItemRub();
                    player.playerInventory.addNewInventoryItem(player.playerInventory.getAvailableItemByTag("poorKey"));
                    dialog.gotItem = true;
                    VladimirQuestDone = true;
                }

                dialog.playerInRange = true;
            }
            else if (player.clickedObject.name == "HanoiPuzzle")
            {
                HanoiPuzzleMini.SetActive(true);
            }
            else if (player.clickedObject.name == "trainValveUp")
            {
                if (!stopCoalMiniGame)
                {
                    if (WaterCoalMeterSlider.transform.position.y < 2.85f)
                    {
                        player.clickedObject.GetComponent<Animator>().SetBool("canRotate", true);
                        WaterCoalMeterSlider.transform.position = new Vector2(WaterCoalMeterSlider.transform.position.x, WaterCoalMeterSlider.transform.position.y + 0.3f);
                    }
                }
                

            }
            else if (player.clickedObject.name == "trainValveDown")
            {
                if (!stopCoalMiniGame)
                {
                    if (WaterCoalMeterSlider.transform.position.y > 1.5f)
                    {
                        player.clickedObject.GetComponent<Animator>().SetBool("canRotate", true);
                        WaterCoalMeterSlider.transform.position = new Vector2(WaterCoalMeterSlider.transform.position.x, WaterCoalMeterSlider.transform.position.y - 0.3f);
                    }
                }
            }
            else if (player.clickedObject.name == "sock")
            {
                player.soundManager.playGenericItemRub();
                player.playerInventory.addNewInventoryItem(player.playerInventory.getAvailableItemByTag("sockItem"));
                Destroy(player.clickedObject);
            }
            else if (player.clickedObject.name == "Furnance")
            {
                if (!stopCoalMiniGame)
                {
                    if (FireCoalMeterSlider.transform.position.y < 2.8f)
                    {
                        player.anim2d.SetBool("canCoal", true);
                        coalShovel.SetActive(false);

                        if (canUseFireMeter)
                        {
                            FireCoalMeterSlider.transform.position = new Vector2(FireCoalMeterSlider.transform.position.x, FireCoalMeterSlider.transform.position.y + 0.45f);
                            WaitABitAfterPuttingCoals = false;
                            StartCoroutine(waitAbitAfterPuttingCoals());
                        }

                    }
                }
            }
            else if (player.clickedObject.name == "CoalLadder")
            {
                player.transform.rotation = Quaternion.Euler(0, 0, 0);
                player.teleportPlayerToPos.screenPos = 0.64f;
                player.teleportPlayerToPos.screenPosX = 0.34f;
                player.teleportPlayerToPos.playerTeleportPos = playerPositionUpTrain.transform.position;
                player.teleportPlayerToPos.gameObject.SetActive(true);
                
                isPlayerOnTrain = true;
            }
            else if (player.clickedObject.name == "CoalsConductor" && !isTiedConductor)
            {
                if (!coalGot)
                {
                    coalGot = true;
                    player.playerInventory.addNewInventoryItem(player.playerInventory.getAvailableItemByTag("coalItem"));
                }

                foreach (GameObject inventoryItem in player.playerInventory.inventoryItems)
                {
                    if (inventoryItem.tag == "coalItem")
                    {
                        TextMeshProUGUI amountText = inventoryItem.transform.Find("amount").GetComponent<TextMeshProUGUI>();

                        string currentText = amountText.text;
                        int currentAmount = int.Parse(currentText.Substring(1));

                        Debug.Log(currentAmount);

                        if (currentAmount == 0)
                        {
                            player.playerInventory.addNewInventoryItem(player.playerInventory.getAvailableItemByTag("coalItem"));
                        }

                        if (currentAmount < 5)
                        {

                            player.soundManager.playGenericItemRub();
                            player.playerInventory.updateInventoryItemCountUp(inventoryItem);

                        }
                        break;
                    }
                }
            }
            else if (player.clickedObject.name == "pickShardsCollider" && !isTiedConductor)
            {
                killer.SetActive(false);
                player.soundManager.playGenericItemRub();
                player.playerInventory.addNewInventoryItem(player.playerInventory.getAvailableItemByTag("ShatteredBottle"));
                player.clickedObject.GetComponent<Collider2D>().enabled = false;
            }
        }
    }

    public IEnumerator waitAbitAfterPuttingCoals()
    {
        yield return new WaitForSeconds(2);

        WaitABitAfterPuttingCoals = true;
    }
}
