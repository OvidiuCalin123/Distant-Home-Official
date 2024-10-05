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
    public Texture2D clickCursorTexture;
    private bool isHovering = false;

    enum objectType
    {
        interactable = 10
    }

    private void changeCursorTextureIfInteractable(Collider2D hitCollider)
    {

        if (hitCollider.gameObject.layer == (int)objectType.interactable)
        {
            SetCursor(customCursorTexture);
        }
    }

    void Start()
    {
        SetCursor(defaultCursorTexture);
    }

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
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D? hit = Physics2D.Raycast(mousePos, Vector2.zero);

            SetCursor(clickCursorTexture);

            if (hit.HasValue)
            {
                var hitCollider = hit.Value.collider;
                if (hitCollider != null)
                {
                    changeCursorTextureIfInteractable(hitCollider);
                }
                
            }
            
            canSetDialogWindow = false;

        }
        else if (Input.GetMouseButtonUp(0))
        {
            SetCursor(isHovering ? customCursorTexture : defaultCursorTexture);
        }
    }

    void DetectMouseHover()
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(worldPoint, Vector2.zero);

        bool foundTaggedObject = false;

        foreach (var hit in hits)
        {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.layer == (int)objectType.interactable)
                {
                    foundTaggedObject = true;
                    if (!isHovering)
                    {
                        isHovering = true;
                        SetCursor(customCursorTexture);
                    }
                    break;
                }
            }
        }

        if (!foundTaggedObject && isHovering)
        {
            isHovering = false;
            SetCursor(defaultCursorTexture);
        }
    }

    void SetCursor(Texture2D cursorTexture)
    {
        int newWidth = (int)(cursorTexture.width * 0.3f);
        int newHeight = (int)(cursorTexture.height * 0.3f);

        Texture2D resizedCursorTexture = new Texture2D(newWidth, newHeight, TextureFormat.RGBA32, false);

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

        resizedCursorTexture.alphaIsTransparency = true;

        Vector2 hotspot = new Vector2(newWidth / 2, newHeight / 2);
        Cursor.SetCursor(resizedCursorTexture, hotspot, CursorMode.ForceSoftware);
    }
}
