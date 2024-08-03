using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    private bool menuActivated;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab) && menuActivated)
        {
            InventoryMenu.SetActive(false);
            menuActivated = false;

        }
        else if (Input.GetKeyDown(KeyCode.Tab) && !menuActivated)
        {
            InventoryMenu.SetActive(true);
            menuActivated = true;

        }
    }
}
