using System.Collections.Generic;
using UnityEngine;

public class HanoiRods : MonoBehaviour
{
    public Transform Rod1, Rod2, Rod3;
    public HanoiDisc Disc1, Disc2, Disc3, Disc4, Disc5;

    private Dictionary<Transform, Stack<HanoiDisc>> rodStacks = new Dictionary<Transform, Stack<HanoiDisc>>();
    private float discHeight = 80f; // Height difference between stacked discs
    private Vector3 baseOffset = new Vector3(0, -250, 0); // Adjust for rod bottom

    public GameObject HanoiSlider;
    public GameObject HanoiMiniGameUI;
    public GameObject HanoiMiniGameWorldCollider;

    public PlayerMovement player;

    void Start()
    {
        rodStacks[Rod1] = new Stack<HanoiDisc>();
        rodStacks[Rod2] = new Stack<HanoiDisc>();
        rodStacks[Rod3] = new Stack<HanoiDisc>();

        // Start with all discs on Rod1 (largest at the bottom)
        PlaceDiscOnRod(Disc5, Rod1);
        PlaceDiscOnRod(Disc4, Rod1);
        PlaceDiscOnRod(Disc3, Rod1);
        PlaceDiscOnRod(Disc2, Rod1);
        PlaceDiscOnRod(Disc1, Rod1);
    }

    void Update()
    {
        if(Rod3.childCount == 5 && HanoiMiniGameUI)
        {
            HanoiSlider.transform.position = new Vector2(HanoiSlider.transform.position.x + 3.9f, HanoiSlider.transform.position.y);
            HanoiMiniGameUI.SetActive(false);
            HanoiMiniGameWorldCollider.GetComponent<Collider2D>().enabled = false;
            HanoiMiniGameWorldCollider.GetComponent<Animator>().SetBool("canDone", true);
            player.soundManager.playCogsHanoi();
        }    
    }

    public Transform GetClosestRod(Vector3 position)
    {
        Transform closestRod = null;
        float minDistance = float.MaxValue;

        foreach (var rod in new List<Transform> { Rod1, Rod2, Rod3 })
        {
            float distance = Vector3.Distance(position, rod.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestRod = rod;
            }
        }

        return minDistance < 240f ? closestRod : null; // Adjust sensitivity as needed
    }

    public bool CanPlaceOnRod(HanoiDisc disc, Transform rod)
    {
        if (rodStacks[rod].Count == 0) return true; // Empty rod allows any disc

        HanoiDisc topDisc = rodStacks[rod].Peek(); // Get top disc
        return int.Parse(disc.name.Replace("Disc", "")) < int.Parse(topDisc.name.Replace("Disc", ""));
    }

    public void PlaceDiscOnRod(HanoiDisc disc, Transform rod)
    {
        // Remove from previous rod
        foreach (var stack in rodStacks.Values)
        {
            if (stack.Contains(disc))
            {
                stack.Pop();
                break;
            }
        }

        // Add to new rod
        rodStacks[rod].Push(disc);
        UpdateDiscPositions(rod);
    }

    private void UpdateDiscPositions(Transform rod)
    {
        int count = rodStacks[rod].Count;
        Vector3 basePosition = rod.position + baseOffset;

        foreach (var disc in rodStacks[rod])
        {
            count--;
            disc.transform.position = basePosition + new Vector3(0, count * discHeight, 0);
            disc.transform.SetParent(rod);
        }
    }

    public bool IsTopDisc(HanoiDisc disc)
    {
        foreach (var stack in rodStacks.Values)
        {
            if (stack.Count > 0 && stack.Peek() == disc)
            {
                return true; // This disc is on top
            }
        }
        return false;
    }
}
