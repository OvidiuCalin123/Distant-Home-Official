using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shopVendor : MonoBehaviour
{
    public float moveSpeed = 2f; // Speed of movement
    public float moveDistance = 5f; // Distance to move left and right from the starting position

    private bool movingRight = true; // Track movement direction
    private bool isWaiting = false; // Check if currently waiting at an edge

    private float leftBoundary; // Calculated left boundary
    private float rightBoundary; // Calculated right boundary

    private Animator animator; // Reference to the Animator

    public bool isTalking;
    
    public GameObject dialogue;
    public Transform player; // Reference to the player's Transform

    void Start()
    {
        isTalking = false;
        animator = GetComponent<Animator>(); // Get the Animator component

        // Calculate left and right boundaries based on the initial position
        leftBoundary = transform.position.x - moveDistance;
        rightBoundary = transform.position.x + moveDistance;
    }

    void Update()
    {
        if (!isWaiting && !isTalking)
        {
            Move();
        }
        else if (isTalking)
        {
            animator.SetBool("canWalk", false);
            // Rotate to face the player when isTalking is true
            RotateTowardsPlayer();
        }
    }

    void Move()
    {
        if(movingRight && transform.rotation.eulerAngles.y == 180)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            dialogue.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        }
        else if(!movingRight && transform.rotation.eulerAngles.y == 0)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            dialogue.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        // Move towards the right or left boundary depending on direction
        if (movingRight)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(rightBoundary, transform.position.y), moveSpeed * Time.deltaTime);
            if (transform.position.x >= rightBoundary)
            {
                StartCoroutine(WaitAtEdge());
            }
            else
            {
                // Set the animator to walking state
                animator.SetBool("canWalk", true);
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(leftBoundary, transform.position.y), moveSpeed * Time.deltaTime);
            if (transform.position.x <= leftBoundary)
            {
                StartCoroutine(WaitAtEdge());
            }
            else
            {
                // Set the animator to walking state
                animator.SetBool("canWalk", true);
            }
        }
        
    }

    IEnumerator WaitAtEdge()
    {
        isWaiting = true;

        // Set the animator to idle state while waiting
        animator.SetBool("canWalk", false);

        // Wait for 4 seconds
        yield return new WaitForSeconds(4f);

        // Flip the direction and continue moving
        Flip();

        // Stop waiting and start moving again
        isWaiting = false;
    }

    void Flip()
    {
        // Reverse the direction of movement
        movingRight = !movingRight;

        // Flip the sprite by rotating 180 degrees around the Y-axis using Quaternion
        if (movingRight)
        {
            // Face the right direction (no rotation)
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            dialogue.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            // Face the left direction (rotate 180 degrees around the Y-axis)
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            dialogue.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    void RotateTowardsPlayer()
    {
        // Calculate the direction vector from shopVendor to player
        Vector3 direction = player.position - transform.position;

        // Normalize the direction to get a unit vector
        direction.Normalize();

        // Check if the player is on the left or right side of the vendor
        if (direction.x > 0)
        {
            // If player is to the right, face right (default direction)
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            // If player is to the left, rotate to face left
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }

        // Ensure the dialogue box doesn't rotate with the NPC
        dialogue.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
}
