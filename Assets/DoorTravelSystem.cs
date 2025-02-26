using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTravelSystem : MonoBehaviour
{
    public GameObject hideOutsideDarkness;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void openDoor()
    {
        hideOutsideDarkness.SetActive(false);
    }

    public void showOutsideNow()
    {
        hideOutsideDarkness.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
