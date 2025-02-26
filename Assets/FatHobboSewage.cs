using System.Collections;
using UnityEngine;

public class FatHobboSewage : MonoBehaviour
{
    public GameObject rat;
    public Transform ratPoint;
    public Transform goldBoxPoint; // Add a Transform for the gold box point
    public Animator anim;
    public float speed = 1f; // Speed of movement
    private bool isMoving = false; // State to check if currently moving

    public Collider2D hoboCollider;

    public PlayerMovement player;
    public GameObject rats;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        // You can initialize or reset variables here if needed
    }

    private void OnEnable()
    {
        StartCoroutine(MoveAndKick());
    }

    private IEnumerator MoveAndKick()
    {
        yield return new WaitForSeconds(3); // Wait for 3 seconds

        isMoving = true;
        while (isMoving)
        {
            // Move towards the target point
            Vector3 direction = (ratPoint.position - transform.position).normalized;
            float step = speed * Time.deltaTime;

            // Move the object closer to the target point
            transform.position = Vector3.MoveTowards(transform.position, ratPoint.position, step);

            // Check if the object has reached the target
            if (Vector3.Distance(transform.position, ratPoint.position) < 0.1f)
            {
                anim.SetBool("canWalk", false);
                isMoving = false; // Stop moving
                anim.SetBool("canKick", true);
            }
            else
            {
                anim.SetBool("canWalk", true);
            }

            yield return null; // Wait until the next frame
        }
    }

    private IEnumerator WaitAfterKick()
    {
        yield return new WaitForSeconds(2);
        anim.SetBool("canKick", false);
        anim.SetBool("canWalk", true);
        player.MoveCamera(0.38f, 0.15f, false);
        player.cinematic.GetComponent<Cinematic>().endCinematic();

        // Move towards the gold box point
        isMoving = true;
        while (isMoving)
        {
            // Move towards the goldBoxPoint
            Vector3 direction = (goldBoxPoint.position - transform.position).normalized;
            float step = speed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, goldBoxPoint.position, step);

            // Check if the object has reached the gold box point
            if (Vector3.Distance(transform.position, goldBoxPoint.position) < 0.1f)
            {
                //player.act1InteractionHandler.dontMoveUntilSewageHobboInPosition = false;
                hoboCollider.enabled = true;
                anim.SetBool("canWalk", false); // Stop walking animation
                isMoving = false; // Stop moving
                player.climbBucketStatus = false;
            }
            else
            {
                anim.SetBool("canWalk", true); // Keep walking animation active
            }

            yield return null; // Wait until the next frame
        }
    }

    private void kickRat()
    {
        rat.GetComponent<Animator>().SetBool("canBeKicked", true); // Trigger the rat's kick animation
    }

    public void fallNow()
    {
        anim.SetBool("canFall", true);
    }

    public void ratsCanPick()
    {
        rats.SetActive(true);
    }

    private void finishKick()
    {
        StartCoroutine(WaitAfterKick()); // Start the coroutine properly
    }

    // Update is called once per frame
    void Update()
    {
        // You can add debug or additional logic here if needed
    }
}
