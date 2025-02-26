using UnityEngine;

public class eventDoorKnobClick : MonoBehaviour
{
    public Animator anim;
    public GameObject candleEventDoor;
    public GameObject Event;
    public PlayerMovement player;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void clickOnKnob()
    {
        anim.SetBool("canTurn", true);
    }

    public void openDoorCandle()
    {
        candleEventDoor.GetComponent<Animator>().SetBool("canOpen", true);
        candleEventDoor.GetComponent<Collider2D>().enabled = false;
        Event.SetActive(false);
        player.act2InteractionHandler.CandlePlayerStorage.SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
