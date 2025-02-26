using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainMiniGameOutside : MonoBehaviour
{
    public Image redArea;
    public Image greenAreaBig;
    public Image greenAreaMedium;
    public Image greenAreaSmall;
    public Image MovingPart;

    private RectTransform redAreaRect;
    private RectTransform movingPartRect;

    private float movingPartSpeed = 700f;
    private bool movingPartRight = true;

    private RectTransform activeGreenArea;
    private float greenAreaSpeed = 200f;
    private bool greenAreaRight = true;

    public PlayerMovement player;

    void Start()
    {
        redAreaRect = redArea.GetComponent<RectTransform>();
        movingPartRect = MovingPart.GetComponent<RectTransform>();

        // Randomly choose and place a green area
        PlaceRandomGreenArea();

        // Initialize the MovingPart position within the red area
        float initialX = Random.Range(redAreaRect.rect.xMin, redAreaRect.rect.xMax);
        movingPartRect.anchoredPosition = new Vector2(initialX, movingPartRect.anchoredPosition.y);
    }

    void Update()
    {
        MovePart();
        MoveGreenArea();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!CheckGreenAreaOverlap()) // Check if not overlapping green area
            {
                CheckRedAreaClick(); // Only check red area if not on green
            }
        }
    }

    private void MovePart()
    {
        float delta = movingPartSpeed * Time.deltaTime;
        Vector2 position = movingPartRect.anchoredPosition;

        if (movingPartRight)
        {
            position.x += delta;
            if (position.x > redAreaRect.rect.xMax)
            {
                position.x = redAreaRect.rect.xMax;
                movingPartRight = false;
            }
        }
        else
        {
            position.x -= delta;
            if (position.x < redAreaRect.rect.xMin)
            {
                position.x = redAreaRect.rect.xMin;
                movingPartRight = true;
            }
        }

        movingPartRect.anchoredPosition = position;
    }

    private void MoveGreenArea()
    {
        if (activeGreenArea == null)
            return;

        float delta = greenAreaSpeed * Time.deltaTime;
        Vector2 position = activeGreenArea.anchoredPosition;

        if (greenAreaRight)
        {
            position.x += delta;
            if (position.x > redAreaRect.rect.xMax)
            {
                position.x = redAreaRect.rect.xMax;
                greenAreaRight = false;
            }
        }
        else
        {
            position.x -= delta;
            if (position.x < redAreaRect.rect.xMin)
            {
                position.x = redAreaRect.rect.xMin;
                greenAreaRight = true;
            }
        }

        activeGreenArea.anchoredPosition = position;
    }

    private bool CheckGreenAreaOverlap()
    {
        if (activeGreenArea == null)
            return false;

        Rect movingPartBounds = GetWorldRect(movingPartRect);
        Rect greenAreaBounds = GetWorldRect(activeGreenArea);

        if (movingPartBounds.Overlaps(greenAreaBounds))
        {
            if (player.act2InteractionHandler.miniGameOutsidePoint1Obj.enabled)
            {
                player.act2InteractionHandler.miniGameOutsidePoint1Obj.enabled = false;
            }
            else if (!player.act2InteractionHandler.miniGameOutsidePoint1Obj.enabled)
            {
                player.act2InteractionHandler.miniGameOutsidePoint2Obj.enabled = false;
            }

            gameObject.SetActive(false);
            Debug.Log("Moving part is overlapping the green area!");
            return true; // Overlapping green area
        }

        return false; // Not overlapping green area
    }

    private void CheckRedAreaClick()
    {
        Rect redAreaBounds = GetWorldRect(redAreaRect);
        Rect movingPartBounds = GetWorldRect(movingPartRect);

        if (movingPartBounds.Overlaps(redAreaBounds))
        {
            //player.transform.position += Vector3.left * 5;
            Debug.Log("Red area clicked!");
        }
    }

    private void PlaceRandomGreenArea()
    {
        // Deactivate all green areas initially
        greenAreaBig.gameObject.SetActive(false);
        greenAreaMedium.gameObject.SetActive(false);
        greenAreaSmall.gameObject.SetActive(false);

        // Randomly choose one green area
        int choice = Random.Range(0, 3);
        if (choice == 0)
        {
            activeGreenArea = greenAreaBig.GetComponent<RectTransform>();
            greenAreaBig.gameObject.SetActive(true);
        }
        else if (choice == 1)
        {
            activeGreenArea = greenAreaMedium.GetComponent<RectTransform>();
            greenAreaMedium.gameObject.SetActive(true);
        }
        else
        {
            activeGreenArea = greenAreaSmall.GetComponent<RectTransform>();
            greenAreaSmall.gameObject.SetActive(true);
        }

        // Place the green area at a random position within the red area
        float randomX = Random.Range(redAreaRect.rect.xMin, redAreaRect.rect.xMax);
        Vector2 greenPosition = activeGreenArea.anchoredPosition;
        greenPosition.x = randomX;
        activeGreenArea.anchoredPosition = greenPosition;
    }

    private Rect GetWorldRect(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        return new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);
    }
}
