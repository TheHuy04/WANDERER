using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth;
    private int currentHealth;
    public HealthBar healthBar;

    private Animator anm;
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxhealth(maxHealth);
        anm = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        anm.SetTrigger("Hit");

        healthBar.SetHealth(currentHealth);
        if(currentHealth <= 0)
        {
            Die();
        }

    }
    void Die()
    {
        anm.SetBool("Die",true);
    }
    public void Heal(int amount)
    {
        currentHealth += amount;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthBar.SetMaxhealth(currentHealth);
    }

}
