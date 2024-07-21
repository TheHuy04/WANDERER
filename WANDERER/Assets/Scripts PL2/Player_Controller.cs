using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anm = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        h_Move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(h_Move *speed, rb.velocity.y);

        if(Input.GetKeyDown(KeyCode.Space) && nhay1L)
        {
            rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
            nhay1L = false;
        }
        Flip();
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
        }
    }

}
