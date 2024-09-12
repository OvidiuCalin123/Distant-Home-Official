using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc_spawner : MonoBehaviour
{
    public GameObject[] npcPrefabs; // Array of NPC prefabs to randomly select from

    public GameObject[] npcPrefabsFar; // Array of NPC prefabs to randomly select from

    public Transform spawnPoint1; // First spawn point
    public Transform spawnPoint2; // Second spawn point
    public Transform spawnPoint3; // Third spawn point

    public Transform spawnPoint4; // First spawn point
    public Transform spawnPoint5; // Second spawn point


    public float spawnInterval = 3f; // Interval in seconds for spawning NPCs

    private System.Random random = new System.Random(); // Random number generator

    void Start()
    {
        // Start spawning NPCs at each of the three points
        StartCoroutine(SpawnNPCAtInterval());
    }

    IEnumerator SpawnNPCAtInterval()
    {
        while (true) // Infinite loop to keep spawning NPCs
        {
            yield return new WaitForSeconds(spawnInterval); // Wait for the specified interval

            // List to track selected NPCs for this cycle
            List<int> selectedNPCs = new List<int>();

            // Spawn NPCs at each of the spawn points with a random y position
            SpawnUniqueNPC(spawnPoint1, selectedNPCs);
            SpawnUniqueNPC(spawnPoint2, selectedNPCs);
            SpawnUniqueNPC(spawnPoint3, selectedNPCs);

            SpawnUniqueNPCFar(spawnPoint4, selectedNPCs);
            SpawnUniqueNPCFar(spawnPoint5, selectedNPCs);
        }
    }

    void SpawnUniqueNPC(Transform spawnPoint, List<int> selectedNPCs)
    {
        int npcIndex;

        // Ensure a unique NPC is selected that hasn't been used in this cycle
        do
        {
            npcIndex = random.Next(0, npcPrefabs.Length);
        }
        while (selectedNPCs.Contains(npcIndex) && selectedNPCs.Count < npcPrefabs.Length);

        // Add the selected NPC index to the list
        selectedNPCs.Add(npcIndex);

        // Randomly adjust the y position between -1 and 1
        float randomY = (float)(random.NextDouble() * 2 - 1);

        // Adjust the spawn position with the random y value
        Vector3 spawnPosition = new Vector3(spawnPoint.position.x, spawnPoint.position.y + randomY, spawnPoint.position.z);

        // Spawn the selected NPC at the adjusted position
        Instantiate(npcPrefabs[npcIndex], spawnPosition, Quaternion.identity);
    }

    void SpawnUniqueNPCFar(Transform spawnPoint, List<int> selectedNPCs)
    {
        int npcIndex;

        // Ensure a unique NPC is selected that hasn't been used in this cycle
        do
        {
            npcIndex = random.Next(0, npcPrefabsFar.Length);
        }
        while (selectedNPCs.Contains(npcIndex) && selectedNPCs.Count < npcPrefabsFar.Length);

        // Add the selected NPC index to the list
        selectedNPCs.Add(npcIndex);

        // Randomly adjust the y position between -1 and 1
        float randomY = (float)(random.NextDouble() * 2 - 1);

        // Adjust the spawn position with the random y value
        Vector3 spawnPosition = new Vector3(spawnPoint.position.x, spawnPoint.position.y + randomY, spawnPoint.position.z);

        // Spawn the selected NPC at the adjusted position
        Instantiate(npcPrefabsFar[npcIndex], spawnPosition, Quaternion.identity);
    }
}
