using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [Tooltip("In seconds")][SerializeField] float levelLoadDelay = 2f;
    [Tooltip("FX pefab on player")][SerializeField] GameObject deathFX;

   // [SerializeField] private List<Animator> anims = new List<Animator>();

    //public float targetSpeed;
    //float _speed;

   // Transform core_Trans;

    //bool setBool;

    private void Start()
    {
        /*core_Trans = GetComponent<PlayerFlightControl>().actual_model.transform;
        _speed = GetComponent<PlayerFlightControl>().speed;*/
    }

   /* void Update()
    {
        if (setBool)
        {
            if (Input.GetButtonDown("Submit"))
            {
                anims[0].SetTrigger("SubmitTrigger");
                anims[1].SetTrigger("KeyPress");
            }
        }
    }*/

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag != "Friendly")
        {
            StartDeathSequence();
            deathFX.SetActive(true);
            Invoke("ReloadScene", levelLoadDelay);
        }                     
    }

    void StartDeathSequence()
    {
        print("Wipe yourself off, you dead.");
        SendMessage("OnPlayerDeath");        
    }  

    void ReloadScene() // string referenced
    {
        SceneManager.LoadScene(1);
    }

    /*void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Land"))
        {
            anims[0].SetBool("isNearLandingPad", true);
            anims[1].SetBool("PlayText", true);
            setBool = true;

            /*targetSpeed = 6.0f;
            _speed = targetSpeed;

            Quaternion playerRo = Quaternion.identity;
            Quaternion coreRo = Quaternion.identity;

            playerRo.eulerAngles = new Vector3(0.0f, transform.rotation.eulerAngles.y, 0.0f);
            coreRo.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);

            core_Trans.localRotation = coreRo;

            transform.rotation = Quaternion.Slerp(transform.rotation, playerRo, targetSpeed * Time.deltaTime);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Land"))
        {
            anims[0].SetBool("isNearLandingPad", false);
            anims[1].SetBool("PlayText", false);
            setBool = false;

            //targetSpeed = GetComponent<PlayerFlightControl>().speed;
        }
    }*/
}
