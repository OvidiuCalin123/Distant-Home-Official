using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RichWomanDoor : MonoBehaviour
{
    public float walkSpeed = 2f; // Speed of walking
    public PlayerMovement player;

    void Update()
    {
        if (gameObject.GetComponent<Animator>().GetBool("canWalk"))
        {
            // Move forward (assumes right is forward)
            transform.position += Vector3.left * walkSpeed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "wineSplash")
        {
            if (gameObject.GetComponent<Animator>().GetBool("canWalk"))
            {
                StartCoroutine(player.CallMoveCameraAfterDelay(2f));
                gameObject.GetComponent<Animator>().SetBool("canFall", true);

            }
        }
    }
}
