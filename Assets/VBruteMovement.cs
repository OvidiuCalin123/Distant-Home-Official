using UnityEngine;

public class VBruteMovement : MonoBehaviour
{
    public float walkSpeed = 2f;
    public GameObject RichWomanDoor;

    public void GoToIdle()
    {
        gameObject.GetComponent<Animator>().SetBool("canKnifeAway", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (RichWomanDoor.GetComponent<Animator>().GetBool("canFall") && gameObject.GetComponent<Animator>().GetBool("canWalk"))
        {
            gameObject.GetComponent<Animator>().SetBool("canKnifeAway", true);
            gameObject.GetComponent<Animator>().SetBool("canWalk", false);
        }

        if (gameObject.GetComponent<Animator>().GetBool("canWalk"))
        {
            // Move forward (assumes right is forward)
            transform.position += Vector3.left * walkSpeed * Time.deltaTime;
        }
    }
}
