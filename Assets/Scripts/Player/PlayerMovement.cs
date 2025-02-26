using UnityEngine;
using System.Linq;
using Pathfinding;
using Cinemachine;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Controls and Movement")]
    public Animator anim2d;
    public bool canWalk;

    [Header("Camera Settings")]
    public CinemachineVirtualCamera vcam;
    public float maxCameraSize = 7.4f;
    public float minCameraSize = 6.6f;
    public float cameraAdjustSpeed = 0.1f;
    private bool isInOutsideTrainCameraZone = false;
    public float outsideTrainCameraSize;
    public bool doorStopCamera;

    [Header("AI and Targeting")]
    public LayerMask mask;
    public Transform target;
    public IAstarAI[] ais;

    [Header("Mouse Interactions")]
    public GameObject mouseClickGroundEffect;
    private GameObject currentEffect;
    public GameObject clickedObject;
    public bool pickedInteractableDestination;

    [Header("Steal System")]
    public stealSystem stealSystem_ref;
    public GameObject stealUIPopup;
    public GameObject UI_StolenItem_text;

    [Header("Inventory System")]
    public int playerCoins;
    public PlayerInventory playerInventory;

    [Header("Quest Items and Spawning")]
    public GameObject hamForTheDog;
    public Transform spawnPointHamForTheDog;
    public dogBarkRange DogQuest;
    public QuestSystem questSystem_;

    [Header("Player Actions and States")]
    public bool playerSittingOnBoxes;
    public bool playerClimbingBoxes;
    public GameObject jumpOnBoxesPopUp;

    [Header("Fishing Mini-Game")]
    public bool fishAfterTicket;
    public Collider2D beggarWithHalfTicket;
    public bool canBeginFishingAfterTicket;
    public GameObject TicketUnderBeggarFoot;
    public Transform fishTicketHookPos;
    public GameObject windWithTicket;
    public Transform windWithTicketPosition;

    [Header("Cinematics and Mini-Games")]
    public bool cantMoveWhileInsideMiniGame;
    public GameObject cinematic;
    public bool makeRatTalk;
    public bool playerSitingOnBoxes;

    [Header("Environment and Interactions")]
    public GameObject BucketSewage;
    public bool climbBucketStatus;
    public Act1WorldInter act1InteractionHandler;
    public Act2WorldInter act2InteractionHandler;
    public PlayerAnimationHelper playerAnimHelper;

    [Header("Player Appearance and Audio")]
    public SpriteRenderer playerRenderer;
    public SoundManager soundManager;

    [Header("Location and Miscellaneous")]
    public int currentLocation;
    public TeleportPlayerToPos teleportPlayerToPos;
    public bool isOnSpiderBox;

    public enum locations
    {
        Streets,
        Sewage,
        TrainGeneric,
        TrainRich,
        OnTrain,
    }

    void Start()
    {
        isOnSpiderBox = false;
        playerAnimHelper = GetComponent<PlayerAnimationHelper>();
        anim2d = GetComponent<Animator>();
        playerRenderer = GetComponent<SpriteRenderer>();
        playerInventory = GetComponent<PlayerInventory>();
        act1InteractionHandler = GetComponent<Act1WorldInter>();
        act2InteractionHandler = GetComponent<Act2WorldInter>();

        climbBucketStatus = false;
        makeRatTalk = false;
        cantMoveWhileInsideMiniGame = false;
        canBeginFishingAfterTicket = false;
        pickedInteractableDestination = false;
        playerSitingOnBoxes = false;
        playerClimbingBoxes = false;

        vcam = FindObjectOfType<CinemachineVirtualCamera>();

        ais = FindObjectsOfType<MonoBehaviour>().OfType<IAstarAI>().ToArray();

        if (beggarWithHalfTicket != null)
        {
            beggarWithHalfTicket.enabled = false;
        }

        if(act2InteractionHandler != null)
        {
            currentLocation = (int)locations.TrainGeneric;
        }

    }

    public void jumpOnSpiderBox()
    {
        if (clickedObject.tag == "spiderBox" && isOnSpiderBox == false)
        {
            
            ais[0].destination = new Vector2(transform.position.x, transform.position.y + 2.2f);
            float targetY = transform.position.y + 2.2f; // Target position
            float moveSpeed = 2f; // Speed of movement (adjust as needed)

            // Move smoothly to the target position
            while (Mathf.Abs(transform.position.y - targetY) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position,
                    new Vector3(transform.position.x, targetY, transform.position.z),
                    moveSpeed * Time.deltaTime);
            }
            
            isOnSpiderBox = true;

        }
        else if (clickedObject.tag == "spiderBox" && isOnSpiderBox == true)
        {
            ais[0].destination = new Vector2(transform.position.x, transform.position.y - 2.2f);
            float targetY = transform.position.y - 2.2f; // Target position
            float moveSpeed = 2f; // Speed of movement (adjust as needed)

            // Move smoothly to the target position
            while (Mathf.Abs(transform.position.y - targetY) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position,
                    new Vector3(transform.position.x, targetY, transform.position.z),
                    moveSpeed * Time.deltaTime);
            }

            isOnSpiderBox = false;
        }
    }

    void Update()
    {
        if (!cinematic.activeSelf)
        {
            if (canBeginFishingAfterTicket)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    anim2d.SetFloat("direction", 1);
                    anim2d.speed = 0.5f;

                }
                if (Input.GetKeyUp(KeyCode.R))
                {
                    anim2d.SetFloat("direction", -1);
                    anim2d.speed = 0.8f;
                }
            }
            if(jumpOnBoxesPopUp != null)
            if (jumpOnBoxesPopUp.activeSelf == true && playerSitingOnBoxes && Input.GetKey(KeyCode.E))
            {
                anim2d.SetBool("canJump", true);
                playerSitingOnBoxes = false;
                jumpOnBoxesPopUp.SetActive(false);
            }

            if (anim2d.GetBool("canBeScared"))
            {

                ais[0].destination = gameObject.transform.position;
            }

            if (stealSystem_ref)
            {
                if (Input.GetKey(KeyCode.E) && stealSystem_ref.canSteal)
                {

                    if (stealSystem_ref)
                    {
                        anim2d.SetBool("canSteal", true);

                        float targetSize = 5.3f;
                        if (vcam.m_Lens.OrthographicSize != targetSize)
                        {
                            vcam.m_Lens.OrthographicSize = Mathf.MoveTowards(vcam.m_Lens.OrthographicSize, targetSize, 5f * Time.deltaTime);
                        }

                        if (transform.position.x < stealSystem_ref.transform.position.x)
                        {
                            transform.rotation = Quaternion.Euler(0, 0, 0);
                        }
                        else
                        {
                            transform.rotation = Quaternion.Euler(0, 180, 0);
                        }
                    }


                }
                else
                {
                    anim2d.SetBool("canSteal", false);
                    float targetSize = 6.6f;
                    if (vcam.m_Lens.OrthographicSize != targetSize)
                    {
                        vcam.m_Lens.OrthographicSize = Mathf.MoveTowards(vcam.m_Lens.OrthographicSize, targetSize, 10 * Time.deltaTime);
                    }
                }
            }

            if (ais[0].reachedDestination)
            {
                canWalk = false;

                if (pickedInteractableDestination)
                {
                    pickedInteractableDestination = false;
                    if (act2InteractionHandler == null)
                    {
                        act1InteractionHandler.handleAct1InteractiveWorldEvent();
                    }
                    if (act1InteractionHandler == null)
                    {
                        act2InteractionHandler.handleAct2InteractiveWorldEvent();
                    }
                }

                if (currentEffect != null)
                {
                    Destroy(currentEffect);
                }
            }

            
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                PointAndClick(ais[0]);
            }

            anim2d.SetBool("canWalk", canWalk);

            AdjustCameraSize();

        }
    }

    private void handleSpawnGroundClickEffect(Vector2 mousePos, RaycastHit2D? mostRelevantHit)
    {
        if (clickedObject != null)
        {
            if (clickedObject.layer == 6)
            {
                anim2d.SetBool("canJump", false);
                currentEffect = Instantiate(mouseClickGroundEffect, mostRelevantHit.Value.point, Quaternion.identity);

                //playGroundClick();

                if (mousePos.x < transform.position.x)
                {
                    currentEffect.transform.localScale = new Vector3(-0.6f, 0.6f, 1);
                }
                else
                {
                    currentEffect.transform.localScale = new Vector3(0.6f, 0.6f, 1);
                }
            }
        }
    }

    private void HandleNoMovementWhenClickingDialogue(IAstarAI ai, RaycastHit2D? mostRelevantHit, Vector2 mousePos)
    {
        if (ShouldSetInteractableDestination())
        {
            pickedInteractableDestination = true;
        }

        if (ShouldAllowWalking())
        {
            ProcessHitObject(ai, mostRelevantHit, mousePos);
            UpdatePlayerOrientation(mousePos);
            HandleEffectSpawn(mousePos, mostRelevantHit);
        }
    }

    private bool ShouldSetInteractableDestination()
    {
        return clickedObject.layer != 6
               && !anim2d.GetBool("canHideSewage")
               && !anim2d.GetBool("canHideReverse")
               && !act2InteractionHandler.isPlayerOnTrain;
    }

    private bool ShouldAllowWalking()
    {
        return clickedObject.tag != "DogRange" && !anim2d.GetBool("canBeScared");
    }

    private void ProcessHitObject(IAstarAI ai, RaycastHit2D? mostRelevantHit, Vector2 mousePos)
    {
        if (!mostRelevantHit.HasValue) return;

        var hitCollider = mostRelevantHit.Value.collider;

        if (!climbBucketStatus)
        {
            canWalk = true;
        }

        if (clickedObject.layer == 10)
        {
            
            HandleLayer10Logic(ai);
        }
        else
        {
            HandleDefaultLayerLogic(ai, mostRelevantHit);
        }
    }

    private void HandleLayer10Logic(IAstarAI ai)
    {
        if (!climbBucketStatus)
        {
            if (act1InteractionHandler != null)
            {
                if (act1InteractionHandler.dontMoveUntilSewageHobboInPosition && clickedObject.tag == "hobbo")
                {
                    act1InteractionHandler.HandleHobboInteraction(ai);
                    return;
                }
            }

            if (act2InteractionHandler != null)
            {
                if (isOnSpiderBox)
                {
                    //anim2d.SetBool("canJump", true);
                    return;
                }
            }

            
            if (clickedObject.name == "PuzzleConductor" && act2InteractionHandler.coalConductorPlank.activeSelf)
            {
                act2InteractionHandler.WoodConductorPlace.GetComponent<Animator>().SetBool("canHit", true);
            }
            if (act2InteractionHandler.isTiedConductor == true)
            {
                if (clickedObject.name == "woodConductor"
                    || clickedObject.name == "stoneConductor"
                    || clickedObject.name == "CoalsConductor"
                    || clickedObject.name == "pickShardsCollider")
                {
                    Debug.Log("act2InteractionHandler.isTiedConductor " + act2InteractionHandler.isTiedConductor);
                    float targetX = clickedObject.transform.position.x;

                    // Move the player to the target position using a while loop
                    StartCoroutine(MoveToTarget(targetX));
                } 
            }
            else
            {
                MoveToPlayerPosition(ai);
            }
        }
    }

    public IEnumerator MoveToTarget(float targetX)
    {
        float speed = 1.25f;
        anim2d.speed = 1;

        while (Mathf.Abs(transform.position.x - targetX) > 0.01f)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                break;
            }
            anim2d.SetBool("moveTied", true);
            Debug.Log("ASA");
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetX, transform.position.y, transform.position.z), speed * Time.deltaTime);
            yield return null; // Wait for next frame
        }
        anim2d.SetBool("moveTied", false);

        if (Mathf.Abs(transform.position.x - targetX) <= 0.01f)
        {

            if (clickedObject.name == "woodConductor")
            {
                soundManager.playGenericItemRub();
                playerInventory.addNewInventoryItem(playerInventory.getAvailableItemByTag("plankConductor"));
                Destroy(clickedObject);
            }
            else if (clickedObject.name == "stoneConductor")
            {
                soundManager.playGenericItemRub();
                playerInventory.addNewInventoryItem(playerInventory.getAvailableItemByTag("stoneConductor"));
                Destroy(clickedObject);
            }
            else if(clickedObject.name == "CoalsConductor")
            {
                if (!act2InteractionHandler.coalGot)
                {
                    act2InteractionHandler.coalGot = true;
                    playerInventory.addNewInventoryItem(playerInventory.getAvailableItemByTag("coalItem"));
                }
                
                foreach (GameObject inventoryItem in playerInventory.inventoryItems)
                {
                    if (inventoryItem.tag == "coalItem")
                    {
                        TextMeshProUGUI amountText = inventoryItem.transform.Find("amount").GetComponent<TextMeshProUGUI>();

                        string currentText = amountText.text;
                        int currentAmount = int.Parse(currentText.Substring(1));

                        Debug.Log(currentAmount);

                        if(currentAmount == 0)
                        {
                            playerInventory.addNewInventoryItem(playerInventory.getAvailableItemByTag("coalItem"));
                        }

                        if (currentAmount < 5)
                        {

                            soundManager.playGenericItemRub();
                            playerInventory.updateInventoryItemCountUp(inventoryItem);

                        }
                        break;
                    }
                }
                
            }
            else if (clickedObject.name == "pickShardsCollider")
            {
                anim2d.SetBool("removeRope", true);
            }
        }
    }

    public void GoToIdleFromRemovedRope()
    {
        anim2d.SetBool("idleTied", false);
        act2InteractionHandler.isTiedConductor = false;
        act2InteractionHandler.isPlayerInTunnel = false;
        act2InteractionHandler.isPlayerOnTrain = false;
        anim2d.SetBool("removeRope", false);
    }

    public void MoveToPlayerPosition(IAstarAI ai)
    {
        ai.destination = clickedObject.transform.Find("playerPosition").transform.position;
    }

    private void HandleDefaultLayerLogic(IAstarAI ai, RaycastHit2D? mostRelevantHit)
    {
        if (act1InteractionHandler != null)
        {
            if (!act1InteractionHandler.dontMoveUntilSewageHobboInPosition)
            {
                if (climbBucketStatus)
                {
                    HandleClimbing(ai);

                    return;
                }
                else if (anim2d.GetBool("canHideSewage"))
                {
                    anim2d.SetBool("canHideReverse", true);

                    return;
                }
            }
        }

        if (act2InteractionHandler != null)
        {
            if (isOnSpiderBox)
            {
                anim2d.SetBool("canJump", true);
                return;
            }
        }

        ai.destination = mostRelevantHit.Value.point;
    }

    private void HandleClimbing(IAstarAI ai)
    {
        anim2d.SetBool("canReverseClimb", true);
        transform.position = new Vector3(transform.position.x, transform.position.y - 1.06f, transform.position.z);
        ais[0].destination = transform.position;
    }

    public void UpdatePlayerOrientation(Vector2 mousePos)
    {
        if (mousePos.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        stealUIPopup.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void HandleEffectSpawn(Vector2 mousePos, RaycastHit2D? mostRelevantHit)
    {
        if (currentEffect != null)
        {
            Destroy(currentEffect);
        }
        handleSpawnGroundClickEffect(mousePos, mostRelevantHit);
    }

    private void DisableObjectStates()
    {
        if (clickedObject == null) return;

        var dialogueNav = clickedObject.GetComponent<DialogueNavigation>();
        if (dialogueNav != null)
        {
            dialogueNav.playerNearNPC = false;
        }

        var shopVendor = clickedObject.GetComponent<shopVendor>();
        if (shopVendor != null)
        {
            shopVendor.isTalking = false;
        }
    }

    private void PointAndClick(IAstarAI ai)
    {
        if (CanInteract())
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleInteraction(ai);
            }
        }
    }

    private bool CanInteract()
    {
        if (act2InteractionHandler)
        {
            return !anim2d.GetBool("canSteal")
              && !anim2d.GetBool("canJump")
              && !cantMoveWhileInsideMiniGame
              && !anim2d.GetBool("canLadderFall")
              && !anim2d.GetBool("canLadderFallGetUp")
              && !act2InteractionHandler.isOnOutsideRope
              && !act2InteractionHandler.isPlayerOnTrain;
        }
        else
        {
            return !anim2d.GetBool("canSteal")
               && !anim2d.GetBool("canJump")
               && !cantMoveWhileInsideMiniGame
               && !anim2d.GetBool("canLadderFall")
               && !anim2d.GetBool("canLadderFallGetUp");
        }
        
    }

    private void HandleInteraction(IAstarAI ai)
    {
        if (playerSitingOnBoxes || fishAfterTicket)
        {
            act1InteractionHandler.HandleFishing();
        }
        else
        {
            HandleGeneralClick(ai);
        }
    }

    private void HandleGeneralClick(IAstarAI ai)
    {
        ais[0].canMove = true;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        DisableObjectStates();

        if (hit.collider != null)
        {
            clickedObject = hit.collider.gameObject;

            if (clickedObject != null && clickedObject.tag == "DogRange")
            {
                act1InteractionHandler.HandleDogRangeInteraction();
                return;
            }

            HandleNoMovementWhenClickingDialogue(ai, hit, mousePos);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "doorStopCamera")
        {
            vcam.Follow = null;
            doorStopCamera = true;
        }

        if (collision.gameObject.CompareTag("OutsideTrainRamp"))
        {
            // Define the values inside the if statement
            float minScreenY = 0.6f; // Minimum screen Y value (camera won't go lower than this)
            float maxScreenY = 0.7f; // Maximum screen Y value (when the player is at the ramp's center)
            float zoomInSize = 8f;   // Normal zoom size (for when the player is far away)
            float zoomOutSize = 7f;  // Zoom out size (for when the player is close)
            float transitionSpeed = 2f;

            // Calculate the distance of the player from the pivot point (center of the ramp)
            float distanceFromCenter = Vector2.Distance(transform.position, collision.transform.position);

            // Normalize the distance based on some max range (adjust the range as needed)
            float normalizedDistance = Mathf.InverseLerp(0, 5f, distanceFromCenter); // Assume max range is 5 units for this example

            // Interpolate the camera properties based on the normalized distance
            // Reverse the logic for zooming out when closer and zooming in when further away
            float targetScreenY = Mathf.Lerp(maxScreenY, minScreenY, normalizedDistance);
            float targetZoom = Mathf.Lerp(zoomOutSize, zoomInSize, normalizedDistance); // Reverse the lerp for zoom

            // Apply the smooth transition to the camera properties
            CinemachineFramingTransposer framingTransposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
            framingTransposer.m_ScreenY = Mathf.Lerp(framingTransposer.m_ScreenY, targetScreenY, Time.deltaTime * transitionSpeed);

            // Adjust the zoom level (OrthographicSize) smoothly
            vcam.m_Lens.OrthographicSize = Mathf.Lerp(vcam.m_Lens.OrthographicSize, targetZoom, Time.deltaTime * transitionSpeed);
        }


        if (collision.gameObject.tag == "FullShadow")
        {
            Color newColor;
            if (ColorUtility.TryParseHtmlString("#686868", out newColor))
            {
                playerRenderer.color = newColor;
            }
        }

        if(collision.gameObject.tag == "jumpOnBoxesStart")
        {
            if (Input.GetKey(KeyCode.E))
            {
                playerClimbingBoxes = true;
                teleportPlayerToPos.screenPos = 0.6f;
                teleportPlayerToPos.playerTeleportPos = new Vector3(72.98f, -2.13f, gameObject.transform.position.z);
                gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
                teleportPlayerToPos.gameObject.SetActive(true);
            }

            jumpOnBoxesPopUp.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "GroundRichArea")
        {
            currentLocation = (int)locations.TrainGeneric;
        }

        if (collision.gameObject.tag == "doorStopCamera")
        {
            if (!teleportPlayerToPos.isActiveAndEnabled)
            {
                vcam.Follow = transform;

            }
            doorStopCamera = false;
        }

        if (collision.gameObject.tag == "OutsideRopePoint1")
        {
            act2InteractionHandler.miniGameOutsidePoint1 = false;
            anim2d.SetBool("canClimbTrainRopeHold", false);
        }

        if (collision.gameObject.tag == "OutsideRopePoint2")
        {
            anim2d.SetBool("canClimbTrainRopeHold", false);
            act2InteractionHandler.miniGameOutsidePoint2 = false;
        }

        if (collision.gameObject.tag == "FullShado")
        {
            Color newColor;
            if (ColorUtility.TryParseHtmlString("#FFFFFF", out newColor)) // Convert hex to Color
            {
                playerRenderer.color = newColor; // Assign the parsed color
            }
        }
        if (collision.gameObject.tag == "jumpOnBoxesStart")
        {
            jumpOnBoxesPopUp.SetActive(false);
            playerSitingOnBoxes = false;
        }

        if (collision.gameObject.tag == "candlePickUpLightArea")
        {
            act2InteractionHandler.playerCatEyes.SetActive(false);
        }
    }

    private void AdjustCameraSize()
    {
        //if (isInOutsideTrainCameraZone)
        //{
            
        //    vcam.Follow = act2InteractionHandler.cameraFollowPointTrainOutside.transform;
        //    vcam.m_Lens.OrthographicSize = 7f;
        //    soundManager.BackgroundNoise.volume = 1f;
        //}
        //else
        //{
        //    vcam.m_Lens.OrthographicSize = 6.6f;

        //    if (!doorStopCamera)
        //    {
        //        vcam.Follow = transform;
        //    }
           
        //    soundManager.BackgroundNoise.volume = 0.25f;

        //}
    }

    private Coroutine moveCoroutine;  // To store the coroutine

    // Call this to start the camera move.
    public void MoveCamera(float moveDistance, float moveSpeed, bool moveRight)
    {
        // If a movement is already running, stop it
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        // Start the coroutine to move the camera
        moveCoroutine = StartCoroutine(MoveCameraCoroutine(moveDistance, moveSpeed, moveRight));
    }

    // Coroutine that moves the camera smoothly
    private IEnumerator MoveCameraCoroutine(float moveDistance, float moveSpeed, bool moveRight)
    {
        var framingTransposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();

        // While we still have distance to cover
        while (moveDistance > 0f)
        {
            // Calculate the amount to move this frame
            float deltaMovement = moveSpeed * Time.deltaTime;

            // Move the camera to the right or left by adjusting m_ScreenX
            if (moveRight)
            {
                framingTransposer.m_ScreenX += deltaMovement; // Move right
            }
            else
            {
                framingTransposer.m_ScreenX -= deltaMovement; // Move left
            }

            // Subtract the moved distance from the total distance
            moveDistance -= deltaMovement;

            // Wait for the next frame to continue moving (prevent freezing)
            yield return null;  // This pauses the coroutine until the next frame
        }

        // Once the movement is complete, set a flag or do any other action
        makeRatTalk = true;
    }

    public IEnumerator CallMoveCameraAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        MoveCamera(0.25f, 0.15f, true);
        cinematic.GetComponent<Cinematic>().endCinematic();
        act2InteractionHandler.girlWithRing.enabled = true;
    }

    public void setCoalsToFalse()
    {
        anim2d.SetBool("canCoal", false);
        act2InteractionHandler.coalShovel.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "KillearAppearCollision")
        {
            collision.enabled = false;
            act2InteractionHandler.killer.SetActive(true);

        }
        if (collision.gameObject.name == "TunnelEvent")
        {
            collision.enabled = false;
            act2InteractionHandler.isPlayerInTunnel = true;

        }
        if (collision.gameObject.tag == "startRatCinematic")
        {
            Destroy(collision.gameObject);
            ais[0].destination = transform.position;
            anim2d.SetBool("canWalk", false);
            cinematic.SetActive(true);
            vcam.gameObject.transform.parent = null;
            
            MoveCamera(0.38f, 0.15f, false);

        }
        if (collision.gameObject.name == "FindRingCinematic")
        {
            Destroy(collision.gameObject);
            ais[0].destination = transform.position;
            anim2d.SetBool("canWalk", false);
            cinematic.SetActive(true);
            vcam.gameObject.transform.parent = null;

            MoveCamera(0.25f, 0.11f, false);
            StartCoroutine(CallMoveCameraAfterDelay(5f));
        }

        if (collision.gameObject.name == "GroundRichArea")
        {
            currentLocation = (int)locations.TrainRich;
        }

        if (collision.gameObject.tag == "OutsideRopePoint1")
        {
            act2InteractionHandler.miniGameOutsidePoint1 = true;
            anim2d.SetBool("canClimbTrainRopeHold", true);
        }

        if (collision.gameObject.tag == "OutsideRopePoint2")
        {
            act2InteractionHandler.miniGameOutsidePoint2 = true;
            anim2d.SetBool("canClimbTrainRopeHold", true);
        }

        if (collision.gameObject.tag == "getDownTrainRope")
        {
            if (act2InteractionHandler.isOnOutsideRope)
            {
                act2InteractionHandler.trainOutsideFloorMidCollision.enabled = true;
                act2InteractionHandler.trainOutsideDoorMidCollision.enabled = true;
                act2InteractionHandler.isOnOutsideRope = false;
                anim2d.SetBool("canClimbTrainRope", false);
                anim2d.SetBool("canClimbTrainRopeMove", false);
            }
           
        }

        if (collision.gameObject.tag == "getDownTrainRopeRich")
        {
            if (act2InteractionHandler.isOnOutsideRope)
            {
                act2InteractionHandler.trainOutsideFloorCollision.enabled = true;
                act2InteractionHandler.trainOutsideDoorCollision.enabled = true;
                act2InteractionHandler.isOnOutsideRope = false;
                anim2d.SetBool("canClimbTrainRope", false);
                anim2d.SetBool("canClimbTrainRopeMove", false);

                act2InteractionHandler.trainOutsideFloorMidCollision.enabled = false;
                act2InteractionHandler.trainOutsideDoorMidCollision.enabled = false;
            }

        }

        if (collision.gameObject.tag == "cinematicViktor")
        {
            act1InteractionHandler.exitScene1Door.GetComponent<Animator>().SetBool("canOpen", true);
            collision.gameObject.SetActive(false);
            //Destroy(collision.gameObject);
            //ais[0].destination = transform.position;
            //anim2d.SetBool("canWalk", false);
            //cinematic.SetActive(true);
            //vcam.gameObject.transform.parent = null;

            //MoveCamera(0.38f, 0.15f, false);

        }

        if(collision.gameObject.tag == "candlePickUpLightArea")
        {
            act2InteractionHandler.playerCatEyes.SetActive(true);
        }
    }
}
