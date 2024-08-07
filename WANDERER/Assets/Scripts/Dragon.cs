using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float speed = 2f;

    private Vector3 start;
    private Vector3 end;
    private bool movingToEnd = true;

    void Start()
    {
        // Lưu vị trí bắt đầu và kết thúc
        start = startPoint.position;
        end = endPoint.position;
    }

    void Update()
    {
        // Di chuyển dragon từ điểm bắt đầu đến điểm kết thúc và ngược lại
        if (movingToEnd)
        {
            transform.position = Vector3.MoveTowards(transform.position, end, speed * Time.deltaTime);
            if (transform.position == end)
            {
                movingToEnd = false;
                Flip();
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, start, speed * Time.deltaTime);
            if (transform.position == start)
            {
                movingToEnd = true;
                Flip();
            }
        }
    }

    void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
