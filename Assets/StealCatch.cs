using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealCatch : MonoBehaviour
{
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            anim.SetBool("canCatch", true);
        }
        else
        {
            anim.SetBool("canCatch", false);

        }
    }
}
