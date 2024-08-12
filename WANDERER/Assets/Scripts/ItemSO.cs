using System;
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

    public void UseItem(GameObject target)
    {
        if(statToChange == StatToChange.Health)
        {
            DamageAble damageable = target.GetComponent<DamageAble>();
            if (damageable != null)
            {
                if (damageable)
                {
                    damageable.ChangeHealth(amountToChangeStat);
                }
            }
            else
            {
                Debug.LogWarning("DamageAble component not found on target!");
            }
        }
        if(statToChange == StatToChange.Mana)
        {
            ManaAble manaAble = target.GetComponent<ManaAble>();
            if(manaAble != null)
            {
                manaAble.ChangeMana(amountToChangeStat);
                if (manaAble)
                {
                    manaAble.ChangeMana(amountToChangeStat);
                }

            }
            else
            {
                Debug.LogWarning("DamageAble component not found on target!");
            }
        }
    }


    public enum StatToChange
    {
        None,
        Health,
        Mana,
        Stamina
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
