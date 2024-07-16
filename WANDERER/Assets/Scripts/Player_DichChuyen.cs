using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

public class Player_DichChuyen : MonoBehaviour
{
    [SerializeField] GameObject Gate;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Gate != null)
        {
            transform.position = Gate.GetComponent<Gate>().getDiemDichChuyen().position;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Gate"))
        {
            Gate = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Gate"))
        {
            Gate = null;
        }
    }
}
