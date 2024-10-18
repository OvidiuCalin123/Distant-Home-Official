using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadingNewsPaper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void stopAnimation()
    {
        gameObject.GetComponent<Animator>().speed = 0;
        Invoke("enableAnimation", 1.5f);
    }

    public void enableAnimation()
    {
        gameObject.GetComponent<Animator>().speed = 1;
    }

    public void resumeToIdle()
    {
        gameObject.GetComponent<Animator>().SetBool("canLookAtPlayer", false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
