using UnityEngine;
using System.Collections.Generic; // For using lists

public class PickLockingSytem : MonoBehaviour
{
    public RectTransform upArrow;    // Up arrow prefab
    public RectTransform downArrow;  // Down arrow prefab
    public RectTransform leftArrow;  // Left arrow prefab
    public RectTransform rightArrow; // Right arrow prefab

    public RectTransform highlightZone; // Highlight area for timing

    public float arrowSpeedEasy = 150f;  // Easy stage arrow speed
    public float arrowSpeedMedium = 300f; // Medium stage arrow speed
    public float arrowSpeedHard = 450f;  // Hard stage arrow speed

    public int maxTries = 3; // Maximum attempts before resetting
    public float spawnIntervalEasy = 1.5f;  // Easy stage spawn interval
    public float spawnIntervalMedium = 1f;  // Medium stage spawn interval
    public float spawnIntervalHard = 0.5f;  // Hard stage spawn interval

    private float nextSpawnTime;
    private List<(RectTransform arrow, KeyCode key)> activeArrows = new List<(RectTransform, KeyCode)>(); // Active arrows
    private int triesLeft;
    private float timeElapsed; // Timer to track the game duration
    private int currentStage;  // 0 = Easy, 1 = Medium, 2 = Hard
    private float arrowSpeed;  // Arrow speed that will change based on stage
    private float spawnInterval;  // Spawn interval that will change based on stage
    public GameObject Canvas;

    public Animator anim;
    public PlayerMovement player;

    void Start()
    {
        triesLeft = maxTries;
        timeElapsed = 0f;
        currentStage = 0;  // Start in Easy stage
        spawnInterval = spawnIntervalEasy;
        arrowSpeed = arrowSpeedEasy;

        nextSpawnTime = Time.time + spawnInterval;
        anim = GetComponent<Animator>();
    }

    public void returnToIdle()
    {
        anim.SetBool("hasMissed", false);
    }

    void Update()
    {
        HandleArrowMovement();

        // Track the time passed to change stages
        timeElapsed += Time.deltaTime;

        // Switch stages based on the elapsed time
        if (timeElapsed >= 10f && currentStage == 0)  // After 10 seconds, switch to Medium
        {
            currentStage = 1;
            spawnInterval = spawnIntervalMedium;
            arrowSpeed = arrowSpeedMedium;
        }
        else if (timeElapsed >= 20f && currentStage == 1)  // After 20 seconds, switch to Hard
        {
            currentStage = 2;
            spawnInterval = spawnIntervalHard;
            arrowSpeed = arrowSpeedHard;
        }

        // Reset the game after 30 seconds
        if (timeElapsed >= 30f)
        {
            Debug.Log("Game Over! Time's up.");

            player.act2InteractionHandler.lockedDoorMiddle.enabled = true;
            player.act2InteractionHandler.pickLockingDoorMiddle.enabled = false;
            player.soundManager.playUnlockedDoor();
            ResetMechanism();
            gameObject.SetActive(false);
            
            return;
        }

        // Spawn new arrows at the current interval
        if (Time.time >= nextSpawnTime)
        {
            SpawnNextArrow();
            nextSpawnTime = Time.time + spawnInterval;
        }

        HandleInput();
    }

    void HandleArrowMovement()
    {
        for (int i = activeArrows.Count - 1; i >= 0; i--)
        {
            var (arrow, key) = activeArrows[i];

            // Move the arrow left towards the highlight zone with the current speed
            arrow.anchoredPosition -= new Vector2(arrowSpeed * Time.deltaTime, 0);

            // Check if the arrow has passed the highlight zone
            if (arrow.anchoredPosition.x < highlightZone.anchoredPosition.x - (highlightZone.rect.width / 2))
            {
                player.soundManager.playErrorPickLockArrows();
                Debug.Log("Missed!");
                anim.SetBool("hasMissed", true);
                Destroy(arrow.gameObject);
                activeArrows.RemoveAt(i);
                HandleMiss();
            }
        }
    }

    void HandleInput()
    {
        for (int i = activeArrows.Count - 1; i >= 0; i--)
        {
            var (arrow, key) = activeArrows[i];

            // If the correct key is pressed and the arrow is in the highlight zone
            if (Input.GetKeyDown(key) && IsArrowInHighlightZone(arrow))
            {
                player.soundManager.playArrowsPickLockHit();
                Debug.Log($"Success! Arrow {key} hit.");
                Destroy(arrow.gameObject);
                activeArrows.RemoveAt(i);
                return; // Only destroy the first valid arrow in the highlight zone
            }
        }
    }

    void SpawnNextArrow()
    {
        if (triesLeft <= 0)
        {
            Debug.Log("Failed lockpick!");
            ResetMechanism();
            return;
        }

        // Randomly choose the next arrow and key
        RectTransform newArrow = null;
        KeyCode newKey = KeyCode.None;
        int arrowIndex = Random.Range(0, 4); // 0 = Up, 1 = Down, 2 = Left, 3 = Right
        switch (arrowIndex)
        {
            case 0:
                newArrow = Instantiate(upArrow, transform);
                newKey = KeyCode.UpArrow;
                break;
            case 1:
                newArrow = Instantiate(downArrow, transform);
                newKey = KeyCode.DownArrow;
                break;
            case 2:
                newArrow = Instantiate(leftArrow, transform);
                newKey = KeyCode.LeftArrow;
                break;
            case 3:
                newArrow = Instantiate(rightArrow, transform);
                newKey = KeyCode.RightArrow;
                break;
        }

        // Position the arrow to the right of the highlight zone dynamically
        float startX = highlightZone.anchoredPosition.x + (highlightZone.rect.width / 2) + 800f; // Offset to the right of the highlight zone
        newArrow.anchoredPosition = new Vector2(startX, highlightZone.anchoredPosition.y);

        // Add the new arrow to the active list
        activeArrows.Add((newArrow, newKey));
    }

    bool IsArrowInHighlightZone(RectTransform arrow)
    {
        // Check if the arrow is inside the highlight zone
        return RectTransformUtility.RectangleContainsScreenPoint(highlightZone, arrow.position);
    }

    void HandleMiss()
    {
        triesLeft--;
        if (triesLeft <= 0)
        {
            Debug.Log("Failed lockpick!");
            ResetMechanism();
        }
    }

    void ResetMechanism()
    {
        Debug.Log("Resetting...");
        triesLeft = maxTries;
        currentStage = 0;

        triesLeft = maxTries;
        timeElapsed = 0f;
        spawnInterval = spawnIntervalEasy;
        arrowSpeed = arrowSpeedEasy;

        nextSpawnTime = Time.time + spawnInterval;

        // Destroy all active arrows
        foreach (var (arrow, _) in activeArrows)
        {
            Destroy(arrow.gameObject);
        }
        activeArrows.Clear();
    }
}
