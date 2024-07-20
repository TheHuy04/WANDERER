using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Dichchuyen : MonoBehaviour
{
    [SerializeField] GameObject Gate;

    
    void Start()
    {
        if(Gate != null)
        {
            transform.position = Gate.GetComponent<Gate>().GetDiemDichChuyen().position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
