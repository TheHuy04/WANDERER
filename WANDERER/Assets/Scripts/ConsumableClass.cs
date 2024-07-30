
using UnityEngine;

[CreateAssetMenu(fileName = "New Consum", menuName = "Item/Consum")]
public class ConsumableClass : ItemClass
{
    [Header("Consumable")]
    public int heathRecovery;//an do hoi mau

    public override ItemClass GetItem() { return this; }
    public override ToolClass GetTool() { return null; }
    public override MiscClass GetMisc() { return null; }
    public override ConsumableClass GetConsumable() { return this; }
}
