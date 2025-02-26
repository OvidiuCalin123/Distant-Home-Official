using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TigerScript : MonoBehaviour
{

    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void goToIdle()
    {
        anim.SetBool("canpaw", false);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
