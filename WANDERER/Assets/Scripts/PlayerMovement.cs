using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using System;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float airSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private int maxJumps = 2; // Maximum number of jumps allowed
    [SerializeField] private AudioClip jumpSound;
    private bool isAttack3Playing = false;
    private bool isSpAttackPlaying = false;
    private int jumpCount; // Current number of jumps performe
    private Rigidbody2D rb;
    private Animator animator;
    private CapsuleCollider2D capsuleCollider;
    Vector2 moveInput;
    TouchingDirections touchingDirections;
    DamageAble damageAble;
    ManaAble manaAble;


    public float currentMoveSpeed
    {
        get
        {
            if (CanMove)
            {
                if (IsMoving && !touchingDirections.IsOnWall)
                {
                    if (touchingDirections.IsGrounded)
                    {
                        if (IsRunning)
                        {
                            return runSpeed;
                        }
                        else
                        {
                            return walkSpeed;
                        }
                    }
                    else
                    {
                        return airSpeed;
                    }
                }
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }

        }
    }

    

    [SerializeField]
    private bool _isMoving = false;
    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }

    [SerializeField]
    private bool _isRunning = false;
    public bool IsRunning
    {
        get
        {
            return _isRunning;
        }
        private set
        {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }


    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public bool _isFacingRight = true;
    public bool IsFacingRight
    {
        get
        {
            return _isFacingRight;
        }
        private set
        {
            if (IsFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }

            _isFacingRight = value;
        }
    }

    public bool IsAlive
    {
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }



    private void Awake()
    {
        //grab references for rigidbody and animator from object
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageAble = GetComponent<DamageAble>();
        manaAble = GetComponent<ManaAble>();

    }


    private void FixedUpdate()
    {
        if (!damageAble.lockVelocity)
            rb.velocity = new Vector2(moveInput.x * currentMoveSpeed, rb.velocity.y);

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);

        if (touchingDirections.IsGrounded)
        {
            ResetJumpCount(); // Reset jump count when grounded
        }
    }

    public void onAttack3(InputAction.CallbackContext context)
    {
        if (context.started && !isAttack3Playing)
        {
            int skillManaCost = 20;
            if (manaAble.CanCastSkill(skillManaCost))
            {
                isAttack3Playing = true;
                manaAble.UseMana(skillManaCost);
                Debug.Log("Hasagi triggered");
                animator.SetTrigger(AnimationStrings.attack3Trigger);
                StartCoroutine(ResetAttack3Flag());
            }
            else
            {
                Debug.Log("Not enough mana to cast skill");
            }
        }
    }

    public void onAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.attack1Trigger);
        }
    }

    public void onSpAttack(InputAction.CallbackContext context)
    {
        if (context.started && !isSpAttackPlaying)
        {
            int skillManaCost = 40;
            if (manaAble.CanCastSkill(skillManaCost))
            {
                isSpAttackPlaying = true;
                manaAble.UseMana(skillManaCost);
                Debug.Log("AttackSP triggered");
                animator.SetTrigger(AnimationStrings.SpAtkTrigger);
                StartCoroutine(PerformSpAttack());
            }
            else
            {
                Debug.Log("Not enough mana to cast skill");
            }
        }
    }




    public void onRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }

    public void onMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;

            SetFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }

    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    public void onJump(InputAction.CallbackContext context)
    {
        if (context.started && jumpCount < maxJumps && CanMove && SoundManager.instance != null)
        {
            SoundManager.instance.PlaySound(jumpSound);
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            jumpCount++;
        }
    }

    public void OnHit(int damage, Vector2 knockBack)
    {
        rb.velocity = new Vector2(knockBack.x, rb.velocity.y + knockBack.y);
    }

    private void ResetJumpCount()
    {
        jumpCount = 0;
    }

    private IEnumerator ResetAttack3Flag()
    {
        // Wait for the attack animation to finish
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        isAttack3Playing = false;
    }

    private IEnumerator PerformSpAttack()
    {
        damageAble.SetSpecialInvulnerability(true);

        // Wait for the special attack animation to finish
        float animationDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationDuration);

        damageAble.SetSpecialInvulnerability(false);
        isSpAttackPlaying = false;
    }
}