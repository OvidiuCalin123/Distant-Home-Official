using Pathfinding;
using TMPro;
using UnityEngine;

public class Act1WorldInter : MonoBehaviour
{
    public PlayerMovement player;

    public int brickSewage;
    public GameObject treasure;
    public treasureSewage treasureDropped;

    public Collider2D hideBoxColider;
    public GameObject handleShowHideUIText;

    public bool dontMoveUntilSewageHobboInPosition;
    public GameObject fatHobbo;

    public Collider2D SewageExit;
    public GameObject hobbo;
    public GameObject jimmyHand;
    public GameObject jimmy;
    public GameObject sewageOutsideLight;
    public GameObject bucket;
    public GameObject QuestItemInterface;

    public GameObject ItemInterfaceTicket1;
    public GameObject ItemInterfaceTicket2;

    public GameObject treasureKey;
    public GameObject coalTrail;

    public GameObject janitor;
    public GameObject ladderCat;

    public GameObject blackScreen;
    public GameObject blackScreenTimePassed;
    public bool didPlayerFallOnCatLadder;

    public GameObject nightLights;
    public GameObject dayStation;

    public GameObject exitScene1Door;
    public GameObject startExitScene1;

    void Start()
    {
        didPlayerFallOnCatLadder = false;
        brickSewage = 0;
        dontMoveUntilSewageHobboInPosition = false;
        player = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if(brickSewage == 3)
        {
            treasure.GetComponent<Animator>().SetBool("canOpen", true);
            brickSewage = 0;
        }

        if (treasureDropped.didTreasureDropped)
        {
            treasureDropped.didTreasureDropped = false;
            hideBoxColider.enabled = true;
            handleShowHideUIText.SetActive(true);
            SewageExit.enabled = false;
        }

    }

