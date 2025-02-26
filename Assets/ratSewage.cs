using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ratSewage : MonoBehaviour
{
    public GameObject bubble;
    public PlayerMovement player;
    public Animator anim;
    public float moveDuration = 1.8f; // How long the rat will move after the bubble is active
    public float moveSpeed = 2f; // Speed of the rat's movement when it moves

    private bool isMoving = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.makeRatTalk && bubble != null)
        {
            bubble.SetActive(true);
            Destroy(bubble, 3);
        }
        if (player.makeRatTalk && bubble == null)
        {
            // If the rat is not already moving
            if (!isMoving)
            {
                // Flip the rat horizontally
                FlipRat();

                // Trigger the "canMove" animation state
                anim.SetBool("canMove", true);

                // Start moving the rat after a short delay
                StartCoroutine(MoveRatAndDestroy());
            }
        }
    }

    void FlipRat()
    {
        // Flip the rat sprite horizontally depending on which direction it's facing
        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Sign(player.transform.position.x - transform.position.x) * Mathf.Abs(localScale.x);
        transform.localScale = localScale;
    }

    IEnumerator MoveRatAndDestroy()
    {
        isMoving = true;

        // Move the rat in a certain direction for a set duration (moveDuration)
        float startTime = Time.time;
        while (Time.time - startTime < moveDuration)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime); // Moving the rat to the right
            yield return null;
        }

        // Destroy the rat or bubble after movement is complete
        player.MoveCamera(0.38f, 0.15f, true);
        player.cinematic.GetComponent<Cinematic>().endCinematic();
        Destroy(gameObject); // Destroy the rat object after moving
    }
}
