using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [Tooltip("In seconds")] [SerializeField] float levelLoadDelay = 2f;
    [Tooltip("FX pefab on player")] [SerializeField] GameObject deathFX;

    [SerializeField] private List<Animator> anims = new List<Animator>();

    const string animBaseLayer = "Base Layer";
    int liftAnimHash = Animator.StringToHash(animBaseLayer + ".LiftOff");

    private float targetSpeed = 6.0f;
    private float initSpeed;
    private float timer;
    private float currentSpeed;

    public float slowTransition = 0.5f;


    Transform core_Trans;

    bool setBool;

    private void Start()
    {
        core_Trans = GetComponent<PlayerFlightControl>().actual_model.transform;
        initSpeed = GetComponent<PlayerFlightControl>().speed;
    }

    void FixedUpdate()
    {
        if (setBool)
        {
            if (Input.GetButtonDown("Submit"))
            {
                anims[0].SetTrigger("SubmitTrigger");                
                anims[1].SetTrigger("KeyPress");
                anims[1].SetBool("IsKeyPressed", true);
            }
            else
            {
                anims[1].SetBool("IsKeyPressed", false);
            }

            if(anims[0].GetBool("IsLiftOff") == true)
            {
                GetComponent<PlayerFlightControl>().speed = initSpeed;
            }
        }        
    }

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

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Land"))
        {
            anims[0].SetBool("IsNearLandingPad", true);
            anims[1].SetBool("PlayText", true);
            setBool = true;            

            Quaternion playerRo = Quaternion.identity;
            Quaternion coreRo = Quaternion.identity;

            playerRo.eulerAngles = new Vector3(0.0f, transform.rotation.eulerAngles.y, 0.0f);
            coreRo.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);

            core_Trans.localRotation = coreRo;
            transform.rotation = Quaternion.Slerp(transform.rotation, playerRo, targetSpeed * Time.deltaTime);

            StartCoroutine(SlowSpeedApproach());            
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Land"))
        {
            anims[0].SetBool("IsNearLandingPad", false);
            anims[1].SetBool("PlayText", false);
            setBool = false;

            GetComponent<PlayerFlightControl>().speed = initSpeed;
        }
    }

    IEnumerator SlowSpeedApproach()
    {
        timer = 0.0f;

        currentSpeed = GetComponent<PlayerFlightControl>().currentMag;

        while (timer < slowTransition)
        {
            yield return new WaitForFixedUpdate();

            timer += Time.fixedDeltaTime;
            float ratio = timer / slowTransition;

            if (ratio > 1.0f)
            {
                break;
            }

            GetComponent<PlayerFlightControl>().speed = Mathf.SmoothStep(currentSpeed, targetSpeed, ratio);
        }

        GetComponent<PlayerFlightControl>().speed = targetSpeed;

        yield return null;
    }
}
