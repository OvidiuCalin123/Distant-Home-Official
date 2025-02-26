using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleConductor : MonoBehaviour
{
    public PlayerMovement player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetShardsCollision()
    {
        player.act2InteractionHandler.bottleShards.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "coalProjectile")
        {
            player.act2InteractionHandler.stoneConductorPlace.GetComponent<Collider2D>().enabled = false;
            gameObject.GetComponent<Animator>().SetBool("canFall", true);
        }
    }
}
