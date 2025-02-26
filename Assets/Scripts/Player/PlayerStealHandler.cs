using TMPro;
using UnityEngine;

public class PlayerStealHandler : MonoBehaviour
{
    public PlayerMovement player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject SpawnStolenItemUI(float posX, float posY)
    {

        GameObject itemStolenUI_screen = Instantiate(player.UI_StolenItem_text, player.playerInventory.canvasTransform);


        RectTransform rectTransform = itemStolenUI_screen.GetComponent<RectTransform>();


        rectTransform.anchoredPosition = new Vector2(posX, posY);

        return itemStolenUI_screen;
    }

    public void endStealStateAndAddItemToInventory()
    {

        player.anim2d.SetBool("canSteal", false);
        GameObject itemStolen = SpawnStolenItemUI(-13, 70);
        itemStolen.GetComponent<TextMeshProUGUI>().text = player.stealSystem_ref.stealItemUI_text;
        //stealSystem_ref.gameObject.SetActive(false);

        foreach (GameObject inventoryItem in player.playerInventory.inventoryItems)
        {
            if (inventoryItem.tag == "CoinItem")
            {
                player.playerCoins += 1;
                player.playerInventory.updateInventoryItemCountUp(inventoryItem);
                return;
            }
        }
        foreach (GameObject item in player.playerInventory.availabelItemsItems)
        {
            if (item.tag == "CoinItem")
            {
                player.playerCoins = 1;
                player.playerInventory.addNewInventoryItem(item);
                return;
            }
        }
    }
}
