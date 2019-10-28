using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSwitch : MonoBehaviour
{
    public GameObject cam1;
    public GameObject cam2;  

    bool toggle = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }   

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Toggle"))
        {
            toggle = !toggle;
            
            cam1.SetActive(toggle);
            cam2.SetActive(!toggle);           
            
        }      
       
    }
}
