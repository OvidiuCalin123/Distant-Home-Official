using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walk_npc_handler : MonoBehaviour
{
    public float speed;

    // Update is called once per frame
    void Update()
    {
        // Move the NPC to the right at a constant speed
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.tag == "EXIT_WALL")
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "EXIT_WALL")
        {
            Destroy(gameObject);
        }
    }
}