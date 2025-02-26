using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public GameObject[] availabelItemsItems;
    public List<GameObject> inventoryItems = new List<GameObject>();
    public Transform canvasTransform;
    public GameObject inventoryUIList;
    public PlayerMovement player;

    private void Start()
    {
        player = GetComponent<PlayerMovement>();
    }

    public GameObject getAvailableItemByTag(string itemTag)
    {
        foreach (GameObject item in availabelItemsItems)
        {
            if (item.tag == itemTag)
            {
                return item;
            }
        }

        return null;
    }

    public void playItemSelect()
    {
        player.soundManager.playItemSelect();
    }

    public bool isItemInPlayerInventory(string itemTag)
    {
        foreach (GameObject item in inventoryItems)
        {
            if (item.tag == itemTag)
            {
                return true;
            }
        }

        return false;
    }

    public void removeItemFromInventory(string itemTag)
    {
        foreach (GameObject item in inventoryItems)
        {
            if (item.tag == itemTag)
            {
                inventoryItems.Remove(item);
                Destroy(item);
                break;
            }
        }
    }

    public void removeAllItemsFromInventory()
    {
        for (int i = inventoryItems.Count - 1; i >= 0; i--)
        {
            GameObject item = inventoryItems[i];
            inventoryItems.RemoveAt(i);
            Destroy(item);
        }
    }

    public void updateInventoryItemCountUp(GameObject inventoryItem)
    {
        TextMeshProUGUI amountText = inventoryItem.transform.Find("amount").GetComponent<TextMeshProUGUI>();

        string currentText = amountText.text;
        int currentAmount = int.Parse(currentText.Substring(1));

        int newAmount = currentAmount + 1;
        amountText.text = $"x{newAmount}";
    }

    public int updateInventoryItemCountDown(GameObject inventoryItem, int value)
    {
        TextMeshProUGUI amountText = inventoryItem.transform.Find("amount").GetComponent<TextMeshProUGUI>();

        string currentText = amountText.text;
        int currentAmount = int.Parse(currentText.Substring(1));

        int newAmount = currentAmount - value;
        amountText.text = $"x{newAmount}";

        return newAmount;
    }

    public bool isItemInInventory(string inventoryItemTag)
    {
        foreach (GameObject item in inventoryItems)
        {
            if (item.tag == inventoryItemTag)
            {
                return true;
            }
        }
        return false;
    }

    public GameObject addNewInventoryItem(GameObject item)
    {
        // Instantiate the item as a child of the canvas
        GameObject newItem = Instantiate(item, canvasTransform);

        // Set the new item to be the first sibling in the hierarchy
        newItem.transform.SetAsFirstSibling();

        // Get the RectTransform of the new item
        RectTransform rectTransform = newItem.GetComponent<RectTransform>();

        // Set the parent of the new item to inventoryUIList using SetParent instead of parent property
        rectTransform.SetParent(inventoryUIList.transform, false);  // false retains the local position, rotation, and scale

        // Add the new item to the list of inventory items
        inventoryItems.Add(newItem);

        return newItem;
    }
}
