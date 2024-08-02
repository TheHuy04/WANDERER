using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[System.Serializable]

public class SlotClass
{
    [SerializeField]private ItemClass item;
    [SerializeField]private int quatity = 0;

    public SlotClass()
    {
        item = null;
        quatity = 0;
    }
    public SlotClass(ItemClass _item, int _quality)
    {
        this.item = _item;
        this.quatity = _quality;
    }
    public ItemClass GetItem() { return this.item; }
    public int GetQuatity() { return this.quatity; }
    public void AddQuatity( int _quatity) { quatity = quatity + _quatity; }
    public void SubQuatity( int _quatity) { quatity = quatity - _quatity; }
    public void SetQuatity( int _quatity) { quatity = _quatity; }
    public void AddItem(ItemClass item, int quantity)
    {
        this.item = item;
        this.quatity = quantity;
    }

    public void RemoveItem()
    {
        this.item = null;
        this.quatity = 0;
    }
} 
