using System.Collections;
using UnityEngine;

public class dogBarkRange : MonoBehaviour
{
    public float moveSpeed = 2f;       // Speed of movement
    public float moveRange = 5f;       // Distance to move left and right
    private Vector3 startPosition;     // Starting position of the GameObject
    private bool movingRight = true;   // Direction flag (true = moving right, false = moving left)
    private Animator anim;             // Animator component reference
    private Vector3 localScale;        // Local scale for flipping the GameObject

    // Start is called before the first frame update
    void Start()
    {
        // Store the starting position of the GameObject
        startPosition = transform.position;

        // Get the Animator component attached to the GameObject
        anim = GetComponent<Animator>();

        // Store the initial local scale for flipping purposes
        localScale = transform.localScale;

        // Start the movement coroutine
        StartCoroutine(MoveLeftAndRight());
    }

    // Coroutine for moving left and right with pauses and animation changes
    IEnumerator MoveLeftAndRight()
    {
        while (true) // Keep the coroutine running indefinitely
        {
            // Set canRun to true to play running animation
            anim.SetBool("canRun", true);

            // Move right if movingRight is true, else move left
            if (movingRight)
            {
                // Flip the GameObject to face right
                localScale.x = -Mathf.Abs(localScale.x);
                transform.localScale = localScale;

                while (transform.position.x < startPosition.x + moveRange)
                {
                    // Move towards the right
                    transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                    yield return null; // Wait for the next frame
                }

                // Reached the right edge, change direction
                movingRight = false;

                // Set canRun to false and wait for 2 seconds
                anim.SetBool("canRun", false);
                anim.SetBool("canBark", true);
                yield return new WaitForSeconds(2f);

            }
            else
            {
                // Flip the GameObject to face left
                localScale.x = Mathf.Abs(localScale.x);
                transform.localScale = localScale;

                while (transform.position.x > startPosition.x - moveRange)
                {
                    // Move towards the left
                    transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                    yield return null; // Wait for the next frame
                }

                // Reached the left edge, change direction
                movingRight = true;

                // Set canRun to false and wait for 2 seconds
                anim.SetBool("canRun", false);
                anim.SetBool("canBark", true);
                yield return new WaitForSeconds(2f);
                
            }
        }
    }

}
