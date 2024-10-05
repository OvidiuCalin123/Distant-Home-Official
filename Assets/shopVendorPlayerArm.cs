using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class shopVendorPlayerArm : MonoBehaviour
{
    // Reference to the RectTransform of the UI element (your arm)
    private RectTransform rectTransform;

    public Animator anim;

    // Reference to the Canvas
    public Canvas canvas;

    // Define the restricted area (in Unity units)
    public float restrictedTopY = 300f; // Adjust this value as needed

    // To keep track of the ham gameObject
    public GameObject ham;

    // Store the previous parent of the ham
    private Transform previousParent;

    public GameObject shopVendorShop;

    public GameObject shopVendorButton;

    public PlayerMovement Player;

    public GameObject playerCoinsUIText;

    void Start()
    {
        // Get the RectTransform component of the UI element
        rectTransform = GetComponent<RectTransform>();

        // Hide the mouse cursor
        Cursor.visible = false;

        previousParent = ham.transform.parent;

        playerCoinsUIText.GetComponent<TextMeshProUGUI>().text = string.Format("x{0}", Player.playerCoins);
    }

    void Update()
    {
        // Handle animation for catching
        HandleAnimation();

        // Update the position of the arm
        UpdateArmPosition();

        // Check for catching interaction with ham
        CheckForHamInteraction();
    }

    private void HandleAnimation()
    {
        if (Input.GetMouseButton(0))
        {
            anim.SetBool("canCatch", true);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            anim.SetBool("canCatch", false);
        }

    }

    private void UpdateArmPosition()
    {
        // Get the mouse position in screen space
        Vector3 mousePos = Input.mousePosition;

        // Clamp the mouse position to the screen dimensions
        mousePos.x = Mathf.Clamp(mousePos.x, 0, Screen.width); // Clamp X position
        mousePos.y = Mathf.Clamp(mousePos.y, 0, Screen.height); // Clamp Y position

        // Convert the mouse position to local position in the canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), mousePos, null, out Vector2 localPoint);


        // Check if the Y position is below the restricted area
        if (localPoint.y > restrictedTopY)
        {
            localPoint.y = restrictedTopY; // Restrict Y position to max value
        }

        // Update the anchoredPosition of the UI element (the arm)
        rectTransform.anchoredPosition = new Vector2(localPoint.x, localPoint.y);
    }

    private void CheckForHamInteraction()
    {
        if (ham != null)
        {
            // Convert the ham and arm RectTransforms to screen space
            Rect hamRect = GetWorldRect(ham.GetComponent<RectTransform>(), canvas);
            Rect armRect = GetWorldRect(rectTransform, canvas);

            if (hamRect.yMin < 360)
            {
                Player.playerCoins -= 6;

                if (Player.playerCoins == 0)
                {

                    Player.removeItemFromInventory("CoinItem");
                    
                }
                else
                {
                    foreach(GameObject item in Player.inventoryItems)
                    {
                        if(item.tag == "CoinItem")
                        {
                            Player.updateInventoryItemCountDown(item, 6);
                            break;
                        }
                    }
                }

                foreach (GameObject item in Player.availabelItemsItems)
                {

                    if (item.tag == "ShopHam")
                    {
                        Player.addNewInventoryItem(item);
                        break;
                    }
                }
                
                shopVendorShop.SetActive(false);
                shopVendorButton.SetActive(false);

                Cursor.visible = true;
            }
            else if (armRect.Overlaps(hamRect))
            {
                if (anim.GetBool("canCatch") && Player.playerCoins >= 6)
                {
                    // Set ham's parent to the arm and make it the first child
                    ham.transform.SetParent(transform);
                    ham.transform.SetAsFirstSibling(); // Make ham the first child
                    ham.transform.localPosition = Vector3.zero; // Reset local position
                }
                else
                {
                    // Restore ham to its previous parent if canCatch is false
                    ham.transform.SetAsFirstSibling();
                    ham.transform.SetParent(previousParent);
                }
            }
            else
            {
                // Restore ham to its previous parent if it's not overlapping
                ham.transform.SetAsFirstSibling();
                ham.transform.SetParent(previousParent);
            }
        }
    }

    // Utility method to get the world space rect of a RectTransform
    private Rect GetWorldRect(RectTransform rt, Canvas canvas)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);

        // If the canvas is in screen space, convert corners to screen space
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay || (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera == null))
        {
            for (int i = 0; i < 4; i++)
            {
                corners[i] = RectTransformUtility.WorldToScreenPoint(null, corners[i]);
            }
        }

        float width = corners[2].x - corners[0].x;
        float height = corners[2].y - corners[0].y;
        return new Rect(corners[0], new Vector2(width, height));
    }
}
