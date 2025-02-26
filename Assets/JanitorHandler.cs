using UnityEngine;
using System.Collections;

public class JanitorHandler : MonoBehaviour
{
    public GameObject coalTrail; // Reference to the coal trail object
    public float speed = 2f; // Movement speed
    public Animator anim;

    private Vector3 initialPosition; // Store the janitor's initial position
    private bool movingToCoal = false; // Flag to indicate if moving towards the coal trail
    private bool returningToStart = false; // Flag to indicate if returning to the initial position
    private bool stopped = false; // Flag to stop movement when colliding with water trail
    private Coroutine currentMoveCoroutine; // Track the currently running coroutine
    public Collider2D coalTrailPlayerColider;

    public SoundManager sound;

    void Start()
    {
        anim = GetComponent<Animator>();
        initialPosition = transform.position; // Store the starting position
    }

    void Update()
    {
        if (stopped) return; // Stop all movement when colliding with a water trail

        if (coalTrail.activeSelf && !movingToCoal)
        {
            // Interrupt current coroutine if the coal trail becomes active again
            if (currentMoveCoroutine != null)
            {
                StopCoroutine(currentMoveCoroutine);
            }

            currentMoveCoroutine = StartCoroutine(MoveToPosition(coalTrail.transform.position, true));
        }
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition, bool isCoal)
    {
        // Handle flags to track movement state
        movingToCoal = isCoal;
        returningToStart = !isCoal;

        anim.SetBool("canWalk", true);

        // Flip the janitor to face the correct direction
        FlipTowards(targetPosition);

        // Move towards the destination
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            // Exit early if coalTrail becomes active and we're not already moving to it
            if (coalTrail.activeSelf && !isCoal)
            {
                anim.SetBool("canWalk", false);
                yield break; // Stop this coroutine
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }

        // Arrived at the destination
        transform.position = targetPosition;
        anim.SetBool("canWalk", false);

        if (isCoal)
        {
            yield return new WaitForSeconds(3f); // Wait for 3 seconds at the coal trail
            coalTrail.SetActive(false); // Deactivate the coal trail
            coalTrailPlayerColider.enabled = true;
            currentMoveCoroutine = StartCoroutine(MoveToPosition(initialPosition, false)); // Start moving back
        }

        // Reset movement flags
        movingToCoal = false;
        returningToStart = false;
    }

    private void FlipTowards(Vector3 targetPosition)
    {
        // Calculate the direction to the target position
        float direction = targetPosition.x - transform.position.x;

        if ((direction > 0 && transform.localScale.x < 0) || (direction < 0 && transform.localScale.x > 0))
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1; // Invert the X scale to flip the sprite
            transform.localScale = scale;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "waterTrail")
        {
            anim.SetBool("canFall", true);
            stopped = true;

            // Stop any ongoing movement
            if (currentMoveCoroutine != null)
            {
                StopCoroutine(currentMoveCoroutine);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "waterTrail")
        {
            anim.SetBool("canFall", true);
            stopped = true;

            // Stop any ongoing movement
            if (currentMoveCoroutine != null)
            {
                StopCoroutine(currentMoveCoroutine);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "waterTrail")
        {
            sound.playSlipNPCClick();
            anim.SetBool("canFall", true);
            stopped = true;

            // Stop any ongoing movement
            if (currentMoveCoroutine != null)
            {
                StopCoroutine(currentMoveCoroutine);
            }
        }
    }
}
