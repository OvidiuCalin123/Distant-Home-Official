using System.Collections;
using UnityEngine;

public class KillerTrain : MonoBehaviour
{
    public PlayerMovement player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void teleportPlayer()
    {
        player.teleportPlayerToPos.screenPos = 0.64f;
        player.teleportPlayerToPos.playerTeleportPos = new Vector3(680.03f, -5.73f, gameObject.transform.position.z);
        player.teleportPlayerToPos.gameObject.SetActive(true);
        player.act2InteractionHandler.isPlayerOnTrain = false;

        StartCoroutine(waitSlappedKillerBlackScreenDissapear());
    }

    IEnumerator waitSlappedKillerBlackScreenDissapear()
    {
        yield return new WaitForSeconds(4);

        player.act2InteractionHandler.afterKillerSlappedBlackScreen.GetComponent<Animator>().SetBool("dis", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
