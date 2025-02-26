using UnityEngine;
using UnityEngine.EventSystems;

public class HanoiDisc : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 startPosition;
    private Transform startParent;
    private HanoiRods hanoiRods;
    private bool isDragging = false;

    private void Start()
    {
        hanoiRods = FindObjectOfType<HanoiRods>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!hanoiRods.IsTopDisc(this))
        {
            eventData.pointerDrag = null; // Cancel drag if not the top disc
            return;
        }

        startPosition = transform.position;
        startParent = transform.parent;
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            transform.position = Input.mousePosition; // Follow the cursor
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        Transform closestRod = hanoiRods.GetClosestRod(transform.position);
        if (closestRod != null && hanoiRods.CanPlaceOnRod(this, closestRod))
        {
            hanoiRods.PlaceDiscOnRod(this, closestRod);
        }
        else
        {
            // Invalid move, return to original position
            transform.position = startPosition;
        }
    }
}
