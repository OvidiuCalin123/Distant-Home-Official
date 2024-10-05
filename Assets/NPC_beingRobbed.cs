using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_beingRobbed : MonoBehaviour
{
    public Animator anim;
    public stealSystem stealsys;
    public bool DoOnce;
    public bool canWarnPlayer;
    // Start is called before the first frame update
    void Start()
    {
        DoOnce = true;
        canWarnPlayer = false;
        anim = GetComponent<Animator>();
    }

    IEnumerator waitToBeRobbed()
    {

        yield return new WaitForSeconds(Random.Range(0.7f, 5f));

        canWarnPlayer = true;

        yield return new WaitForSeconds(0.4f);

        anim.SetBool("isBeingRobbed", true);


        yield return new WaitForSeconds(1);

        anim.SetBool("isBeingRobbed", false);
        DoOnce = true;
        canWarnPlayer = false;
        gameObject.GetComponent<NPC_EYE_TRACK>().eyeLeft.localRotation = gameObject.GetComponent<NPC_EYE_TRACK>().initialRotationLeft;
        gameObject.GetComponent<NPC_EYE_TRACK>().eyeRight.localRotation = gameObject.GetComponent<NPC_EYE_TRACK>().initialRotationRight;
    }

    // Update is called once per frame
    void Update()
    {
        if (stealsys.canSteal && DoOnce && Input.GetKey(KeyCode.E))
        {
            DoOnce = false;
            StartCoroutine(waitToBeRobbed());
            
        }

    }
}
