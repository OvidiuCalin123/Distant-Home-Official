using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogSystem : MonoBehaviour
{
    private string dialogName;
    public GameObject dialog;

    public bool canSetDialogWindow;

    public PlayerMovement player;

    public Texture2D defaultCursorTexture;
    public Texture2D customCursorTexture;
    public Texture2D clickCursorTexture; // New cursor texture for clicking
    private bool isHovering = false;

    // This method will be called when the sprite is clicked
    public void OnSpriteClick(GameObject clickedObject)
    {
        Debug.Log(clickedObject);
        if (clickedObject.CompareTag("TicketGuy"))
        {
            Debug.Log("Nice");
            Debug.Log(player.ais[0].reachedDestination);
            SetDialogName(clickedObject.tag);
        }
    }

    public void exitDialog()
    {
        canSetDialogWindow = false;
        dialog.SetActive(false);
    }

    public void SetDialogName(string name)
    {
        canSetDialogWindow = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetCursor(defaultCursorTexture);
    }

    // Update is called once per frame
    void Update()
    {
        DetectSpriteClick();
        DetectMouseHover();

        if (canSetDialogWindow && player.ais[0].reachedDestination)
        {
            canSetDialogWindow = false;
            dialogName = name;
            dialog.SetActive(true);
        }
    }

    void DetectSpriteClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit3 = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit3.collider != null)
            {
                if (!hit3.collider.CompareTag("TicketGuy"))
                {
                    SetCursor(clickCursorTexture); // Change to click cursor only if not clicking on TicketGuy
                }
                OnSpriteClick(hit3.collider.gameObject);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            SetCursor(isHovering ? customCursorTexture : defaultCursorTexture); // Revert to appropriate cursor
        }
    }

    void DetectMouseHover()
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("TicketGuy"))
        {
            if (!isHovering)
            {
                isHovering = true;
                SetCursor(customCursorTexture);
            }
        }
        else
        {
            if (isHovering)
            {
                isHovering = false;
                SetCursor(defaultCursorTexture);
            }
        }
    }

    void SetCursor(Texture2D cursorTexture)
    {
        // Scale the cursor size by 1.5 times
        int newWidth = (int)(cursorTexture.width * 0.3f);
        int newHeight = (int)(cursorTexture.height * 0.3f);

        // Create a new Texture2D with the appropriate format
        Texture2D resizedCursorTexture = new Texture2D(newWidth, newHeight, TextureFormat.RGBA32, false);

        // Resize the texture manually
        for (int y = 0; y < newHeight; y++)
        {
            for (int x = 0; x < newWidth; x++)
            {
                float u = x / (float)newWidth;
                float v = y / (float)newHeight;
                resizedCursorTexture.SetPixel(x, y, cursorTexture.GetPixelBilinear(u, v));
            }
        }

        resizedCursorTexture.Apply();

        // Ensure the texture is readable, has alpha transparency enabled, and no mip chain
        resizedCursorTexture.alphaIsTransparency = true;

        Vector2 hotspot = new Vector2(newWidth / 2, newHeight / 2);
        Cursor.SetCursor(resizedCursorTexture, hotspot, CursorMode.ForceSoftware);
    }
}
