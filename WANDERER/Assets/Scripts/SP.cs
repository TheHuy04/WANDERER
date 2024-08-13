using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SP : MonoBehaviour
{
    public Transform player;
    public float followSpeed;
    public float followDistance;

    public Transform attackPoint;    // Vị trí đặt vùng tấn công
    public Vector2 attackSize;       // Kích thước của vùng tấn công hình vuông
    public float attackCooldown;
    public int damageAttack;
    private DamageAble damageAble;

    private Animator anm;
    private float lastAttackTime = 0f;

    public Vector2 knockBack = Vector2.zero;

    private bool isFacingRight = true;
    private Golem golem;

    private Rigidbody2D rb;

    private void Awake()
    {
        damageAble = GetComponent<DamageAble>();
        rb = GetComponent<Rigidbody2D>();
        golem = GetComponent<Golem>();
    }
    void Start()
    {
        anm = GetComponent<Animator>();
    }

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
            if ((direction.x > 0 && !isFacingRight) || (direction.x < 0 && isFacingRight))
            {
                Flip();
            }
        }
        else
        {
            anm.SetBool("isWalking", false);
        }

        // Kiểm tra xem có kẻ địch trong phạm vi tấn công không
        Collider2D[] enemiesInRange = Physics2D.OverlapBoxAll(attackPoint.position, attackSize, 0f, LayerMask.GetMask("Enemy1()")); // Sử dụng OverlapBoxAll với kích thước hình vuông
        if (enemiesInRange.Length > 0)
        {
            // Nếu có kẻ địch trong phạm vi và thời gian hồi tấn công đã hết, thì tấn công
            if (Time.time > lastAttackTime + attackCooldown)
            {
                foreach (Collider2D enemy in enemiesInRange)
                {
                    Attack(enemy.gameObject);
                }
                lastAttackTime = Time.time;
            }
        }
    }

    private void Flip()
    {
        // Đảo ngược hướng của nhân vật
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        // Đảo ngược hướng của attackPoint nếu cần
        Vector3 attackPointScale = attackPoint.localScale;
        attackPointScale.x *= -1;
        attackPoint.localScale = attackPointScale;
    }

    private void Attack(GameObject enemy)
    {
        // Chuyển sang trạng thái tấn công trong Animator
        anm.SetTrigger("Attack");

        // Gây sát thương cho kẻ địch
        DamageAble damageable = enemy.GetComponent<DamageAble>();
        if (damageable != null)
        {
            Vector2 deliveredKnockBack = transform.localScale.x > 0 ? knockBack : new Vector2(-knockBack.x, knockBack.y);
            bool takehit = damageable.Hit(damageAttack, deliveredKnockBack);
            if (takehit)
            {
                Debug.Log(enemy.name + " hit for " + damageAttack);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Vẽ vùng tấn công hình vuông trong Scene View để dễ dàng điều chỉnh
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPoint.position, attackSize); // Vẽ hình vuông dựa trên attackPoint.position
    }
    public void Golem(GameObject gl, int damage, Vector2 knockBack)
    {
        Golem golem = gl.GetComponent<Golem>();
        if (golem != null)
        {
            golem.OnHit(damage, knockBack);
        }
    }

    //private void onHitSP(GameObject golem, Vector2 knockBack,int damage)
    //{
    //    Golem goLem = golem.GetComponent<Golem>();
    //    rb.velocity = new Vector2(knockBack.x, rb.velocity.y + knockBack.y);
    //    if (golem != null)
    //    {
    //        golem = goLem.OnHit(damageAttack, knockBack); 

    //}

}
