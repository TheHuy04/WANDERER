
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/New Item")]
public abstract class ItemClass : ScriptableObject
{
    public string itemName = "New Item";
    public Sprite itemIcon = null;
    public bool isStackable = true;
    public int maxStackQuanity = 99;

    public abstract ItemClass GetItem();
    public abstract ToolClass GetTool();
    public abstract MiscClass GetMisc();
    public abstract ConsumableClass GetConsumable();
}
