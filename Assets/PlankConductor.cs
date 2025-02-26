using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankConductor : MonoBehaviour
{
    public GameObject coalShootable;
    public Transform coalSpawnPoint;
    public Transform coalDestroyPoint;
    public float launchForce = 5f;
    public float archHeight = 2f;

    public GameObject coal;

    void Start()
    {
    }

    public void shootCoalAndGoToIdle()
    {
        coal.SetActive(false);
        // Spawn and shoot the coal immediately
        ShootCoal();

        // Start coroutine to wait 1 second before setting canHit to false
        StartCoroutine(DisableCanHitAfterDelay());
    }

    private void ShootCoal()
    {
        // Spawn the coal
        GameObject coalInstance = Instantiate(coalShootable, coalSpawnPoint.position, Quaternion.identity);

        // Add Rigidbody if not already attached
        Rigidbody2D rb = coalInstance.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = coalInstance.AddComponent<Rigidbody2D>();
        }

        // Calculate the arching trajectory
        Vector2 launchDirection = new Vector2(1, archHeight).normalized; // Adjust direction if needed
        rb.velocity = launchDirection * launchForce;

        // Start the coroutine to destroy the coal when it falls below coalDestroyPoint
        StartCoroutine(DestroyCoalWhenFallen(coalInstance));
    }

    private IEnumerator DisableCanHitAfterDelay()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second
        gameObject.GetComponent<Animator>().SetBool("canHit", false);
    }

    private IEnumerator DestroyCoalWhenFallen(GameObject coal)
    {
        while (coal != null)
        {
            if (coal.transform.position.y < coalDestroyPoint.position.y)
            {
                Destroy(coal);
                yield break; // Exit coroutine
            }
            yield return null; // Wait for the next frame
        }
    }

    void Update()
    {
    }
}
