using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalValve : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setRotateToFalse()
    {
        gameObject.GetComponent<Animator>().SetBool("canRotate", false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
