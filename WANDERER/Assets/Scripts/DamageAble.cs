using UnityEngine;
using UnityEngine.Events;

public class DamageAble : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent damageableDeath;

    Animator animator;

    [SerializeField]
    private int _maxHealth = 100;
    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set 
        { 
            _maxHealth = value;
        }
    }

    [SerializeField]
    private int _health = 100;

    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;

            //if health drops below 0, character is no longer alive
            if(_health <= 0)
            {
                IsAlive = false;
            }
        }
    }

    [SerializeField]
    private bool _isAlive = true;

    [SerializeField]
    private bool isInvicible = false;

   

    private float timeSinceHit = 0;
    public float invincibilityTime = 0.25f;

    public bool IsAlive 
    {   get 
        { 
            return _isAlive; 
        }
        set
        {
            _isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
            Debug.Log("IsAlive set " + value);

            if(value == false )
            {
                damageableDeath.Invoke();
            }
        }
    }

    public bool lockVelocity
    {
        get
        {
            return animator.GetBool(AnimationStrings.lockVelocity);
        }
        set
        {
            animator.SetBool(AnimationStrings.lockVelocity, value);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(isInvicible)
        {
            if(timeSinceHit > invincibilityTime)
            {
                // remove invincibility
                isInvicible = false;
                timeSinceHit = 0;
            }

            timeSinceHit += Time.deltaTime;
        }

        
    }

    // Returns whenever the damageable took damage or not
    public bool Hit(int damage, Vector2 knockBack)
    {
        if(IsAlive && !isInvicible)
        {
            Health -= damage;
            isInvicible = true;

            // Notify other subcribed components that the damageable was hit to handle the knockback and such
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
        if(IsAlive && Health < MaxHealth)
        {
            int maxHeal = Mathf.Max(MaxHealth - Health, 0);
            int actualHeal = Mathf.Min(maxHeal, healthRestore);
            Health += actualHeal;
            CharacterEvents.characterHealed(gameObject, actualHeal);
            return true;
        }

        return false;
    }


    
}
