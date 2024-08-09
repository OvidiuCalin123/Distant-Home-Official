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
    private bool effectInstantiated = false;

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
        if (Input.GetMouseButtonDown(0))
        {
            effectInstantiated = false;
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            LayerMask mask = 1 << 6;

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, mask);

            RaycastHit2D hit2 = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity);

            if(hit2.collider != null)
            {

                if (hit2.collider.CompareTag("TicketGuy"))
                {

                    
                    canWalk = true;

                    ai.destination = hit2.collider.gameObject.transform.position;

                    Vector3 playerPosition = transform.position;

                    // Flip player based on mouse click position
                    if (mousePos.x < playerPosition.x)
                    {
                        // Click is to the left of the player
                        transform.rotation = Quaternion.Euler(0, 180, 0);
                    }
                    else
                    {
                        // Click is to the right of the player
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                    }

                }
            }
            

            if (hit.collider != null)
            {

                if (effectInstantiated == false)
                {
                    Instantiate(mouseClickGroundEffect, hit2.point, Quaternion.identity);
                    effectInstantiated = true;
                }

                canWalk = true;
                ai.destination = hit.point;

                // Calculate player position
                Vector3 playerPosition = transform.position;

                // Flip player based on mouse click position
                if (mousePos.x < playerPosition.x)
                {
                    // Click is to the left of the player
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else
                {
                    // Click is to the right of the player
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
