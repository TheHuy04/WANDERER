using UnityEngine;
using UnityEngine.Events;

public class DamageAble : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent damageableDeath;
    public UnityEvent<int, int> healthChanged;

    private Animator animator;
    private PlayerRespawnn playerRespawnn;

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

    // Returns whether the damageable took damage or not
    public bool Hit(int damage, Vector2 knockBack)
    {
        if (IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;

            // Notify other subscribed components that the damageable was hit
            animator.SetTrigger(AnimationStrings.hitTrigger);
            lockVelocity = true;
            damageableHit?.Invoke(damage, knockBack);
            CharacterEvents.characterDamaged.Invoke(gameObject, damage);

            return true;
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
    }
}
