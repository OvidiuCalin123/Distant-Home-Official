using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class WindWithTicket : MonoBehaviour
{
    public float speed = 5f;                 // Speed at which the object moves
    public float rotationSpeed = 5f;         // Speed at which the object rotates
    public float closeEnoughDistance = 0.1f; // Distance to determine if it's close enough to the target
    private Transform sewageTransform;
    private CinemachineVirtualCamera virtualCamera; // Reference to the Cinemachine camera
    private Transform playerTransform;       // Reference to the Player's transform
    private GameObject player;

    void Start()
    {
        // Find the GameObject named "SewageEntrance" in the scene
        GameObject sewage = GameObject.Find("SewageEntrance");
        if (sewage != null)
        {
            sewageTransform = sewage.transform;
        }
        else
        {
            Debug.LogError("SewageEntrance GameObject not found in the scene!");
        }

        // Find and set the Cinemachine camera
        virtualCamera = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        if (virtualCamera != null)
        {
            virtualCamera.Follow = transform; // Set this object as the follow target
        }
        else
        {
            Debug.LogError("CM vcam1 not found in the scene!");
        }

        // Find the Player GameObject
        player = GameObject.Find("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player GameObject not found in the scene!");
        }
    }

    void Update()
    {
        // Ensure the target exists
        if (sewageTransform != null)
        {
            // Calculate the direction towards the sewage object
            Vector2 direction = (sewageTransform.position - transform.position);

            // Check if close enough to the destination
            if (direction.magnitude <= closeEnoughDistance)
            {
                // Switch the camera to follow the Player before destroying this GameObject
                if (virtualCamera != null && playerTransform != null)
                {
                    virtualCamera.Follow = playerTransform;
                }
                player.GetComponent<PlayerMovement>().cantMoveWhileInsideMiniGame = false;
                player.GetComponent<PlayerMovement>().playerInventory.removeItemFromInventory("StringHookStick");
                Destroy(gameObject); // Destroy object upon reaching destination
                return;
            }

            // Normalize direction for consistent speed
            direction.Normalize();

            // Move towards the sewage object
            transform.position += (Vector3)direction * speed * Time.deltaTime;

            // Calculate the rotation angle towards the target
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Rotate smoothly towards the target
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
