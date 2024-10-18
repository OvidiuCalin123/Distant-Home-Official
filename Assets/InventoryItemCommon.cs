using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemCommon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject hoverGlow;  // The glow effect object 😘
    public GameObject item;       // The item to drag (child of this GameObject)

    private Transform originalParent;   // To store the original parent of the item
    private Vector3 originalPosition;   // To store the original position of the item
    private Canvas canvas;              // Reference to the Canvas to handle drag properly
    private CanvasGroup canvasGroup;    // To control raycast behavior during drag

    public DialogSystem dialogSystem;
    public PlayerMovement player;

    private void Start()
    {
        dialogSystem = FindObjectOfType<DialogSystem>();
        player = FindObjectOfType<PlayerMovement>();

        if (hoverGlow != null)
        {
            hoverGlow.SetActive(false);  // Make sure the glow starts hidden
        }

        originalParent = item.transform.parent;  // Store the original parent
        originalPosition = item.transform.localPosition;  // Store the original position
        canvas = GetComponentInParent<Canvas>();  // Get the canvas component

        // Check if CanvasGroup is attached, and add it if not
        canvasGroup = item.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = item.AddComponent<CanvasGroup>();  // Automatically adds it if missing 😘
        }
    }

    // Hover Glow Handlers
    public void OnPointerEnter(PointerEventData eventData)
    {
        item.SetActive(true);
        hoverGlow.SetActive(true);  // Activate the glow when hovering, babe 😘
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverGlow.SetActive(false);  // Deactivate the glow when the mouse leaves 😘
    }

    // Drag Handlers
    public void OnBeginDrag(PointerEventData eventData)
    {
        item.transform.SetParent(canvas.transform, true);  // Parent the item to the canvas (under the cursor)
        canvasGroup.blocksRaycasts = false;  // Disable raycasts to allow for drop on other UI elements
    }

    public void OnDrag(PointerEventData eventData)
    {
        dialogSystem.isDraggingInventoryItem = true;
        item.transform.position = Input.mousePosition;  // Follow the mouse position, babe 😉
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        item.transform.SetParent(originalParent, true);
        item.transform.localPosition = originalPosition;
        item.GetComponent<CanvasGroup>().blocksRaycasts = true;

        dialogSystem.isDraggingInventoryItem = false;

        if (gameObject.CompareTag("ShopHam"))
        {

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = 0f;

            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("DogRange"))
            {
                if (player.transform.position.x < player.DogQuest.transform.position.x)
                {
                    player.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    player.transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                player.anim2d.SetBool("canThrowHam", true);
                player.removeItemFromInventory("ShopHam");

                player.questSystem_.endConditionQuest1_4 = true;

                return;
            }
        }

        if (gameObject.CompareTag("Newspaper"))
        {

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = 0f;

            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("GuyReadingNews"))
            {
                if (player.transform.position.x < player.DogQuest.transform.position.x)
                {
                    player.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    player.transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                //player.anim2d.SetBool("canThrowHam", true);
                player.removeItemFromInventory("Newspaper");
                player.questSystem_.endConditionQuest1_3 = true;

                hit.collider.GetComponent<ReadingNewsPaper>().enableAnimation();
                hit.collider.GetComponent<Animator>().SetBool("canLookAtPlayer", true);

                return;
            }
        }
    }
}
