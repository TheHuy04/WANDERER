using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SP : MonoBehaviour
{
    public Transform player;
    public float followSpeed;
    public float followDistance;

    public float attackRange;       
    public float attackCooldown;    
    public int damageAttack;              

    private Animator anm;           
    private float lastAttackTime = 0f;

    public Vector2 knockBack = Vector2.zero;

    void Start()
    {
        anm = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Tính khoảng cách giữa trợ thủ và nhân vật chính
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Di chuyển về phía nhân vật chính nếu cần thiết
        if (distanceToPlayer > followDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.position, followSpeed * Time.deltaTime);
            anm.SetBool("isWalking", true);
        }
        else
        {
            anm.SetBool("isWalking", false);
        }

        // Kiểm tra xem có kẻ địch trong phạm vi tấn công không
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, attackRange, LayerMask.GetMask("Enemy"));
        if (enemiesInRange.Length > 0)
        {
            // Nếu có kẻ địch trong phạm vi và thời gian hồi tấn công đã hết, thì tấn công
            if (Time.time > lastAttackTime + attackCooldown)
            {
                Attack(enemiesInRange[0].gameObject); // Tấn công kẻ địch đầu tiên trong danh sách
                lastAttackTime = Time.time;
            }
        }
    }

    private void Attack(GameObject enemy)
    {
        // Chuyển sang trạng thái tấn công trong Animator
        anm.SetTrigger("Attack");

        // Gây sát thương cho kẻ địch (giả định kẻ địch có script tên "Enemy" với hàm "TakeDamage(int amount)")
        DamageAble damageable = enemy.GetComponent<DamageAble>();
        if (damageable != null)
        {
            Vector2 deliveredKnockBack = transform.parent.localScale.x > 0 ? knockBack : new Vector2(-knockBack.x, knockBack.y);
            bool takehit = damageable.Hit(damageAttack, deliveredKnockBack);
            if (takehit)
                Debug.Log(enemy.name + "hit for" + damageAttack);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Vẽ phạm vi tấn công trong Scene View để dễ dàng điều chỉnh
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
    