    public void HandleDogRangeInteraction()
    {
        player.anim2d.SetBool("canBeScared", true);

        if (transform.position.x < player.clickedObject.transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    public void HandleFishing()
    {
        player.canBeginFishingAfterTicket = false;
        player.beggarWithHalfTicket.enabled = false;
        player.fishAfterTicket = false;

        player.teleportPlayerToPos.screenPos = 0.8f;
        player.teleportPlayerToPos.playerTeleportPos = new Vector3(69.84f, -6.76f, transform.position.z);
        player.teleportPlayerToPos.gameObject.SetActive(true);

        GetComponent<SpriteRenderer>().sortingOrder = -1;
    }

    public void HandleHobboInteraction(IAstarAI ai)
    {
        if (player.anim2d.GetBool("canHideSewage"))
        {
            player.anim2d.SetBool("canHideReverse", true);
        }
        else if (!player.anim2d.GetBool("canHideSewage") && !player.anim2d.GetBool("canHideReverse"))
        {
            player.MoveToPlayerPosition(ai);
        }
    }

    public void handleAct1InteractiveWorldEvent()
    {
        if (player.clickedObject != null)
        {

            if (player.clickedObject.tag == "thrash_bin")
            {
                player.clickedObject.GetComponent<Animator>().SetBool("canOpenTrash", true);
                player.clickedObject.GetComponent<Collider2D>().enabled = false;
            }
            else if (player.clickedObject.tag == "Coin")
            {
                Destroy(player.clickedObject);
                player.soundManager.playCoinItemPicked();

                foreach (GameObject inventoryItem in player.playerInventory.inventoryItems)
                {
                    if (inventoryItem.tag == "CoinItem")
                    {
                        player.playerCoins += 1;
                        player.playerInventory.updateInventoryItemCountUp(inventoryItem);
                        return;
                    }
                }
                foreach (GameObject item in player.playerInventory.availabelItemsItems)
                {
                    if (item.tag == "CoinItem")
                    {
                        player.playerCoins = 1;
                        player.playerInventory.addNewInventoryItem(item);
                        return;
                    }
                }

            }
            else if (player.clickedObject.tag == "shopVendor")
            {
                player.clickedObject.GetComponent<shopVendor>().isTalking = true;

            }
            else if (player.clickedObject.tag == "ShopVendorButton")
            {
                player.clickedObject.GetComponent<shopVendorButton>().shopUI.SetActive(true);

            }
            else if (player.clickedObject.GetComponent<InteractableNPCCommon>())
            {
                GameObject dialogue = player.clickedObject.GetComponent<InteractableNPCCommon>().showDialogue(); // NPC's


                if (player.clickedObject.tag == "newspaper_guy")
                {
                    dialogue.GetComponent<NpcDialogueCommon>().player = gameObject.GetComponent<PlayerMovement>();
                    dialogue.GetComponent<NpcDialogueCommon>().addItemOnce = true;

                    if (player.questSystem_.endConditionQuest1_3)
                    {
                        player.clickedObject.GetComponent<InteractableNPCCommon>().text1 = "Back already thanks for helping buddy, here as a reward I'm pretty poor ";
                        player.clickedObject.GetComponent<InteractableNPCCommon>().text1_0 = "but here is half a ticket I found on the ground might help if you can find another half ";
                    }
                }

                if (player.clickedObject.tag == "TicketGuy")
                {
                    dialogue.GetComponent<NpcDialogueCommon>().player = gameObject.GetComponent<PlayerMovement>();
                    dialogue.GetComponent<NpcDialogueCommon>().addItemOnce = true;
                }

            }
            else if (player.clickedObject.tag == "DogRange")
            {
                player.questSystem_.startConditionQuest1_4 = true;

            }
            else if (player.clickedObject.tag == "sewageEntrance")
            {
                player.teleportPlayerToPos.screenPos = 0.64f;
                player.teleportPlayerToPos.playerTeleportPos = new Vector3(53.55f, -37.5f, gameObject.transform.position.z);
                player.teleportPlayerToPos.gameObject.SetActive(true);
                player.currentLocation = (int)PlayerMovement.locations.Sewage;
            }
            else if (player.clickedObject.tag == "sewageExit")
            {
                player.teleportPlayerToPos.screenPos = 0.8f;
                player.teleportPlayerToPos.playerTeleportPos = new Vector3(85.36f, -4.13f, gameObject.transform.position.z);
                player.teleportPlayerToPos.gameObject.SetActive(true);
                player.currentLocation = (int)PlayerMovement.locations.Streets;
            }
            else if (player.clickedObject.tag == "jumpOnBoxesStart")
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (player.clickedObject.tag == "pantString")
            {
                player.soundManager.playGenericItemRub();
                player.playerInventory.addNewInventoryItem(player.playerInventory.getAvailableItemByTag("pantString"));
                player.clickedObject.transform.parent.GetComponent<Animator>().SetBool("canPantsDown", true);
                Destroy(player.clickedObject);

            }
            else if (player.clickedObject.tag == "meatHook")
            {
                player.soundManager.playGenericItemRub();
                player.playerInventory.addNewInventoryItem(player.playerInventory.getAvailableItemByTag("meatHook"));
                Destroy(player.clickedObject);
            }
            else if (player.clickedObject.tag == "stick")
            {
                player.soundManager.playGenericItemRub();
                player.playerInventory.addNewInventoryItem(player.playerInventory.getAvailableItemByTag("stick"));
                Destroy(player.clickedObject);
            }
            else if (player.clickedObject.tag == "bucketSewage")
            {
                player.soundManager.playGenericItemRub();
                player.playerInventory.addNewInventoryItem(player.playerInventory.getAvailableItemByTag("bucketSewage"));
                Destroy(player.clickedObject);
            }
            else if (player.clickedObject.tag == "brickSewage")
            {
                if (player.clickedObject.GetComponent<SpriteRenderer>().color != new Color(0.267f, 0.267f, 0.267f) && !player.climbBucketStatus)
                {
                    player.soundManager.playBrickRub();
                    player.clickedObject.GetComponent<SpriteRenderer>().color = new Color(0.267f, 0.267f, 0.267f);
                    brickSewage += 1;
                    player.clickedObject.GetComponent<Collider2D>().enabled = false;
                }

            }
            else if (player.clickedObject.tag == "brickSewageUP")
            {
                if (player.climbBucketStatus && (player.clickedObject.GetComponent<SpriteRenderer>().color != new Color(0.267f, 0.267f, 0.267f)))
                {
                    player.soundManager.playBrickRub();
                    player.clickedObject.GetComponent<SpriteRenderer>().color = new Color(0.267f, 0.267f, 0.267f);
                    brickSewage += 1;
                    player.clickedObject.GetComponent<Collider2D>().enabled = false;
                }

            }
            else if (player.clickedObject.tag == "bucketClimb")
            {

                if (player.climbBucketStatus == false && !dontMoveUntilSewageHobboInPosition)
                {
                    player.anim2d.SetBool("canClimb", true);
                    player.climbBucketStatus = true;
                }

            }
            else if (player.clickedObject.tag == "hideSewageBox")
            {
                if (!player.climbBucketStatus)
                {
                    //sewageOutsideLight.SetActive(false);
                    player.anim2d.SetBool("canHideSewage", true);
                    handleShowHideUIText.SetActive(false);
                    dontMoveUntilSewageHobboInPosition = true;

                    player.cinematic.SetActive(true);
                    player.vcam.gameObject.transform.parent = null;

                    player.MoveCamera(0.38f, 0.15f, true);
                    fatHobbo.SetActive(true);
                    player.clickedObject.GetComponent<Collider2D>().enabled = false;
                    bucket.GetComponent<SpriteRenderer>().sortingOrder = -1;
                    bucket.GetComponent<Collider2D>().enabled = false;

                }
            }
            else if (player.clickedObject.tag == "hobbo")
            {
                player.anim2d.SetBool("canPush", true);
                hobbo = player.clickedObject;
                hobbo.GetComponent<Collider2D>().enabled = false;
            }
            else if (player.clickedObject.tag == "treasureKey")
            {
                player.soundManager.playGenericItemRub();
                player.playerInventory.addNewInventoryItem(player.playerInventory.getAvailableItemByTag("treasureKeyitem"));
                Destroy(player.clickedObject);
            }
            else if (player.clickedObject.tag == "ticket2")
            {
                player.soundManager.playGenericItemRub();
                player.playerInventory.addNewInventoryItem(player.playerInventory.getAvailableItemByTag("halfTicket2"));
                Destroy(player.clickedObject);
                ItemInterfaceTicket1.SetActive(false);
                ItemInterfaceTicket2.SetActive(true);
                QuestItemInterface.SetActive(true);
            }
            else if (player.clickedObject.tag == "coalPile")
            {

                foreach (GameObject inventoryItem in player.playerInventory.inventoryItems)
                {
                    if (inventoryItem.tag == "coalItem")
                    {
                        TextMeshProUGUI amountText = inventoryItem.transform.Find("amount").GetComponent<TextMeshProUGUI>();

                        string currentText = amountText.text;
                        int currentAmount = int.Parse(currentText.Substring(1));

                        if (currentAmount < 5)
                        {

                            player.soundManager.playGenericItemRub();
                            player.playerInventory.updateInventoryItemCountUp(inventoryItem);

                        }
                        return;
                    }
                }
                foreach (GameObject item in player.playerInventory.availabelItemsItems)
                {
                    if (item.tag == "coalItem")
                    {

                        player.soundManager.playGenericItemRub();
                        player.playerInventory.addNewInventoryItem(item);
                        return;
                    }
                }

            }
            else if (player.clickedObject.tag == "waterBucket")
            {
                player.soundManager.playMetalBucketClick();
                player.clickedObject.GetComponent<Animator>().SetBool("canEmpty", true);
                player.clickedObject.GetComponent<Collider2D>().enabled = false;
            }
            else if (player.clickedObject.tag == "streetCoal")
            {

                player.clickedObject.GetComponent<Animator>().SetBool("canEmpty", true);
                player.clickedObject.GetComponent<Collider2D>().enabled = false;
            }
            else if (player.clickedObject.tag == "janitorLadder")
            {
                if (janitor.GetComponent<Animator>().GetBool("canFall"))
                {
                    player.soundManager.playGenericItemRub();
                    player.playerInventory.addNewInventoryItem(player.playerInventory.getAvailableItemByTag("janitorLadderItem"));
                    Destroy(player.clickedObject);

                }
            }
            else if (player.clickedObject.tag == "climbLadderCat")
            {

                player.GetComponent<Animator>().SetBool("canLadderFall", true);
            }
            else if (player.clickedObject.tag == "gotToScene2")
            {
                startExitScene1.SetActive(true);

                player.soundManager.Music.Stop();
            }
        }
    }
}
