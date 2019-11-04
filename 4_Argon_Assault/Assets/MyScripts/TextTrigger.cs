using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTrigger : MonoBehaviour
{   
    [SerializeField] private List<Animator> anims = new List<Animator>();

    bool setBool;

    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (setBool)
        {
            if (Input.GetButtonDown("Submit"))
            {
                anims[0].SetTrigger("SubmitTrigger");
                anims[1].SetTrigger("KeyPress");
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            anims[0].SetBool("isNearLandingPad", true);
            anims[1].SetBool("PlayText", true);
            setBool = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            anims[0].SetBool("isNearLandingPad", false);
            anims[1].SetBool("PlayText", false);
            setBool = false;
        }
    }
}