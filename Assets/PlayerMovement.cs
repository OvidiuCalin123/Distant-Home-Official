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
    public float maxCameraSize = 7.4f;
    public float minCameraSize = 6.6f;
    public float cameraAdjustSpeed = 0.1f;

    private CinemachineVirtualCamera vcam;
    private bool isInMakeCameraBigZone = false;

    public GameObject mouseClickGroundEffect;
    private GameObject currentEffect;

    public GameObject clickedObject;

    public bool pickedInteractableDestination;

    private void handleInteractiveWorldEvent()
    {
        if (clickedObject != null)
        {
            Debug.Log(clickedObject.tag);

            if (clickedObject.tag == "thrash_bin")
            {
                clickedObject.GetComponent<Animator>().SetBool("canOpenTrash", true);
                clickedObject.GetComponent<Collider2D>().enabled = false;
            }
            else if (clickedObject.tag == "Coin")
            {
                Destroy(clickedObject);
            }
            else if (clickedObject.tag == "newspaper_guy")
            {
                clickedObject.transform.Find("Npc_Dialogue").gameObject.SetActive(true);
                clickedObject.transform.Find("Npc_Dialogue").gameObject.transform.Find("Dialogue_Background").gameObject.GetComponent<DialogueNavigation>().playerNearNPC = true;
                    
            }
            else if(clickedObject.tag == "NextDialogue")
            {
                clickedObject.GetComponent<DialogueNavigation>().dialogueState += 1;
                clickedObject.GetComponent<DialogueNavigation>().playerNearNPC = true;
            }
        }
    }

    void Start()
    {
        vcam = FindObjectOfType<CinemachineVirtualCamera>();
        anim2d = GetComponent<Animator>();
        ais = FindObjectsOfType<MonoBehaviour>().OfType<IAstarAI>().ToArray();
        pickedInteractableDestination = false;
    }

    void Update()
    {
        if (ais[0].reachedDestination)
        {
            canWalk = false;

            if (pickedInteractableDestination)
            {
                pickedInteractableDestination = false;
                handleInteractiveWorldEvent();
            }
           
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

    private void handleSpawnGroundClickEffect(Vector2 mousePos, RaycastHit2D? mostRelevantHit)
    {
        if (clickedObject != null)
        {
            if (clickedObject.layer == 6)
            {

                currentEffect = Instantiate(mouseClickGroundEffect, mostRelevantHit.Value.point, Quaternion.identity);

                if (mousePos.x < transform.position.x)
                {
                    currentEffect.transform.localScale = new Vector3(-0.6f, 0.6f, 1);
                }
                else
                {
                    currentEffect.transform.localScale = new Vector3(0.6f, 0.6f, 1);
                }
            }
        }
    }

    private void handleNoMovementWhenClickingDialogue(IAstarAI ai, RaycastHit2D? mostRelevantHit, Vector2 mousePos)
    {
        if (clickedObject.layer != 6)
        {
            pickedInteractableDestination = true;
        }

        if (clickedObject.tag != "NextDialogue")
        {
            
            if (mostRelevantHit.HasValue)
            {
                var hitCollider = mostRelevantHit.Value.collider;
                canWalk = true;

                if (clickedObject.layer == 10)
                {
                    ai.destination = clickedObject.transform.position;
                }
                else
                {
                    ai.destination = mostRelevantHit.Value.point;
                }

                // Flip player based on mouse click position
                if (mousePos.x < transform.position.x)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }

                // Handle ground click effect
                if (currentEffect != null)
                {
                    Destroy(currentEffect);
                }

                handleSpawnGroundClickEffect(mousePos, mostRelevantHit);
            }
        }
    }

    void PointAndClick(IAstarAI ai)
    {
        if (Input.GetMouseButtonDown(0))
        {

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.zero, Mathf.Infinity);

            RaycastHit2D? mostRelevantHit = null;

            foreach (var hit in hits)
            {
                if (hit.collider != null)
                {
                    
                    if (hit.collider.tag != "NextDialogue")
                    {
                        if (clickedObject != null)
                        {
                            if (clickedObject.GetComponent<DialogueNavigation>() != null)
                            {
                                clickedObject.GetComponent<DialogueNavigation>().playerNearNPC = false;
                            }
                        }

                        
                        if (mostRelevantHit == null || hit.collider.transform.position.z > mostRelevantHit.Value.collider.transform.position.z)
                        {
                            mostRelevantHit = hit;
                        }
                    }
                    
                    clickedObject = hit.collider.gameObject;
                }
            }

            handleNoMovementWhenClickingDialogue(ai, mostRelevantHit, mousePos);

            
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
