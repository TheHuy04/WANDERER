using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    public float speed;
    public float Jump;
    private float h_move;
    private bool nhay1L;
    private bool isFacingRight;

    private Animator anm;
    private Rigidbody2D rb;
    

    void Start()
    {
       rb = GetComponent<Rigidbody2D>();
       anm = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        h_move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(h_move * speed, rb.velocity.y);
        anm.SetFloat("Run", Mathf.Abs(h_move));

        if(Input.GetKeyDown(KeyCode.Space) && nhay1L)
        {
            rb.AddForce(Vector2.up * Jump, ForceMode2D.Impulse);
            nhay1L = false;
            anm.SetBool("Jump", true);
        }
        Flip();
        
    }
    void Flip()
    {
        if(isFacingRight && h_move > 0 || !isFacingRight && h_move < 0)
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
}
