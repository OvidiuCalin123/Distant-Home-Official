using System.Runtime.InteropServices;
using UnityEngine;

public class stealSystem : MonoBehaviour
{

    [DllImport("User32.dll")]
    private static extern bool SetCursorPos(int x, int y);

    public string stealItemUI_text;

    public GameObject stealCircleSelectPerson;

    public GameObject Player;

    public bool canSteal;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void MoveMouseToRightMiddle()
    {
        // Get the screen width and height
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;

        // Calculate the far right middle position (X = screenWidth, Y = screenHeight / 2)
        int targetX = screenWidth - 1; // To ensure it's within the screen boundary
        int targetY = screenHeight / 2;

        // Move the mouse to the calculated position
        SetCursorPos(targetX, targetY);
    }


    void Update()
    {
        //if (Player != null)
        //{
        //    PlayerMovement playerMovement = Player.GetComponent<PlayerMovement>();

        //    if (playerMovement != null)
        //    {
                
        //        if (stealUI.activeSelf && Input.GetKeyDown(KeyCode.E))
        //        {
        //            Cursor.visible = true;
        //            stealUI.SetActive(false);
        //            stealUIBlur.SetActive(false);
                    
        //        }

        //        else if (!stealUI.activeSelf && playerMovement.stealUIPopup.activeSelf && Input.GetKeyDown(KeyCode.E))
        //        {
        //            Cursor.visible = false;
        //            stealUI.SetActive(true);
        //            stealUIBlur.SetActive(true);
        //            MoveMouseToRightMiddle();
        //        }
        //    }
        //}
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player = collision.gameObject;
            stealCircleSelectPerson.SetActive(true);
            collision.gameObject.GetComponent<PlayerMovement>().stealUIPopup.SetActive(true);
            collision.gameObject.GetComponent<PlayerMovement>().stealSystem_ref = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player = null;
            stealCircleSelectPerson.SetActive(false);
            collision.gameObject.GetComponent<PlayerMovement>().stealUIPopup.SetActive(false);
            canSteal = false;
            collision.gameObject.GetComponent<PlayerMovement>().stealSystem_ref = null;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Player = collision.gameObject;
            canSteal = true;

            if (!Input.GetKey(KeyCode.E))
            {
                collision.gameObject.GetComponent<PlayerMovement>().stealUIPopup.SetActive(true);
            }
            else
            {
                collision.gameObject.GetComponent<PlayerMovement>().stealUIPopup.SetActive(false);
            }
            
        }
    }
}
