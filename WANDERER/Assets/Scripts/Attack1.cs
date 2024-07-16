using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack1 : MonoBehaviour
{
   
    public int attackDamage = 10;
    public Vector2 knockBack = Vector2.zero;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // See if it can be hit
        DamageAble damageAble = collision.GetComponent<DamageAble>();
        
        if (damageAble != null)
        {
            Vector2 deliveredKnockBack = transform.parent.localScale.x > 0 ? knockBack : new Vector2(-knockBack.x, knockBack.y);
            // Hit the target
            bool gotHit = damageAble.Hit(attackDamage, deliveredKnockBack);

            if (gotHit)
                Debug.Log(collision.name + "hit for" + attackDamage);
        }
    }
}
