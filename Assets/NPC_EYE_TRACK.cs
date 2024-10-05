using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_EYE_TRACK : MonoBehaviour
{
    public Transform eyeLeft;
    public Transform eyeRight;
    public Transform lookAtPoint;
    private Transform player;

    public Quaternion initialRotationLeft;
    public Quaternion initialRotationRight;

    // Start is called before the first frame update
    void Start()
    {
        
        initialRotationLeft = eyeLeft.localRotation;
        initialRotationRight = eyeRight.localRotation;
        
    }

    void Update()
    {
        if (gameObject.GetComponent<NPC_beingRobbed>())
        {
            if (gameObject.GetComponent<NPC_beingRobbed>().anim.GetBool("isBeingRobbed") == true)
            {
                RotateEyesTowardsPoint();
            }

            if (gameObject.GetComponent<NPC_beingRobbed>().canWarnPlayer == true)
            {
                RotateEyesTowardsPoint();
            }
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;

            eyeLeft.localRotation = initialRotationLeft;
            eyeRight.localRotation = initialRotationRight;
            
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && lookAtPoint != null)
        {
            RotateEyesTowardsPoint();
        }
    }

    void RotateEyesTowardsPoint()
    {
        if (lookAtPoint != null)
        {
            if (eyeLeft != null)
            {
                Vector2 directionLeft = lookAtPoint.position - eyeLeft.position;
                float angleLeft = Mathf.Atan2(directionLeft.y, directionLeft.x) * Mathf.Rad2Deg;
                eyeLeft.localRotation = Quaternion.Euler(0, 0, angleLeft + 90);
            }

            if (eyeRight != null)
            {
                Vector2 directionRight = lookAtPoint.position - eyeRight.position;
                float angleRight = Mathf.Atan2(directionRight.y, directionRight.x) * Mathf.Rad2Deg;
                eyeRight.localRotation = Quaternion.Euler(0, 0, angleRight + 90);
            }
        }
    }
}
