using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject slotHolder;
    [SerializeField] private SlotClass[] items;
    [SerializeField] private SlotClass[] startingItems;

    [SerializeField] private SlotClass movingSlot;
    [SerializeField] private SlotClass originalSlot;
    [SerializeField] private SlotClass tempSlot;

    public Image itemCursor;
    public GameObject inventoryUI;

    [SerializeField] private GameObject[] slots;
    public bool isMoving;
    private bool isInventoryOpen = false;

    private Pickup nearbyPickup; // Vật phẩm gần nhất để nhặt

    void Start()
    {
        slots = new GameObject[slotHolder.transform.childCount];
        items = new SlotClass[slots.Length];
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotHolder.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = new SlotClass();
        }
        for (int i = 0; i < startingItems.Length; i++)
        {
            items[i] = startingItems[i];
        }
        originalSlot = new SlotClass();
        movingSlot = new SlotClass();
        tempSlot = new SlotClass();

        ReFreshUI();
        inventoryUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isMoving)
            {
                EndMove();
            }
            else
            {
                BeginMove();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            BeginSplit();
        }

        if (Input.GetKeyDown(KeyCode.E) && nearbyPickup != null)
        {
            PickUpItem();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }

        if (isMoving)
        {
            itemCursor.enabled = true;
            itemCursor.transform.position = Input.mousePosition;
            itemCursor.sprite = movingSlot.GetItem().itemIcon;
        }
        else
        {
            itemCursor.enabled = false;
            itemCursor.sprite = null;
        }
    }

    private void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryUI.SetActive(isInventoryOpen);
    }

    private void ReFreshUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            try
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].GetItem().itemIcon;
                if (!items[i].GetItem().isStackable)
                {
                    slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                }
                else
                {
                    slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = items[i].GetQuatity() + "";
                }
            }
            catch
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            }
        }
    }

    public void AddItem(ItemClass item, int quantity)
    {
        Debug.Log("Adding item: " + item.name + ", quantity: " + quantity);
        SlotClass slot = ContainsItem(item);
        if (slot != null && slot.GetItem().isStackable)
        {
            slot.AddQuatity(quantity);
            Debug.Log("Increased quantity of existing item: " + item.name);
        }
        else
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].GetItem() == null)
                {
                    items[i].AddItem(item, quantity);
                    Debug.Log("Added new item to inventory: " + item.name);
                    break;
                }
            }
        }
        ReFreshUI();
    }


    public void RemoveItem(ItemClass item, int quantity)
    {
        SlotClass temp = ContainsItem(item);
        if (temp != null)
        {
            if (temp.GetQuatity() > 1)
            {
                temp.SubQuatity(quantity);
            }
            else
            {
                int slotToRemoveIndex = 0;
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].GetItem() == item)
                    {
                        slotToRemoveIndex = i;
                        break;
                    }
                }
                items[slotToRemoveIndex].RemoveItem();
            }
        }
        else
        {
            return;
        }

        ReFreshUI();
    }

    private SlotClass ContainsItem(ItemClass item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].GetItem() == item)
            {
                return items[i];
            }
        }
        return null;
    }

    private SlotClass GetClosestSlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (Vector2.Distance(slots[i].transform.position, Input.mousePosition) <= 64)
            {
                return items[i];
            }
        }
        return null;
    }

    private void BeginMove()
    {
        originalSlot = GetClosestSlot();

        if (originalSlot == null || originalSlot.GetItem() == null) return;

        movingSlot.AddItem(
            originalSlot.GetItem(),
            originalSlot.GetQuatity());
        originalSlot.RemoveItem();

        isMoving = true;
        ReFreshUI();
        return;
    }

    private void BeginSplit()
    {
        originalSlot = GetClosestSlot();

        if (originalSlot == null || originalSlot.GetItem() == null) return;
        if (originalSlot.GetQuatity() <= 1)
        {
            return;
        }

        movingSlot.AddItem(originalSlot.GetItem(), Mathf.CeilToInt(originalSlot.GetQuatity() / 2f));

        originalSlot.SubQuatity(Mathf.CeilToInt(originalSlot.GetQuatity() / 2f));

        isMoving = true;
        ReFreshUI();
        return;
    }

    private void EndMove()
    {
        SlotClass targetSlot = GetClosestSlot();

        if (targetSlot == null)
        {
            AddItem(movingSlot.GetItem(), movingSlot.GetQuatity());
        }
        else
        {
            if (targetSlot.GetItem() != null)
            {
                if (targetSlot.GetItem() == movingSlot.GetItem())
                {
                    if (targetSlot.GetItem().isStackable)
                    {
                        int itemMaxStack = targetSlot.GetItem().maxStackQuanity;
                        int count = targetSlot.GetQuatity() + movingSlot.GetQuatity();

                        if (count > itemMaxStack)
                        {
                            int remain = count - itemMaxStack;
                            targetSlot.SetQuatity(itemMaxStack);
                            movingSlot.SetQuatity(remain);

                            isMoving = true;
                            ReFreshUI();
                            return;
                        }
                        else
                        {
                            targetSlot.AddQuatity(movingSlot.GetQuatity());
                            movingSlot.RemoveItem();
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    tempSlot.AddItem(targetSlot.GetItem(), targetSlot.GetQuatity());
                    targetSlot.AddItem(movingSlot.GetItem(), movingSlot.GetQuatity());
                    movingSlot.AddItem(tempSlot.GetItem(), tempSlot.GetQuatity());
                    tempSlot.RemoveItem();

                    ReFreshUI();
                    return;
                }
            }
            else
            {
                targetSlot.AddItem(movingSlot.GetItem(), movingSlot.GetQuatity());
                movingSlot.RemoveItem();
            }
        }

        isMoving = false;
        ReFreshUI();
        return;
    }

    public void SetNearbyPickup(Pickup pickup)
    {
        nearbyPickup = pickup;
    }

    private void PickUpItem()
    {
        if (nearbyPickup != null)
        {
            nearbyPickup.OnPickup();
            nearbyPickup = null;
        }
    }
}
