using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    public int maxHealth = 100;
    public Image healthBar;

    private Animator animator;
    private Rigidbody2D rb;
    private Transform player;
    private int currentHealth;
    private float lastAttackTime;
    private bool isDead = false;

    private static readonly int WalkHash = Animator.StringToHash("Walk");
    private static readonly int AttackHash = Animator.StringToHash("Attack");
    private static readonly int DieHash = Animator.StringToHash("Die");
    private static readonly int AppearHash = Animator.StringToHash("Appear");
    private static readonly int TakeHitHash = Animator.StringToHash("TakeHit");

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;
        UpdateHealthBar();

        // Trigger the appear animation when the boss spawns
        animator.SetTrigger(AppearHash);
    }

    void Update()
    {
        if (isDead) return;

        Vector2 direction = (player.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            // Move towards the player
            rb.velocity = direction * moveSpeed;
            animator.SetBool(WalkHash, true);
        }
        else
        {
            // Stop moving
            rb.velocity = Vector2.zero;
            animator.SetBool(WalkHash, false);

            // Attack if cooldown has passed
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                Attack();
            }
        }

        // Flip the sprite based on movement direction
        if (direction.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);
        }
    }

    void Attack()
    {
        animator.SetTrigger(AttackHash);
        lastAttackTime = Time.time;

        // Implement your attack logic here
        // For example, you could use a raycast or overlap circle to detect the player
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger(TakeHitHash);
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger(DieHash);
        rb.velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;

        // Implement any other death logic here (e.g., dropping items, ending the level)
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = (float)currentHealth / maxHealth;
        }
    }
}