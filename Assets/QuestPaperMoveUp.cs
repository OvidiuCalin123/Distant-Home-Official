using System.Collections;
using UnityEngine;

public class QuestPaperMoveUp : MonoBehaviour
{
    private bool isMovingUp = false;
    private bool isMoving = false; // To track if movement is in progress
    public float moveDuration = 1.0f;
    public float moveDistance = 200.0f;
    private Vector3 originalPosition;
    private Coroutine moveCoroutine; // To track the running coroutine

    public GameObject pauseMenu;

    public void MakePauseMenuAppear()
    {
        pauseMenu.SetActive(true);
    }

    public void MakePauseMenuDissapear()
    {
        pauseMenu.SetActive(false);
    }

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    public void MoveQuestPaper()
    {
        // If already moving, stop the current coroutine
        if (isMoving)
        {
            StopCoroutine(moveCoroutine);
            isMoving = false;
        }

        // Toggle the direction
        isMovingUp = !isMovingUp;

        // Determine the target position based on the direction
        Vector3 targetPosition = isMovingUp ? originalPosition + new Vector3(0, moveDistance, 0) : originalPosition;

        // Start the new movement coroutine
        moveCoroutine = StartCoroutine(MoveToPosition(targetPosition));
    }

    IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        isMoving = true;
        Vector3 startPosition = transform.localPosition;
        float elapsedTime = 0;

        while (elapsedTime < moveDuration)
        {
            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position is set correctly
        transform.localPosition = targetPosition;
        isMoving = false; // Mark movement as complete
    }
}
