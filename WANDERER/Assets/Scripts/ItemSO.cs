using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public StatToChange statToChange = new StatToChange();
    public int amountToChangeStat;
    public AttributeToChange attributeToChange = new AttributeToChange();
    public int amountToChangeAttribute;

    public void UseItem()
    {
        if(statToChange == StatToChange.health)
        {
            //DamageAble damageAble = GameObject.Find("")
        }
        if(statToChange == StatToChange.mana)
        {

        }
    }




    public enum StatToChange
    {
        None,
        health,
        mana,
        stamina
    };
    public enum AttributeToChange
    {
        None,
        strength,
        defense,
        intelligence,
        agility
    };
}
