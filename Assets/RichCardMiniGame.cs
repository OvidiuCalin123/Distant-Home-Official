using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class RichCardMiniGame : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Vector2 originalPosition;
    public static List<GameObject> stackedGlasses = new List<GameObject>(); // Shared list for stacked glasses
    public static GameObject currentlyDragging = null; // Track the active dragging object
    public bool isCoin = false; // Is this object the coin?
    private bool isStacked = false; // Prevent multiple stacking
    private static List<RichCardMiniGame> allObjects = new List<RichCardMiniGame>(); // Track all objects
    public PlayerMovement player;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;

        // Add this object to tracking
        allObjects.Add(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentlyDragging != null && currentlyDragging != gameObject)
        {
            Debug.Log("❌ You can't drag multiple objects at once!");
            return;
        }

        currentlyDragging = gameObject; // Set current dragging object
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentlyDragging != gameObject) return; // Prevent dragging multiple objects
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        currentlyDragging = null; // Allow other objects to be dragged after releasing

        if (!isCoin)
        {
            CheckGlassStacking();
        }
        else
        {
            StartCoroutine(DropCoinWithGravity());
        }
    }

    private void CheckGlassStacking()
    {
        if (isStacked) return; // Prevent stacking twice

        foreach (GameObject glass in stackedGlasses)
        {
            if (glass != gameObject && IsOverlapping(glass))
            {
                StackGlass(glass);
                return;
            }
        }
        stackedGlasses.Add(gameObject);
        isStacked = true;
    }

    private void StackGlass(GameObject targetGlass)
    {
        rectTransform.SetParent(targetGlass.transform);
        rectTransform.anchoredPosition = Vector2.zero;
        stackedGlasses.Add(gameObject);
        isStacked = true;
    }

    private IEnumerator DropCoinWithGravity()
    {
        if (stackedGlasses.Count == 3) // Only drop if all 3 glasses are stacked
        {
            GameObject lastGlass = stackedGlasses[stackedGlasses.Count - 1];

            if (IsOverlapping(lastGlass))
            {
                rectTransform.SetParent(lastGlass.transform);
                Vector2 targetPosition = Vector2.zero;

                // 📉 Simulating Gravity (Lerp)
                float duration = 0.5f;
                float elapsedTime = 0f;
                Vector2 startPosition = rectTransform.anchoredPosition;

                while (elapsedTime < duration)
                {
                    rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / duration);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                rectTransform.anchoredPosition = targetPosition;
                Debug.Log("🎉 YOU WIN! Coin dropped inside stacked glasses!");
                player.soundManager.playGenericItemRub();
                player.playerInventory.addNewInventoryItem(player.playerInventory.getAvailableItemByTag("richRingItem"));
                player.act2InteractionHandler.CardMiniGameRich.SetActive(false);
                
            }
        }
        else
        {
            //ResetPosition(); // Reset position if conditions aren't met
        }
    }

    private bool IsOverlapping(GameObject otherObject)
    {
        RectTransform otherRect = otherObject.GetComponent<RectTransform>();
        return Vector2.Distance(rectTransform.position, otherRect.position) < 50f;
    }

    // 🔄 RESET FUNCTION
    public static void ResetGame()
    {
        foreach (var obj in allObjects)
        {
            obj.ResetPosition(); // Reset position
            obj.isStacked = false;
        }

        stackedGlasses.Clear(); // Clear stacking list
        currentlyDragging = null;

        Debug.Log("🔄 Game Reset!");
    }

    private void ResetPosition()
    {
        rectTransform.anchoredPosition = originalPosition;
    }
}
