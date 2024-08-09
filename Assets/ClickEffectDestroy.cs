using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEffectDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void onFinishDestroy()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
