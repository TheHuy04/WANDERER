using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rock : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool hasFallen = false;
    public int attackDamage = 10;
    public Vector2 knockBack = Vector2.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // Ban đầu không có trọng lực

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        DamageAble damageAble = collision.GetComponent<DamageAble>();
        if (damageAble != null)
        {
            Vector2 deliveredKnockBack = transform.parent.localScale.x > 0 ? knockBack : new Vector2(-knockBack.x, knockBack.y);
            // Hit the target
            bool gotHit = damageAble.Hit(attackDamage, deliveredKnockBack);

            if (gotHit)
                Debug.Log(collision.name + "hit for" + attackDamage);
        }
        if (collision.CompareTag("Player") && !hasFallen)
        {
            rb.gravityScale = 1; // Bắt đầu rơi khi nhân vật đi qua
            hasFallen = true;
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject); // Phá hủy đối tượng khi chạm đất
        }
    }
}
