using System.Collections;
using UnityEngine;

public class RatPickHobo : MonoBehaviour
{
    // Target position for the GameObject to move to
    public Transform targetPosition;
    // Speed of the movement
    public float moveSpeed = 3f;

    // Original position of the GameObject
    private Vector2 originalPosition;
    public GameObject hobbo;
    public GameObject sewageTicket;

    public Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        // Save the original position of the GameObject
        originalPosition = transform.position;
        // Start the movement coroutine
        StartCoroutine(MoveToTargetAndBack());
    }

    IEnumerator MoveToTargetAndBack()
    {
        // Move to the target position
        yield return StartCoroutine(MoveToPosition(targetPosition.position));

        anim.SetBool("canWalk", false);
        // Wait for 1 second
        yield return new WaitForSeconds(0.5f);

        if(sewageTicket != null)
        {
            sewageTicket.SetActive(true);
        }
        

        anim.SetBool("canWalk", true);

        hobbo.transform.parent = gameObject.transform;
        // Move back to the original position
        yield return StartCoroutine(MoveToPosition(originalPosition));
    }

    IEnumerator MoveToPosition(Vector2 destination)
    {
        while ((Vector2)transform.position != destination)
        {
            anim.SetBool("canWalk", true);
            // Smoothly move towards the destination
            transform.position = Vector2.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }
    }
}
