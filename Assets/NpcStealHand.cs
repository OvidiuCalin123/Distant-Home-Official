using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcStealHand : MonoBehaviour
{

    public Animator anim;
    public bool onlyOnce;

    public GameObject arm;

    // Start is called before the first frame update
    void Start()
    {
        onlyOnce = false;
        anim = GetComponent<Animator>();
    }

    IEnumerator func()
    {
        yield return new WaitForSeconds(2f);
        anim.SetBool("canScratch", false);
        onlyOnce = false;
        arm.GetComponent<Animator>().speed = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetBool("canScratch") && onlyOnce == false)
        {
            onlyOnce = true;
            StartCoroutine(func());
        }
    }
}
