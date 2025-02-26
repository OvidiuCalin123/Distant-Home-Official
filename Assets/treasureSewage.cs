using UnityEngine;

public class treasureSewage : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject treasure;

    public bool didTreasureDropped;
    // Start is called before the first frame update
    void Start()
    {
        didTreasureDropped = false;
    }

    public void switchToOpenAfter()
    {
        Instantiate(treasure, spawnPoint.position, spawnPoint.rotation);
        gameObject.GetComponent<Animator>().SetBool("canAfterOpen", true);
        didTreasureDropped = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
