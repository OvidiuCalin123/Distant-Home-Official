using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WineGuyRich : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;
    private Vector3 target;
    private bool facingRight = true;

    void Start()
    {
        target = pointB.position; // Start moving towards pointB
    }

    void Update()
    {
        if (!gameObject.GetComponent<Animator>().GetBool("canWheel"))
        {
            Move();
        }
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            // Switch target between pointA and pointB
            target = (target == pointA.position) ? pointB.position : pointA.position;
            Flip();
        }
    }

    void Flip()
    {
        // Rotate the whole GameObject using Quaternion
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "TigerPawCol")
        {
            gameObject.GetComponent<Animator>().SetBool("canWheel", true);
        }
    }

}
