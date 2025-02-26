using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsLogic : MonoBehaviour
{

    public GameObject helpText1;
    public GameObject helpText2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void MakeHelpTextVisible()
    {
        helpText1.SetActive(true);
        helpText2.SetActive(true);
    }

    public void MakeHelpTextHide()
    {
        helpText1.SetActive(false);
        helpText2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
