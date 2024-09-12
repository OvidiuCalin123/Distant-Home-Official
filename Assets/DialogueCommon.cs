using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCommon : MonoBehaviour
{
    private Camera mainCamera;
    public bool playerNearNPC;

    public GameObject DialogueBox;
    public GameObject optionalMarker;
    public GameObject optionalGameObject;

    // Start is called before the first frame update
    void Start()
    {
        // Cache the main camera
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerNearNPC)
        {
            DialogueBox.SetActive(true);

            if (optionalMarker)
            {
                optionalMarker.SetActive(false);
            }
            if (optionalGameObject)
            {
                optionalGameObject.SetActive(false);
            }
        }
        else
        {

            DialogueBox.SetActive(false);

            optionalGameObject.SetActive(true);
        }
    }
}
