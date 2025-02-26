using UnityEngine;

public class TrainTunnelMovement : MonoBehaviour
{
    public float speed = 5f;
    private float targetX = 476f;

    public PlayerMovement player;

    void Update()
    {
        if (player.act2InteractionHandler.isPlayerInTunnel)
        {
            if (transform.position.x > targetX)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetX, transform.position.y, transform.position.z), speed * Time.deltaTime);
            }
        }
    }
}