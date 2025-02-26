using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinematic : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();   
    }

    public void stayOnScreen()
    {
        animator.SetBool("canStartCinematic", true);
    }

    public void endCinematic()
    {
        animator.SetBool("canEndCinematic", true);
    }

    public void defaultCinematic()
    {
        animator.SetBool("canEndCinematic", false);
        animator.SetBool("canStartCinematic", false);

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
