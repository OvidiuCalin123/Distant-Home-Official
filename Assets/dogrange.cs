using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dogrange : MonoBehaviour
{

    public GameObject quest4;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(quest4.activeSelf == false)
            {
                quest4.SetActive(true);
            }
        }
    }
}
