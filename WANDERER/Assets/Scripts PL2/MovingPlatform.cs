using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public int speed;
    public Transform diemBatDau;
    public Transform diemKetThuc;
    public Vector2 diemMucTieu;
    void Start()
    {
        diemMucTieu = diemBatDau.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position,diemBatDau.position) <0.1)
        {
            diemMucTieu = diemKetThuc.position;
        }
        if(Vector2.Distance(transform.position,diemKetThuc.position) <0.1)
        {
            diemMucTieu = diemBatDau.position;
        }
        transform.position = Vector2.MoveTowards(transform.position, diemMucTieu, speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
