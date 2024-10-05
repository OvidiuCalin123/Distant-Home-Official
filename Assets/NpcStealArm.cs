using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcStealArm : MonoBehaviour
{

    public GameObject hand;
    public bool canStartCycleAgain;
    // Start is called before the first frame update
    void Start()
    {
        canStartCycleAgain = true;
    }

    public void returnToIdle()
    {
        gameObject.GetComponent<Animator>().SetBool("canScratch", false);
        canStartCycleAgain = true;
    }

    public void startScratch()
    {
        hand.GetComponent<Animator>().SetBool("canScratch", true);
        gameObject.GetComponent<Animator>().speed = 0;
        canStartCycleAgain = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
