using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHelper : MonoBehaviour
{
    public PlayerMovement player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setHobboAnim()
    {
        player.act1InteractionHandler.hobbo.GetComponent<FatHobboSewage>().fallNow();
        player.act1InteractionHandler.jimmyHand.SetActive(true);
        // TO DO: FIX ISSUE WHERE THE KEY IS TAKEN BY THE RATS ALONG WITH THE CORPSE
        // IDEA: INSTANTIATE THE KEY
        //player.act1InteractionHandler.treasureKey.transform.parent = null;  
    }

    public void stopFishAnim()
    {
        if (!Input.GetKeyDown(KeyCode.R))
        {
            player.anim2d.SetFloat("direction", 0);
        }

    }

    public void spawnWindWithTicketFunc()
    {
        GameObject wind = Instantiate(player.windWithTicket, player.windWithTicketPosition);
        wind.transform.parent = null;

        player.TicketUnderBeggarFoot.transform.parent = wind.transform;
        player.TicketUnderBeggarFoot.transform.position = wind.transform.position;
    }

    public void goBackToIdle()
    {

        player.anim2d.SetBool("canWindTicket", false);
        player.anim2d.SetBool("canFishTicket", false);
    }

    public void stopFishAnimEnd()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            player.anim2d.SetFloat("direction", 0);
        }

        player.TicketUnderBeggarFoot.transform.parent = player.fishTicketHookPos;
        player.TicketUnderBeggarFoot.GetComponent<SpriteRenderer>().sortingOrder = 1;

    }

    public void startWindTicketAim()
    {
        if (player.TicketUnderBeggarFoot.transform.parent == player.fishTicketHookPos)
        {
            player.anim2d.SetBool("canWindTicket", true);
        }

    }

    public void setCanBeScaredToFalse()
    {
        player.anim2d.SetBool("canBeScared", false);
    }

    public void MovePlayerForward(float duration)
    {
        StartCoroutine(MoveForwardForTime(duration));
    }

    private IEnumerator MoveForwardForTime(float duration)
    {
        float timer = 0;

        while (timer < duration)
        {
            // Move the player to the right over time
            transform.Translate(Vector2.right * 12 * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    public void canHideReverseEnd()
    {
        player.climbBucketStatus = false;
        player.anim2d.SetBool("canHideSewage", false);
        player.anim2d.SetBool("canHideReverse", false);
    }

    public void finishThrowingPaperBall()
    {
        player.anim2d.SetBool("canThrowPaper", false);
    }

    public void kitchenForkFallNow()
    {
        player.act2InteractionHandler.kitchenFork.GetComponent<Animator>().SetBool("canForkFall", true);
        player.act2InteractionHandler.kitchenFork.GetComponent<Collider2D>().enabled = false;
    }

    public void finishClimb()
    {

        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 1.06f, transform.position.z);
        player.ais[0].destination = transform.position;
        player.anim2d.SetBool("canClimb", false);
    }

    public void finishReverseClimb()
    {
        player.anim2d.SetBool("canReverseClimb", false);
        player.climbBucketStatus = false;
    }
    public void setJumpToFalse()
    {
        if (player.clickedObject != null)
        {
            if (player.clickedObject.tag != "spiderBox")
            {
                player.ais[0].maxSpeed = 4; ///// bug
                player.fishAfterTicket = true;
                player.beggarWithHalfTicket.enabled = true;
            }
        }
        player.anim2d.SetBool("canJump", false);
    }

    public void setPushFalse()
    {
        player.climbBucketStatus = false;
        player.anim2d.SetBool("canPush", false);
        player.act1InteractionHandler.dontMoveUntilSewageHobboInPosition = false;
    }

    public void playPlayerSteps()
    {
        player.soundManager.playPlayerSteps();
    }

    public void playGroundClick()
    {
        player.soundManager.playGroundClick();
    }

    public void setThrowHamToFalse()
    {
        player.DogQuest.ham = Instantiate(player.hamForTheDog, new Vector3(player.spawnPointHamForTheDog.position.x, player.spawnPointHamForTheDog.position.y), Quaternion.identity);
        player.anim2d.SetBool("canThrowHam", false);
        player.DogQuest.followHam = true;
        player.DogQuest.GetComponentInParent<Collider2D>().enabled = false;
        player.DogQuest.transform.parent.gameObject.transform.Find("guy").GetComponent<Collider2D>().enabled = true;
    }

    public void setAnimSpeedToMinusOne()
    {
        if(player.clickedObject != null)
        {
            if (player.clickedObject.tag != "spiderBox")
            {
                player.ais[0].destination = new Vector2(68.89f, -2.08f);
                player.ais[0].maxSpeed = 0;
                MovePlayerForward(0.34f);
            }
            
        }
        
    }

    public void handleLadderCatGroundHit()
    {
        player.act1InteractionHandler.nightLights.SetActive(true);
        player.act1InteractionHandler.dayStation.SetActive(false);
        player.soundManager.Music.Stop();
        player.act1InteractionHandler.didPlayerFallOnCatLadder = true;
        StartCoroutine(ActivateBlackScreenAfterDelay());
    }

    public void handleFinishGettingUp()
    {
        player.anim2d.SetBool("canLadderFallGetUp", false);
    }

    private IEnumerator ActivateBlackScreenAfterDelay()
    {
        player.act1InteractionHandler.blackScreen.SetActive(true);
        player.playerInventory.removeAllItemsFromInventory();
        yield return new WaitForSeconds(3f);

        player.act1InteractionHandler.blackScreenTimePassed.SetActive(true);

        yield return new WaitForSeconds(3f);

        player.act1InteractionHandler.blackScreenTimePassed.GetComponent<Animator>().SetBool("canFadeOut", true);
        player.anim2d.SetBool("canLadderFallGetUp", true);
        player.anim2d.SetBool("canLadderFall", false);
        player.anim2d.speed = 0;
        yield return new WaitForSeconds(2.5f);
        player.act1InteractionHandler.blackScreen.GetComponent<Animator>().SetBool("canFadeOut", true);
        yield return new WaitForSeconds(2f);
        player.anim2d.speed = 1;
    }
}
