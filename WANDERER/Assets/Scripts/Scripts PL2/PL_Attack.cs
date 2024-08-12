using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PL_Attack : MonoBehaviour
{
    private Animator anm;

    public Transform attackPoint;
    public float attackRange;
    public LayerMask enemyLayers;
    public int attackDamage;

    public float attackRate = 2f;
    float nextAttackTime = 0f;
    void Start()
    {
        anm = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                Attack1();
                nextAttackTime = Time.time +1f * attackRate;
            }
        }
    }
    void Attack1()
    {
        anm.SetTrigger("Attack1");
        Collider2D[] hitenemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange,enemyLayers);

        foreach(Collider2D enemy in hitenemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
