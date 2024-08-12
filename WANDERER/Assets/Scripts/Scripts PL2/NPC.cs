using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class NPC : MonoBehaviour
{
    public GameObject Panel;
    public Text dialogueText;
    public string[] dialogue;
    private int index;
    public float wordSpeed;
    public bool playerIsClose;

    public GameObject contiButton;

    public Transform player;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && playerIsClose)
        {
            if(Panel.activeInHierarchy)
            {
                zeroText();
            }
            else
            {
                Panel.SetActive(true);
                StartCoroutine(Typing());
            }
        }
        if(dialogueText.text == dialogue[index])
        {
            contiButton.SetActive(true);
        }

        if (player != null)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0;
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);  // Hướng mặt phải
            }
            else if (direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);  // Hướng mặt trái
            }
        }
    }
    

    public void zeroText()
    {
        dialogueText.text = "";
        index = 0;
        Panel.SetActive(false);
    }

    IEnumerator Typing()
    {
        foreach(char letter in dialogue[index])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine()
    {

        contiButton.SetActive(false);
        if(index < dialogue.Length -1)
        {
            index ++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            zeroText();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsClose = false;
            zeroText();
        }
    }
}
