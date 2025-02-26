using UnityEngine;

public class candlePlayerArm : MonoBehaviour
{
    private RectTransform rectTransform;

    public Canvas canvas; // Reference to the Canvas
    private Camera mainCamera; // Reference to the main camera
    public bool freezeCandle;
    public GameObject candleEvent;
    public PlayerMovement player;

    void Start()
    {
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        freezeCandle = false;
        rectTransform = GetComponent<RectTransform>();
        mainCamera = canvas.worldCamera; // Get the camera used by the canvas

        // Hide the mouse cursor
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && freezeCandle == false)
        {
            freezeCandle = true;
            Cursor.visible = true;
        }
        else if(Input.GetKeyDown(KeyCode.E) && freezeCandle == true)
        {
            freezeCandle = false;
            Cursor.visible = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            player.act2InteractionHandler.CandlePlayerStorage.SetActive(true);
            player.act2InteractionHandler.LightFromCandleLightEventZone.SetActive(true);
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            candleEvent.SetActive(false);
        }
        else
        {
            if(canvas.renderMode != RenderMode.ScreenSpaceCamera)
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
        }
        UpdateArmPosition();
    }

    private void UpdateArmPosition()
    {
        // Get the mouse position in screen space
        Vector3 mousePos = Input.mousePosition;

        // Convert the mouse position to local position in the canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            mousePos,
            mainCamera, // Pass the canvas camera here
            out Vector2 localPoint
        );

        // Update the anchoredPosition of the UI element (the arm)
        if (freezeCandle == false)
        {
            rectTransform.anchoredPosition = localPoint;
        }
    }
}
