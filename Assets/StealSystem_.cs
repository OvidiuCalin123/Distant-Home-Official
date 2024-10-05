using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealSystem_ : MonoBehaviour
{
    public GameObject Danger;
    public GameObject Arm;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(HandleDangerAppearance());
    }

    // Coroutine to handle the appearance of danger every random 3 to 5 seconds
    IEnumerator HandleDangerAppearance()
    {
        while (true && Arm.GetComponent<NpcStealArm>().canStartCycleAgain)
        {
            // Wait for a random time between 3 to 5 seconds
            float randomTime = Random.Range(3f, 6f);
            yield return new WaitForSeconds(randomTime);

            // Set the Danger object to active (true)
            Danger.SetActive(true);

            // Wait for 2 seconds with Danger active
            yield return new WaitForSeconds(2f);

            // Execute the logic when danger is active
            Arm.GetComponent<Animator>().SetBool("canScratch", true);

            // After 2 seconds, deactivate the Danger object
            Danger.SetActive(false);
        }
    }
}
