using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour
{
    public float walkAcceleration = 3f;
    public float maxSpeed = 3f;
    public float walkStopRate = 0.05f;
    public DetectionZone attackZone;
    public DetectionZone cliffDetectionZone;
    public DetectionZone playerRangeZone; // New collider for player range
    public float targetFollowDistance = 5f; // Distance to keep from the target
    public float attackRange = 1f; // Distance to trigger attack

    private Rigidbody2D rb;
    private TouchingDirections touchingDirections;
    private Animator animator;
    private DamageAble damageAble;
    private Transform target;
    private Transform player;

    public enum WalkableDirection { Right, Left }
    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;

    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set
        {
            if (_walkDirection != value)
            {
                // Direction flip
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                walkDirectionVector = (value == WalkableDirection.Right) ? Vector2.right : Vector2.left;
            }
            _walkDirection = value;
        }
    }

    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool CanMove
    {
        get { return animator.GetBool(AnimationStrings.canMove); }
    }

    public float AttackCooldown
    {
        get { return animator.GetFloat(AnimationStrings.attackCooldown); }
        private set { animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0)); }
    }

    private bool _hasTarget = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        damageAble = GetComponent<DamageAble>();
    }

    private void Update()
    {
        UpdateTargetDetection();

        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (touchingDirections.IsGrounded && touchingDirections.IsOnWall)
        {
            FlipDirection();
        }

        if (!damageAble.lockVelocity)
        {
            if (CanMove && touchingDirections.IsGrounded)
            {
                if (player != null) // Check if player is in range
                {
                    float distanceToPlayer = Vector2.Distance(transform.position, player.position);
                    if (distanceToPlayer <= attackRange && AttackCooldown < 1e-05f)
                    {
                        Attack(); // Trigger attack
                    }
                    else
                    {
                        MoveTowardsTarget(player.transform); // Move towards player
                    }
                }
                else if (HasTarget)
                {
                    MoveTowardsTarget(target); // Move towards other target if player not in range
                }
                else
                {
                    // Default walking behavior when no target
                    rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + (walkAcceleration * walkDirectionVector.x * Time.fixedDeltaTime), -maxSpeed, maxSpeed), rb.velocity.y);
                }
            }
            else
            {
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
            }
        }
    }

    private void UpdateTargetDetection()
    {
        if (attackZone.detectedCollinders.Count > 0)
        {
            HasTarget = true;
            target = attackZone.detectedCollinders[0].transform;
        }
        else
        {
            HasTarget = false;
            target = null;
        }

        if (playerRangeZone.detectedCollinders.Count > 0)
        {
            player = playerRangeZone.detectedCollinders[0].transform;
        }
        else
        {
            player = null;
        }
    }

    private void MoveTowardsTarget(Transform targetTransform)
    {
        if (targetTransform != null)
        {
            Vector2 directionToTarget = (targetTransform.position - transform.position).normalized;
            float distanceToTarget = Vector2.Distance(transform.position, targetTransform.position);

            // Determine walk direction based on target position
            WalkDirection = (directionToTarget.x > 0) ? WalkableDirection.Right : WalkableDirection.Left;

            if (distanceToTarget > targetFollowDistance)
            {
                // Move towards target
                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + (walkAcceleration * directionToTarget.x * Time.fixedDeltaTime), -maxSpeed, maxSpeed), rb.velocity.y);
            }
            else
            {
                // Stop when close enough to target
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
            }
        }
    }

    private void Attack()
    {
        // Implement your attack logic here
         // Example: Trigger attack animation
        AttackCooldown = 2f; // Set cooldown time (example value)
    }

    private void FlipDirection()
    {
        WalkDirection = (WalkDirection == WalkableDirection.Right) ? WalkableDirection.Left : WalkableDirection.Right;
    }

    public void OnHit(int damage, Vector2 knockBack)
    {
        rb.velocity = new Vector2(knockBack.x, rb.velocity.y + knockBack.y);
    }

    public void OnCliffDetected()
    {
        if (touchingDirections.IsGrounded)
        {
            FlipDirection();
        }
    }
}
