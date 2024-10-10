using UnityEngine;
using System.Linq;
using Pathfinding;
using Cinemachine;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    public Animator anim2d;
    public bool canWalk;
    public LayerMask mask;
    public Transform target;
    public IAstarAI[] ais;
    public float maxCameraSize = 7.4f;
    public float minCameraSize = 6.6f;
    public float cameraAdjustSpeed = 0.1f;

    private CinemachineVirtualCamera vcam;
    private bool isInMakeCameraBigZone = false;

    public GameObject mouseClickGroundEffect;
    private GameObject currentEffect;

    public GameObject clickedObject;

    public bool pickedInteractableDestination;

    public stealSystem stealSystem_ref;

    public GameObject stealUIPopup;

    public GameObject UI_StolenItem_text;

    public Transform canvasTransform;

    public GameObject[] availabelItemsItems;
    public List<GameObject> inventoryItems = new List<GameObject>();

    public SpriteRenderer playerRenderer;

    public float itemOffsetUI;

    public int playerCoins;

    public GameObject hamForTheDog;

    public Transform spawnPointHamForTheDog;

    public dogBarkRange DogQuest;
    
    void Start()
    {
        vcam = FindObjectOfType<CinemachineVirtualCamera>();
        anim2d = GetComponent<Animator>();
        ais = FindObjectsOfType<MonoBehaviour>().OfType<IAstarAI>().ToArray();
        pickedInteractableDestination = false;
        itemOffsetUI = 63.625f;
        playerRenderer = GetComponent<SpriteRenderer>();
    }

    public void setThrowHamToFalse()
    {
        DogQuest.ham = Instantiate(hamForTheDog, new Vector3(spawnPointHamForTheDog.position.x, spawnPointHamForTheDog.position.y), Quaternion.identity);
        anim2d.SetBool("canThrowHam", false);
        DogQuest.followHam = true;
        DogQuest.GetComponentInParent<Collider2D>().enabled = false;
        DogQuest.transform.parent.gameObject.transform.Find("guy").GetComponent<Collider2D>().enabled = true;
    }

    public void removeItemFromInventory(string itemTag)
    {
        foreach(GameObject item in inventoryItems)
        {
            if(item.tag == itemTag)
            {
                inventoryItems.Remove(item);
                Destroy(item);
                break;
            }
        }
        itemOffsetUI -= 100;
    }

    public void updateInventoryItemCountUp(GameObject inventoryItem)
    {
        TextMeshProUGUI amountText = inventoryItem.transform.Find("amount").GetComponent<TextMeshProUGUI>();

        string currentText = amountText.text;
        int currentAmount = int.Parse(currentText.Substring(1));

        int newAmount = currentAmount + 1;
        amountText.text = $"x{newAmount}";
    }

    public void updateInventoryItemCountDown(GameObject inventoryItem, int value)
    {
        TextMeshProUGUI amountText = inventoryItem.transform.Find("amount").GetComponent<TextMeshProUGUI>();

        string currentText = amountText.text;
        int currentAmount = int.Parse(currentText.Substring(1));

        int newAmount = currentAmount - value;
        amountText.text = $"x{newAmount}";
    }

    public void addNewInventoryItem(GameObject item)
    {
        GameObject newItem = Instantiate(item, canvasTransform);
        newItem.transform.SetAsFirstSibling();
        RectTransform rectTransform = newItem.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(itemOffsetUI, -65.5f);

        inventoryItems.Add(newItem);
        itemOffsetUI += 100;
    }

    private void handleInteractiveWorldEvent()
    {
        if (clickedObject != null)
        {

            if (clickedObject.tag == "thrash_bin")
            {
                clickedObject.GetComponent<Animator>().SetBool("canOpenTrash", true);
                clickedObject.GetComponent<Collider2D>().enabled = false;
            }
            else if (clickedObject.tag == "Coin")
            {
                Destroy(clickedObject);

                foreach (GameObject inventoryItem in inventoryItems)
                {
                    if(inventoryItem.tag == "CoinItem")
                    {
                        playerCoins += 1;
                        updateInventoryItemCountUp(inventoryItem);
                        return;
                    }
                }
                foreach (GameObject item in availabelItemsItems)
                {
                    if (item.tag == "CoinItem")
                    {
                        playerCoins = 1;
                        addNewInventoryItem(item);
                        return;
                    }
                }
                
            }
            else if (clickedObject.tag == "newspaper_guy")
            {
                clickedObject.transform.Find("Npc_Dialogue").gameObject.SetActive(true);
                clickedObject.transform.Find("Npc_Dialogue").gameObject.transform.Find("Dialogue_Background").gameObject.GetComponent<DialogueNavigation>().playerNearNPC = true;
                    
            }
            else if(clickedObject.tag == "shopVendor")
            {
                clickedObject.transform.Find("Npc_Dialogue").gameObject.SetActive(true);
                clickedObject.transform.Find("Npc_Dialogue").gameObject.transform.Find("Dialogue_Background").gameObject.GetComponent<DialogueNavigation>().playerNearNPC = true;
                clickedObject.GetComponent<shopVendor>().isTalking = true;

            }else if(clickedObject.tag == "TicketGuy")
            {
                clickedObject.transform.Find("Npc_Dialogue").gameObject.SetActive(true);
                clickedObject.transform.Find("Npc_Dialogue").gameObject.transform.Find("Dialogue_Background").gameObject.GetComponent<DialogueNavigation>().playerNearNPC = true;

            }else if(clickedObject.tag == "ShopVendorButton")
            {
                clickedObject.GetComponent<shopVendorButton>().shopUI.SetActive(true);
            }
        }
    }

    public GameObject SpawnStolenItemUI(float posX, float posY)
    {
        
        GameObject itemStolenUI_screen = Instantiate(UI_StolenItem_text, canvasTransform);

        
        RectTransform rectTransform = itemStolenUI_screen.GetComponent<RectTransform>();

        
        rectTransform.anchoredPosition = new Vector2(posX, posY);

        return itemStolenUI_screen;
    }

    public void endStealStateAndAddItemToInventory()
    {
        
        anim2d.SetBool("canSteal", false);
        GameObject itemStolen = SpawnStolenItemUI(-13, 70);
        itemStolen.GetComponent<TextMeshProUGUI>().text = stealSystem_ref.stealItemUI_text;
        //stealSystem_ref.gameObject.SetActive(false);

        foreach (GameObject inventoryItem in inventoryItems)
        {
            if (inventoryItem.tag == "CoinItem")
            {
                playerCoins += 1;
                updateInventoryItemCountUp(inventoryItem);
                return;
            }
        }
        foreach (GameObject item in availabelItemsItems)
        {
            if (item.tag == "CoinItem")
            {
                playerCoins = 1;
                addNewInventoryItem(item);
                return;
            }
        }
    }

    void Update()
    {
        if (stealSystem_ref)
        {
            if (Input.GetKey(KeyCode.E) && stealSystem_ref.canSteal)
            {

                if (stealSystem_ref)
                {
                    anim2d.SetBool("canSteal", true);

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
            }
        }
        
        if (ais[0].reachedDestination)
        {
            canWalk = false;

            if (pickedInteractableDestination)
            {
                pickedInteractableDestination = false;
                handleInteractiveWorldEvent();
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

    private void handleSpawnGroundClickEffect(Vector2 mousePos, RaycastHit2D? mostRelevantHit)
    {
        if (clickedObject != null)
        {
            if (clickedObject.layer == 6)
            {

                currentEffect = Instantiate(mouseClickGroundEffect, mostRelevantHit.Value.point, Quaternion.identity);

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

    private void handleNoMovementWhenClickingDialogue(IAstarAI ai, RaycastHit2D? mostRelevantHit, Vector2 mousePos)
    {

        if (clickedObject.layer != 6)
        {
            pickedInteractableDestination = true;
        }

        if (clickedObject.tag != "NextDialogue" && clickedObject.tag != "DogRange")
        {
            
            if (mostRelevantHit.HasValue)
            {
                var hitCollider = mostRelevantHit.Value.collider;
                canWalk = true;

                if (clickedObject.layer == 10)
                {
                    ai.destination = clickedObject.transform.Find("playerPosition").transform.position;
                }
                else
                {
                    ai.destination = mostRelevantHit.Value.point;
                }

                if (mousePos.x < transform.position.x)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    stealUIPopup.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    stealUIPopup.transform.rotation = Quaternion.Euler(0, 0, 0);
                }

                if (currentEffect != null)
                {
                    Destroy(currentEffect);
                }

                handleSpawnGroundClickEffect(mousePos, mostRelevantHit);
            }
        }
    }

    private void disableObjectStates()
    {
        if (clickedObject != null)
        {
            var dialogueNav = clickedObject.GetComponent<DialogueNavigation>();

            if (dialogueNav != null)
            {
                dialogueNav.playerNearNPC = false;
            }

            if (clickedObject.GetComponent<shopVendor>())
            {
                clickedObject.GetComponent<shopVendor>().isTalking = false;
            }
        }
    }

    void PointAndClick(IAstarAI ai)
    {
        if (!anim2d.GetBool("canSteal"))
        { 
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

                disableObjectStates();

                if (hit.collider)
                {
                    if (hit.collider.gameObject.layer != 7)
                    {
                        if (hit)
                        {
                            clickedObject = hit.collider.gameObject;
                        }

                        if (hit.collider != null)
                        {
                            if (hit.collider.tag != "NextDialogue")
                            {

                                handleNoMovementWhenClickingDialogue(ai, hit, mousePos);
                            }
                            else
                            {
                                clickedObject.GetComponent<DialogueNavigation>().dialogueState += 1;
                                clickedObject.GetComponent<DialogueNavigation>().playerNearNPC = true;
                            }
                        }
                    }
                }
            }
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MakeCameraBig")
        {
            isInMakeCameraBigZone = true;
        }

        if (collision.gameObject.tag == "FullShadow")
        {
            Color newColor;
            if (ColorUtility.TryParseHtmlString("#686868", out newColor)) // Convert hex to Color
            {
                playerRenderer.color = newColor; // Assign the parsed color
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MakeCameraBig")
        {
            isInMakeCameraBigZone = false;
        }

        if (collision.gameObject.tag == "FullShadow")
        {
            Color newColor;
            if (ColorUtility.TryParseHtmlString("#FFFFFF", out newColor)) // Convert hex to Color
            {
                playerRenderer.color = newColor; // Assign the parsed color
            }
        }
    }

    private void AdjustCameraSize()
    {
        float targetSize = isInMakeCameraBigZone ? maxCameraSize : minCameraSize;
        float currentSize = vcam.m_Lens.OrthographicSize;

        if (Mathf.Abs(currentSize - targetSize) > 0.01f)
        {
            vcam.m_Lens.OrthographicSize = Mathf.MoveTowards(currentSize, targetSize, cameraAdjustSpeed * Time.deltaTime);
        }
    }

}
