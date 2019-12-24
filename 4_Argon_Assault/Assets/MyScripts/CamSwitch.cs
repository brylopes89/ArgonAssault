using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CamSwitch : MonoBehaviour
{
    public GameObject cam1;
    public GameObject cam2;
    public GameObject cam3;

    public bool toggle = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }   

    // Update is called once per frame
    void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("CamSwitch"))
        {
            toggle = !toggle;
            
            cam1.SetActive(toggle);
            cam2.SetActive(!toggle);                
        }

        if (CrossPlatformInputManager.GetButton("Toggle"))
        {
            cam3.SetActive(true);

            //cam1.GetComponent<AudioListener>().enabled = false;
            //cam2.GetComponent<AudioListener>().enabled = false;

            cam1.GetComponent<Camera>().enabled = false;
            cam2.GetComponent<Camera>().enabled = false;
        }
        else
        {
            cam3.SetActive(false);

            //cam1.GetComponent<AudioListener>().enabled = true;
            //cam2.GetComponent<AudioListener>().enabled = true;

            cam1.GetComponent<Camera>().enabled = true;
            cam2.GetComponent<Camera>().enabled = true;
        }
       
    }
}
