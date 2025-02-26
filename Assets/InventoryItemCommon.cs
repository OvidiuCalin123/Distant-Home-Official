using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemCommon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject hoverGlow;
    public GameObject item;
    private Transform originalParent;
    private Vector3 originalPosition;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private GraphicRaycaster raycaster; // For UI combination logic

    public DialogSystem dialogSystem;
    public PlayerMovement player;

    private void Start()
    {
        dialogSystem = FindObjectOfType<DialogSystem>();
        player = FindObjectOfType<PlayerMovement>();

        if (hoverGlow != null)
            hoverGlow.SetActive(false);

        originalParent = item.transform.parent;
        originalPosition = item.transform.localPosition;
        canvas = GetComponentInParent<Canvas>();

        canvasGroup = item.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = item.AddComponent<CanvasGroup>();

        raycaster = canvas.GetComponent<GraphicRaycaster>(); // Initialize GraphicRaycaster
    }

    // Hover Glow Handlers
    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverGlow?.SetActive(true);
        player.playerInventory.playItemSelect();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverGlow?.SetActive(false);
    }

    // Drag Handlers
    public void OnBeginDrag(PointerEventData eventData)
    {
        item.transform.SetParent(canvas.transform, true);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        dialogSystem.isDraggingInventoryItem = true;
        item.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        item.transform.SetParent(originalParent, true);
        item.transform.localPosition = originalPosition;
        canvasGroup.blocksRaycasts = true;

        dialogSystem.isDraggingInventoryItem = false;

        // Check for UI item combination using GraphicRaycaster
        if (HandleUICombination()) return;

        // Handle world interactions using Physics2D.Raycast
        HandleWorldInteraction();
    }

    private bool HandleUICombination()
    {
        // Prepare a PointerEventData for GraphicRaycaster
        PointerEventData pointerData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        foreach (RaycastResult result in results)
        {
            if (gameObject.CompareTag("pantString") && result.gameObject.CompareTag("stick"))
            {
                CombineItems("StringAndStick", "pantString", "stick");
                return true;
            }
            if (gameObject.CompareTag("pantString") && result.gameObject.CompareTag("meatHook"))
            {
                CombineItems("StringAndHook", "pantString", "meatHook");
                return true;
            }
            if (gameObject.CompareTag("meatHook") && result.gameObject.CompareTag("pantString"))
            {
                CombineItems("StringAndHook", "pantString", "meatHook");
                return true;
            }
            if (gameObject.CompareTag("stick") && result.gameObject.CompareTag("pantString"))
            {
                CombineItems("StringAndStick", "pantString", "stick");
                return true;
            }
            if (gameObject.CompareTag("StringAndStick") && result.gameObject.CompareTag("meatHook"))
            {
                CombineItems("StringHookStick", "StringAndStick", "meatHook");
                return true;
            }
            if (gameObject.CompareTag("StringAndHook") && result.gameObject.CompareTag("stick"))
            {
                CombineItems("StringHookStick", "StringAndHook", "stick");
                return true;
            }
            if (gameObject.CompareTag("stick") && result.gameObject.CompareTag("StringAndHook"))
            {
                CombineItems("StringHookStick", "StringAndHook", "stick");
                return true;
            }
            if (gameObject.CompareTag("meatHook") && result.gameObject.CompareTag("StringAndStick"))
            {
                CombineItems("StringHookStick", "StringAndStick", "meatHook");
                return true;
            }

            //--------------------------------- ACT2

            if (gameObject.CompareTag("threadBall") && result.gameObject.CompareTag("kitchenFork"))
            {
                CombineItems("forkAndString", "threadBall", "kitchenFork");
                return true;
            }
            if (gameObject.CompareTag("forkAndString") && result.gameObject.CompareTag("rope"))
            {
                CombineItems("forkStringRope", "forkAndString", "rope");
                return true;
            }

            if (gameObject.CompareTag("rope") && result.gameObject.CompareTag("forkAndString"))
            {
                CombineItems("forkStringRope", "rope", "forkAndString");
                return true;
            }
        }
        return false;
    }

    private void HandleWorldInteraction()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0f;
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

        if (gameObject.CompareTag("poorKey"))
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerData, results);

            foreach (RaycastResult result in results)
            {
                // Handle the case when poorKey is dragged over poorDoor
                if (gameObject.CompareTag("poorKey") && result.gameObject.CompareTag("poorDoor"))
                {
                    SetPlayerDirectionTowards(hit.collider.transform.position.x);
                    //player.playerInventory.removeItemFromInventory("poorKey");
                    //player.soundManager.playRemoveItem();
                    player.act2InteractionHandler.eventCandleLightUICanvas.SetActive(false);
                    player.act2InteractionHandler.LightFromCandleLightEventZone.SetActive(true);
                    player.act2InteractionHandler.poorCandles.SetActive(true);
                    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    player.act2InteractionHandler.doorPoor.GetComponent<Collider2D>();
                    player.act2InteractionHandler.poorDarkness.SetActive(false);
                    player.act2InteractionHandler.poorFloor.SetActive(true);

                    player.playerInventory.removeItemFromInventory("poorKey");
                    player.soundManager.playRemoveItem();
                    return;  // Exit after handling the interaction
                }
            }
        }

        if (hit.collider == null) return;

        Debug.Log(gameObject.tag + " " + hit.collider.name + " " + player.act2InteractionHandler.stoneConductorPlace.activeSelf);

        if (gameObject.CompareTag("StringHookStick") && hit.collider.CompareTag("FishTicket"))
        {
            player.cantMoveWhileInsideMiniGame = true;
            player.anim2d.SetBool("canFishTicket", true);
            player.anim2d.speed = 0;
            player.canBeginFishingAfterTicket = true;
            player.soundManager.playRemoveItem();

        }

        if (gameObject.CompareTag("ShopHam") && hit.collider.CompareTag("DogRange"))
        {
            SetPlayerDirectionTowards(player.DogQuest.transform.position.x);
            player.anim2d.SetBool("canThrowHam", true);
            player.playerInventory.removeItemFromInventory("ShopHam");
            player.questSystem_.endConditionQuest1_4 = true;
            player.soundManager.playRemoveItem();
        }
        else if (gameObject.CompareTag("Newspaper") && hit.collider.CompareTag("GuyReadingNews"))
        {
            SetPlayerDirectionTowards(player.DogQuest.transform.position.x);
            //player.ais[0].destination = hit.collider.transform.po
            player.playerInventory.removeItemFromInventory("Newspaper");
            player.questSystem_.endConditionQuest1_3 = true;
            var reader = hit.collider.GetComponent<ReadingNewsPaper>();
            reader.enableAnimation();
            hit.collider.GetComponent<Animator>().SetBool("canLookAtPlayer", true);
            player.soundManager.playRemoveItem();
        }
        else if (gameObject.CompareTag("bucketSewage") && hit.collider.CompareTag("bucketSewagePlace"))
        {
            SetPlayerDirectionTowards(player.DogQuest.transform.position.x);
            player.playerInventory.removeItemFromInventory("bucketSewage");
            player.BucketSewage.SetActive(true);
            hit.collider.enabled = false;
            player.soundManager.playRemoveItem();
        }
        else if (gameObject.CompareTag("treasureKeyitem") && hit.collider.CompareTag("treasure"))
        {
            SetPlayerDirectionTowards(hit.collider.transform.position.x);
            player.playerInventory.removeItemFromInventory("treasureKeyitem");
            hit.collider.enabled = false;
            hit.collider.gameObject.GetComponent<Animator>().SetBool("canOpen", true);
            player.playerInventory.addNewInventoryItem(player.playerInventory.getAvailableItemByTag("goldenRing"));
            player.soundManager.playGenericItemRub();
            player.soundManager.playRemoveItem();
        }
        else if (gameObject.CompareTag("goldenRing") && hit.collider.CompareTag("jimmyHand"))
        {
            SetPlayerDirectionTowards(hit.collider.transform.position.x);
            player.playerInventory.removeItemFromInventory("goldenRing");
            hit.collider.enabled = false;
            hit.collider.gameObject.GetComponent<Animator>().SetBool("canRetract", true);
            player.act1InteractionHandler.SewageExit.enabled = true;
            Destroy(player.act1InteractionHandler.jimmy);
            player.soundManager.playRemoveItem();
        }
        else if (gameObject.CompareTag("coalItem") && hit.collider.CompareTag("streetCoal"))
        {
            SetPlayerDirectionTowards(hit.collider.transform.position.x);
            player.playerInventory.updateInventoryItemCountDown(gameObject, 1);
            hit.collider.enabled = false;
            player.act1InteractionHandler.coalTrail.SetActive(true);
            player.soundManager.playRemoveItem();
        }
        else if (gameObject.CompareTag("janitorLadderItem") && hit.collider.CompareTag("ladderCat"))
        {
            SetPlayerDirectionTowards(hit.collider.transform.position.x);
            player.playerInventory.removeItemFromInventory("janitorLadderItem");
            hit.collider.enabled = false;
            player.act1InteractionHandler.ladderCat.SetActive(true);
            player.soundManager.playRemoveItem();
        }
        else if (gameObject.CompareTag("candlePickup") && hit.collider.CompareTag("candlePickUpLightArea"))
        {
            SetPlayerDirectionTowards(hit.collider.transform.position.x);
            player.soundManager.playRemoveItem();
            player.act2InteractionHandler.playerCatEyes.SetActive(false);
            player.act2InteractionHandler.floorCollistionCandleEventCantMoveWithoutLight.GetComponent<Collider2D>().enabled = true;
            player.act2InteractionHandler.CandlePlayerStorage.SetActive(true);

            player.act2InteractionHandler.putLightHereColliderStorageRoom.SetActive(false);
        }
        else if (gameObject.CompareTag("KitchenSoup") && hit.collider.CompareTag("KitchenFatGuy"))
        {
            SetPlayerDirectionTowards(hit.collider.transform.position.x);
            hit.transform.gameObject.GetComponent<Animator>().SetBool("canPopp", true);
            player.playerInventory.removeItemFromInventory("KitchenSoup");
            player.soundManager.playRemoveItem();
        }
        else if (gameObject.CompareTag("spiderBoxItem") && hit.collider.CompareTag("spiderBoxPlacement"))
        {
            SetPlayerDirectionTowards(hit.collider.transform.position.x);
            player.playerInventory.removeItemFromInventory("spiderBoxItem");
            player.act2InteractionHandler.showSpiderBoxPlacement.SetActive(true);
            player.soundManager.playRemoveItem();
            hit.transform.gameObject.SetActive(false);
        }
        else if (gameObject.CompareTag("poorSpider") && hit.collider.CompareTag("paperClip"))
        {
            SetPlayerDirectionTowards(hit.collider.transform.position.x);
            player.playerInventory.removeItemFromInventory("poorSpider");
            player.soundManager.playRemoveItem();
            player.playerInventory.addNewInventoryItem(player.playerInventory.getAvailableItemByTag("paperClip"));

            player.act2InteractionHandler.paperClip.SetActive(false);
        }
        else if (gameObject.CompareTag("buttonFat") && hit.collider.CompareTag("poorGirl"))
        {
            SetPlayerDirectionTowards(hit.collider.transform.position.x);
            player.playerInventory.removeItemFromInventory("buttonFat");
            player.soundManager.playRemoveItem();
            player.playerInventory.addNewInventoryItem(player.playerInventory.getAvailableItemByTag("threadBall"));

            hit.transform.gameObject.GetComponent<Collider2D>().enabled = false;
        }
        else if (gameObject.CompareTag("pickLock") && hit.collider.CompareTag("pickLockThis"))
        {
            SetPlayerDirectionTowards(hit.collider.transform.position.x);

            player.act2InteractionHandler.lockPickMiniGame.SetActive(true);

        }
        else if (gameObject.CompareTag("paperClip") && hit.collider.CompareTag("createPickLock"))
        {
            SetPlayerDirectionTowards(hit.collider.transform.position.x);
            player.playerInventory.removeItemFromInventory("paperClip");
            player.soundManager.playRemoveItem();
            player.playerInventory.addNewInventoryItem(player.playerInventory.getAvailableItemByTag("pickLock"));

        }
        else if (gameObject.CompareTag("paperBall") && hit.collider.CompareTag("kitchenFork"))
        {
            SetPlayerDirectionTowards(hit.collider.transform.position.x);
            player.playerInventory.removeItemFromInventory("paperBall");
            player.soundManager.playRemoveItem();
            player.anim2d.SetBool("canThrowPaper", true);

        }
        else if (gameObject.CompareTag("forkStringRope") && hit.collider.CompareTag("placeRopeHook"))
        {
            SetPlayerDirectionTowards(hit.collider.transform.position.x);
            player.playerInventory.removeItemFromInventory("forkStringRope");
            player.soundManager.playRemoveItem();
            player.act2InteractionHandler.ropeHookOutside.SetActive(true);

        }
        else if (gameObject.CompareTag("richRingItem") && hit.collider.name == "FindRing")
        {
            SetPlayerDirectionTowards(hit.collider.transform.position.x);
            player.playerInventory.removeItemFromInventory("richRingItem");
            player.soundManager.playRemoveItem();
            player.act2InteractionHandler.WomanDoorQuestDone = true;

        }
        else if (gameObject.CompareTag("knifeRichTable") && hit.collider.name == "WindowRichOpen")
        {
            SetPlayerDirectionTowards(hit.collider.transform.position.x);
            hit.collider.gameObject.GetComponent<Animator>().SetBool("canOpen", true);
            player.act2InteractionHandler.richAreaFog.GetComponent<Animator>().SetBool("noFog", true);
            player.playerInventory.removeItemFromInventory("knifeRichTable");
            player.soundManager.playRemoveItem();

        }
        else if (gameObject.CompareTag("sockItem") && hit.collider.name == "borkPipe")
        {
            SetPlayerDirectionTowards(hit.collider.transform.position.x);
            player.playerInventory.removeItemFromInventory("sockItem");
            player.soundManager.playRemoveItem();
            hit.collider.gameObject.GetComponent<Animator>().SetBool("sockPipe", true);
            hit.collider.enabled = false;
            player.act2InteractionHandler.canUseFireMeter = true;

        }
        else if (gameObject.CompareTag("plankConductor") && (hit.collider.name == "PuzzleConductorPlace" || hit.collider.name == "PuzzleConductor") && player.act2InteractionHandler.stoneConductorPlace.activeSelf)
        {
            SetPlayerDirectionTowards(hit.collider.transform.position.x);
            player.act2InteractionHandler.WoodConductorPlace.SetActive(true);
            player.playerInventory.removeItemFromInventory("plankConductor");
            player.soundManager.playRemoveItem();
            player.act2InteractionHandler.PuzzleConductorPlace.enabled = false;

        }
        else if (gameObject.CompareTag("stoneConductor") && hit.collider.name == "PuzzleConductorPlace")
        {
            SetPlayerDirectionTowards(hit.collider.transform.position.x);
            player.act2InteractionHandler.stoneConductorPlace.SetActive(true);
            player.playerInventory.removeItemFromInventory("stoneConductor");
            player.soundManager.playRemoveItem();

        }
        else if (gameObject.CompareTag("coalItem") && hit.collider.name == "PuzzleConductor")
        {
            SetPlayerDirectionTowards(hit.collider.transform.position.x);
            int itemCount = player.playerInventory.updateInventoryItemCountDown(gameObject, 1);
            if(itemCount <= 0)
            {
                player.playerInventory.removeItemFromInventory("coalItem");
                player.act2InteractionHandler.coalGot = false;
            }
            player.soundManager.playRemoveItem();
            player.act2InteractionHandler.coalConductorPlank.SetActive(true);
        }
        else if (gameObject.CompareTag("ShatteredBottle") && hit.collider.name == "conductor")
        {
            SetPlayerDirectionTowards(hit.collider.transform.position.x);
            player.playerInventory.removeItemFromInventory("ShatteredBottle");
            player.soundManager.playRemoveItem();
        }
    }

    private void SetPlayerDirectionTowards(float targetX)
    {
        player.transform.rotation = player.transform.position.x < targetX
            ? Quaternion.Euler(0, 0, 0)
            : Quaternion.Euler(0, 180, 0);
    }

    private void CombineItems(string newItemTag, string removeTag1, string removeTag2)
    {
        player.playerInventory.addNewInventoryItem(player.playerInventory.getAvailableItemByTag(newItemTag));
        player.playerInventory.removeItemFromInventory(removeTag1);
        player.playerInventory.removeItemFromInventory(removeTag2);
    }
}
