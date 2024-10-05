using UnityEngine;

public class startQuest1_4 : MonoBehaviour
{
    public GameObject quest4;
    public GameObject player;
    public float activationDistance = 10.5f;

    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance <= activationDistance)
        {
            if (!quest4.activeInHierarchy)
            {
                quest4.SetActive(true);
            }
        }
    }
}
