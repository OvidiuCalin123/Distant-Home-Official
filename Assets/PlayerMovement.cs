using UnityEngine;
using System.Linq;
using Pathfinding;
using Cinemachine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public Animator anim2d;
    public bool canWalk;
    public LayerMask mask;
    public Transform target;
    public IAstarAI[] ais;
    public float maxCameraSize = 7f;
    public float minCameraSize = 6f;
    public float cameraAdjustSpeed = 0.1f;

    private CinemachineVirtualCamera vcam;
    private bool isInMakeCameraBigZone = false;

    public GameObject mouseClickGroundEffect;
    private GameObject currentEffect;

    void Start()
    {
        vcam = FindObjectOfType<CinemachineVirtualCamera>();
        anim2d = GetComponent<Animator>();
        ais = FindObjectsOfType<MonoBehaviour>().OfType<IAstarAI>().ToArray();
    }

    void Update()
    {
        if (ais[0].reachedDestination)
        {
            canWalk = false;

            // Destroy the effect if the player reached the destination
            if (currentEffect != null)
            {
                Destroy(currentEffect);
            }
        }

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            PointAndClick(ais[0]);
        }

        anim2d.SetBool("canWalk", canWalk);

        AdjustCameraSize();
    }

    void PointAndClick(IAstarAI ai)
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            LayerMask mask = 1 << 6;

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, mask);
            RaycastHit2D hit2 = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity);

            if (hit2.collider != null && hit2.collider.CompareTag("TicketGuy"))
            {
                canWalk = true;
                ai.destination = hit2.collider.gameObject.transform.position;

                // Flip player based on mouse click position
                if (mousePos.x < transform.position.x)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }

            if (hit.collider != null)
            {
                // Destroy the old effect if it exists
                if (currentEffect != null)
                {
                    Destroy(currentEffect);
                }

                // Instantiate the new effect at the clicked position
                currentEffect = Instantiate(mouseClickGroundEffect, hit.point, Quaternion.identity);

                // Flip the effect if the click is to the left of the player
                if (mousePos.x < transform.position.x)
                {
                    currentEffect.transform.localScale = new Vector3(-0.6f, 0.6f, 1);  // Flip the effect on the X axis
                }
                else
                {
                    currentEffect.transform.localScale = new Vector3(0.6f, 0.6f, 1);   // Ensure the effect is not flipped
                }

                canWalk = true;
                ai.destination = hit.point;

                // Flip player based on mouse click position
                if (mousePos.x < transform.position.x)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MakeCameraBig")
        {
            isInMakeCameraBigZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MakeCameraBig")
        {
            isInMakeCameraBigZone = false;
        }
    }

    private void AdjustCameraSize()
    {
        float targetSize = isInMakeCameraBigZone ? maxCameraSize : minCameraSize;
        float currentSize = vcam.m_Lens.OrthographicSize;

        if (Mathf.Abs(currentSize - targetSize) > 0.01f)
        {
            vcam.m_Lens.OrthographicSize = Mathf.MoveTowards(currentSize, targetSize, cameraAdjustSpeed * Time.deltaTime);
        }
    }
}
