using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class QuestPaperMoveUp : MonoBehaviour
{
    private bool isMovingUp = false;
    private bool isMoving = false; // To track if movement is in progress
    public float moveDuration = 1.0f;
    public float moveDistance = 200.0f;
    private Vector3 originalPosition;
    private Coroutine moveCoroutine; // To track the running coroutine

    public GameObject pauseMenu;
    public GameObject pauseButtons;
    public GameObject musicMenu;

    public AudioSource gameMusic;
    public Slider musicSlider;  

    public void handleMusicVolume()
    {

        gameMusic.volume = musicSlider.value;
    }

    public void MakePauseMenuAppear()
    {
        pauseMenu.SetActive(true);
    }

    public void MakePauseMenuDissapear()
    {
        pauseMenu.SetActive(false);
    }

    public void MakeMusicMenuAppear()
    {
        pauseButtons.SetActive(false);
        musicMenu.SetActive(true);
    }

    public void MakeMusicMenuDissapear()
    {
        musicMenu.SetActive(false);
        pauseButtons.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    void Start()
    {
        if (musicSlider && gameMusic != null)
        {
            musicSlider.value = gameMusic.volume; 
            musicSlider.onValueChanged.AddListener(delegate { handleMusicVolume(); });
        }
        
       
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
