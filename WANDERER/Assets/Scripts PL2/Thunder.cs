using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    public float timeThunder;
    private Animator anm;
    void Start()
    {
        anm = GetComponent<Animator>();
        StartCoroutine(StarThunder());
    }
    IEnumerator StarThunder()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeThunder);
            anm.SetTrigger("Thunder");  
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
