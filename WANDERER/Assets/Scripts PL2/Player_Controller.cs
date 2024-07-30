using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    public float speed;
    public float jump;
    private float h_Move;

    private Animator anm;
    private Rigidbody2D rb;

    private bool isFacingRight = true;
    private bool nhay1L;

    public float roll;
    private bool isRolling;
    public float rollTime;
    public float rollCooldown;
    private bool canRoll = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anm = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        h_Move = Input.GetAxis("Horizontal");
        
        if (!isRolling)
        {
            rb.velocity = new Vector2(h_Move * speed, rb.velocity.y);
            Flip();
        }
        if(h_Move == 0)
        {
            anm.SetBool("Run", false);
        }   
        else
        {
            anm.SetBool("Run", true);
        }

        if(Input.GetKeyDown(KeyCode.Space) && nhay1L)
        {
            rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
            nhay1L = false;
            anm.SetBool("Jump", true);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && canRoll && !isRolling)
        {
            Roll();
        }
    }
    private void Flip()
    {
        if(isFacingRight && h_Move < 0 || !isFacingRight && h_Move > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x = scale.x * -1;
            transform.localScale = scale;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            nhay1L = true;
            anm.SetBool("Jump", false);
        }
    }
    void Roll()
    {
        float rollDirection = isFacingRight ? 1f : -1f;
        rb.velocity = new Vector2(roll * rollDirection, rb.velocity.y);
        StartCoroutine(StopRoll());
        StartCoroutine(RollCoolDown());
        anm.SetTrigger("Roll");
        isRolling = true;
    }
    IEnumerator StopRoll()
    {
        yield return new WaitForSeconds(rollTime);
        isRolling = false;
    }
    IEnumerator RollCoolDown()
    {
        canRoll = false;
        yield return new WaitForSeconds(rollCooldown);
        canRoll = true;
    }

}
