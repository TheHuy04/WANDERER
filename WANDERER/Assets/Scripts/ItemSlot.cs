using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    // item data
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;
    public string itemDescription;

    //itemslot
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemImage;

    //item desription slot
    public Image itemDescriptionImage;
    public TMP_Text ItemDescriptionNameText;
    public TMP_Text ItemDescriptionText;
    public Sprite emptySprite;

    [SerializeField] private int maxNumberOfItems;

    public GameObject selectedShader;
    public bool thisItemSelected;

    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
    {
        //chefck to see if the slot is already full
        if(isFull)
        {
            return quantity;
        }
        //update name
        this.itemName = itemName;

        //update image
        this.itemSprite = itemSprite;
        itemImage.sprite = itemSprite;

        //update des    
        this.itemDescription = itemDescription;

        //update quantity
        this.quantity += quantity;
        if(this.quantity >= maxNumberOfItems)
        {
            quantityText.text = quantity.ToString();
            quantityText.enabled = true;
            isFull = true;

            //return leftOvers
            int extraItems = this.quantity - maxNumberOfItems;
            this.quantity = maxNumberOfItems;
            return extraItems;
        }
        //update quantity text 
        quantityText.text = this.quantity.ToString();
        quantityText.enabled = true;

        return 0;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnLeftClick();
        }
    }
    public void OnLeftClick()
    {
        if (thisItemSelected)
        {
            inventoryManager.UseItem(itemName);
            this.quantity -= 1;
            quantityText.text = this.quantity.ToString();
            if (this.quantity <= 0)
            {
                EmptySlot();
            }

        }
        else
        {
            inventoryManager.DeselecAllSlots();
            selectedShader.SetActive(true);
            thisItemSelected = true;
            ItemDescriptionNameText.text = itemName;
            ItemDescriptionText.text = itemDescription;
            itemDescriptionImage.sprite = itemSprite;
            if (itemDescriptionImage.sprite == null)
            {
                itemDescriptionImage.sprite = emptySprite;
            }
        }
    }

    private void EmptySlot()
    {
        quantityText.enabled = false;
        itemImage.sprite = emptySprite;
        ItemDescriptionNameText.text = "";
        ItemDescriptionText.text = "";
        itemDescriptionImage.sprite = emptySprite;
    }
    public void OnRightClick()
    {

    }
}
