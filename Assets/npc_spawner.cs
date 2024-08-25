using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc_spawner : MonoBehaviour
{
    public GameObject npcPrefab; // The NPC prefab to spawn

    public Transform spawnPoint1; // First spawn point
    public Transform spawnPoint2; // Second spawn point
    public Transform spawnPoint3; // Third spawn point

    public float spawnInterval1 = 3f; // Interval in seconds for first spawn point
    public float spawnInterval2 = 5f; // Interval in seconds for second spawn point
    public float spawnInterval3 = 7f; // Interval in seconds for third spawn point

    private System.Random random = new System.Random(); // Create a random number generator

    void Start()
    {
        // Start spawning NPCs at the three points
        StartCoroutine(SpawnNPCAtInterval(spawnPoint1, spawnInterval1));
        StartCoroutine(SpawnNPCAtInterval(spawnPoint2, spawnInterval2));
        StartCoroutine(SpawnNPCAtInterval(spawnPoint3, spawnInterval3));
    }

    IEnumerator SpawnNPCAtInterval(Transform spawnPoint, float interval)
    {
        while (true) // Infinite loop to keep attempting to spawn NPCs
        {
            yield return new WaitForSeconds(interval); // Wait for the specified interval

            int roll = random.Next(0, 101); // Roll a number between 0 and 100

            if (roll > 70) // If the roll is greater than 70, spawn the NPC
            {
                Instantiate(npcPrefab, spawnPoint.position, Quaternion.identity);
            }
        }
    }
}
