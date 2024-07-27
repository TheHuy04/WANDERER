using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator anm;
    public int maxHealth;
    int currentHealth;

    public int attackDamage;
    public float attackRange = 0.5f;
    public float attackRate = 2f;
    public Transform attackPoint;
    public LayerMask playerLayer;

    public float nexAttackTime = 1.5f;
    public float moveSpeed = 2f;
    public float patrolRange = 5f;

    private Vector2 patrolCenter;
    private Vector2 patrolTarget;
    private bool movingRght = true;

    void Start()
    {
        anm = GetComponent<Animator>();
        currentHealth = maxHealth;
        SetPatrolTarget();
    }

    // Update is called once per frame
    private void Update()
    {
        Patrol();
        if(Time.time >= nexAttackTime)
        {
            Collider2D player = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
            if (player != null)
            {
                Attack(player.GetComponent<Health>());
                nexAttackTime = Time.time + 2f / attackRate;
            }
        }
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        anm.SetTrigger("Hit");
        

        if(currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        anm.SetBool("Die", true);
        GetComponent<Collider2D>().enabled = false; 
        this.enabled = false;
    }
    void Patrol()
    {
        transform.position = Vector2.MoveTowards(transform.position, patrolTarget, moveSpeed * Time.deltaTime);
        anm.SetBool("Run", true);
        if (Vector2.Distance(transform.position, patrolTarget) < 0.2f)
        {
            movingRght = !movingRght;
            SetPatrolTarget();
            Flip();
        }
    }
    void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
    void SetPatrolTarget()
    {
        float targetX = movingRght ? patrolCenter.x + patrolRange : patrolCenter.x - patrolRange;
        patrolTarget = new Vector2(targetX, transform.position.y);
    }
    void Attack(Health playerHealth)
    {
        if (playerHealth != null)
        {
            anm.SetTrigger("Attack");
            playerHealth.TakeDamage(attackDamage);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
