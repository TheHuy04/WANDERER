using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    private bool menuActivated;
    public ItemSlot[] itemSlot;
    public ItemSO[] itemSos;

    void Start()
    {
        // Initialize menuActivated to false, ensuring the game isn't paused at start
        menuActivated = false;
        InventoryMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (menuActivated)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
            }
        }
    }

    private void OpenInventory()
    {
        InventoryMenu.SetActive(true);
        menuActivated = true;
        Time.timeScale = 0f; // Pause the game
    }

    private void CloseInventory()
    {
        InventoryMenu.SetActive(false);
        menuActivated = false;
        Time.timeScale = 1f; // Resume the game
    }

    public void UseItem(GameObject player, string itemName)
    {
        for (int i = 0; i < itemSos.Length; i++)
        {
            if (itemSos[i].itemName == itemName)
            {
                itemSos[i].UseItem(player);
            }
        }
    }

    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDiscription)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].isFull == false && itemSlot[i].name == name || itemSlot[i].quantity == 0)
            {
                int leftOverItems = itemSlot[i].AddItem(itemName, quantity, itemSprite, itemDiscription);
                if (leftOverItems > 0)
                {
                    leftOverItems = AddItem(itemName, leftOverItems, itemSprite, itemDiscription);
                }
                return leftOverItems;
            }
        }
        return quantity;
    }

    public void DeselecAllSlots()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].selectedShader.SetActive(false);
            itemSlot[i].thisItemSelected = false;
        }
    }
}
