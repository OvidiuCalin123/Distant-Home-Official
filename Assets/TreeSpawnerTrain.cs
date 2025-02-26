using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawnerTrain : MonoBehaviour
{
    public GameObject treePrefab;  // The tree prefab to spawn
    public Transform endDestroyHere; // Position where trees should be destroyed
    public Transform startGenerate; // Position where trees should spawn
    public float spawnInterval = 2f; // Interval between tree spawns
    public float moveSpeed = 5f; // Speed at which trees move
    public float spawnRange = 5f; // Random range around spawn position

    private List<GameObject> trees = new List<GameObject>(); // List to store spawned trees

    void Start()
    {
        StartCoroutine(SpawnTrees());
    }

    void Update()
    {
        MoveTrees();
        DestroyTrees();
    }

    IEnumerator SpawnTrees()
    {
        while (true)
        {
            Vector3 spawnPosition = startGenerate.position + new Vector3(Random.Range(-spawnRange, spawnRange), 0, Random.Range(-spawnRange, spawnRange));
            GameObject newTree = Instantiate(treePrefab, spawnPosition, Quaternion.identity);
            trees.Add(newTree);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void MoveTrees()
    {
        foreach (GameObject tree in trees)
        {
            if (tree != null)
            {
                tree.transform.position = Vector3.MoveTowards(tree.transform.position, endDestroyHere.position, moveSpeed * Time.deltaTime);
            }
        }
    }

    void DestroyTrees()
    {
        for (int i = trees.Count - 1; i >= 0; i--)
        {
            if (trees[i] != null && Vector3.Distance(trees[i].transform.position, endDestroyHere.position) < 0.5f)
            {
                Destroy(trees[i]);
                trees.RemoveAt(i);
            }
        }
    }
}
