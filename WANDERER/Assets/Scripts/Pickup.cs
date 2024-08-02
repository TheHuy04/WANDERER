using UnityEngine;

public class Pickup : MonoBehaviour
{
    public ItemClass item;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player entered pickup range of: " + item.name);
            InventoryManager inventory = collision.GetComponent<InventoryManager>();
            if (inventory != null)
            {
                inventory.SetNearbyPickup(this);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player exited pickup range of: " + item.name);
            InventoryManager inventory = collision.GetComponent<InventoryManager>();
            if (inventory != null)
            {
                inventory.SetNearbyPickup(null);
            }
        }
    }

    public void OnPickup()
    {
        Debug.Log("Picked up: " + item.name);
        InventoryManager inventory = FindObjectOfType<InventoryManager>();
        if (inventory != null)
        {
            inventory.AddItem(item, 1);
            Debug.Log("Item added to inventory: " + item.name);
            Destroy(gameObject); // Hủy vật phẩm sau khi nhặt
            Debug.Log("Item destroyed: " + item.name);
        }
        else
        {
            Debug.Log("InventoryManager not found.");
        }
    }
}
