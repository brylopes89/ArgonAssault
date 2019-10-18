using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flight : MonoBehaviour
{
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
       anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Submit"))
        {
            StartCoroutine(ObjectActive());
        }           
    }

    IEnumerator ObjectActive()
    {        
        anim.SetTrigger("LiftOff");
        anim.enabled = false;
        yield return new WaitForSeconds(4);
        
    }

}

