using UnityEngine;
using UnityEngine.Events;

public class DamageAble : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent damageableDeath;
    public UnityEvent<int, int> healthChanged;
    [SerializeField] private HealthBarMonsters healthBar;
    [SerializeField] private BossHealthBar healthBar2;

    private Animator animator;
    private PlayerRespawnn playerRespawnn;

    private bool isSpecialInvulnerable = false;

    private void Start()
    {
        if (healthBar == null)
        {
            Debug.LogWarning("HealthBar not assigned to DamageAble script!");
        }
        else
        {
            healthBar.SetHealth(Health, MaxHealth);
        }

        if (healthBar2 == null)
        {
            Debug.LogWarning("BossHealthBar not assigned to DamageAble script!");
        }
        else
        {
            healthBar2.SetHealth(Health, MaxHealth);
            healthBar2.Show();
        }
    }

    [SerializeField]
    private int _maxHealth = 100;
    public int MaxHealth
    {
        get => _maxHealth;
        set => _maxHealth = value;
    }

    [SerializeField]
    private int _health = 100;
    public int Health
    {
        get => _health;
        set
        {
            _health = value;
            healthChanged?.Invoke(_health, MaxHealth);

            // Update the boss health bar
            if (healthBar2 != null)
            {
                healthBar2.SetHealth(_health, MaxHealth);
            }

            // Check if health drops below 0 to set IsAlive status
            if (_health <= 0)
            {
                IsAlive = false;
            }
        }
    }

    [SerializeField]
    private bool _isAlive = true;
    public bool IsAlive
    {
        get => _isAlive;
        set
        {
            if (_isAlive != value)
            {
                _isAlive = value;
                animator.SetBool(AnimationStrings.isAlive, value);
                Debug.Log("IsAlive set " + value);

                if (!value)
                {
                    damageableDeath.Invoke();
                    healthBar2?.Hide();
                }
            }
        }
    }

    [SerializeField]
    private bool isInvincible = false;

    private float timeSinceHit = 0;
    public float invincibilityTime = 0.25f;

    public bool lockVelocity
    {
        get => animator.GetBool(AnimationStrings.lockVelocity);
        set => animator.SetBool(AnimationStrings.lockVelocity, value);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerRespawnn = GetComponent<PlayerRespawnn>();
    }

    private void Update()
    {
        if (isInvincible)
        {
            timeSinceHit += Time.deltaTime;
            if (timeSinceHit > invincibilityTime)
            {
                // Remove invincibility
                isInvincible = false;
                timeSinceHit = 0;
            }
        }
    }

    public void SetSpecialInvulnerability(bool invulnerable)
    {
        isSpecialInvulnerable = invulnerable;
    }

    // Returns whether the damageable took damage or not
    public bool Hit(int damage, Vector2 knockBack)
    {
        if (IsAlive && !isInvincible && !isSpecialInvulnerable)
        {
                if (IsAlive && !isInvincible)
            {
                Health -= damage;
                isInvincible = true;

                if (healthBar != null)
                {
                    healthBar.SetHealth(Health, MaxHealth);
                    healthBar.Show();
                }

                if (healthBar2 != null)
                {
                    healthBar2.SetHealth(Health, MaxHealth);
                    healthBar2.Show();
                }

                // Notify other subscribed components that the damageable was hit
                animator.SetTrigger(AnimationStrings.hitTrigger);
                lockVelocity = true;
                damageableHit?.Invoke(damage, knockBack);
                CharacterEvents.characterDamaged.Invoke(gameObject, damage);

                return true;
            }
                return false;
        }
        return false;
    }

    public bool Heal(int healthRestore)
    {
        if (IsAlive && Health < MaxHealth)
        {
            int maxHeal = Mathf.Max(MaxHealth - Health, 0);
            int actualHeal = Mathf.Min(maxHeal, healthRestore);
            Health += actualHeal;
            CharacterEvents.characterHealed.Invoke(gameObject, actualHeal);
            return true;
        }

        return false;
    }

    public void Respawn()
    {
        Health = MaxHealth;
        IsAlive = true;
        isInvincible = false;
        animator.Play("Idle"); // Ensure "Idle" animation exists in the Animator

        if (healthBar2 != null)
        {
            healthBar2.SetHealth(Health, MaxHealth);
            healthBar2.Show();
        }
    }
}
