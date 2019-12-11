using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [Tooltip("In seconds")] [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] private List<Animator> anims = new List<Animator>();
  
    private float targetSpeed = 6.0f;
    private float initSpeed;
    private float timer;
    private float currentSpeed;
    private float Health = 200f;
    private float _maxHealth;
    private bool setBool;
    private PlayerHealth currentHealth;

    [HideInInspector] public bool isHit = false;

    private Image _targetBar;
    private Transform core_Trans;
    

    public float triggerEnterSpeed = 0.5f;
    public float triggerLeaveSpeed = 1.0f;    

    private void Start()
    {
        core_Trans = GetComponent<PlayerFlightControl>().actual_model.transform;        
        initSpeed = GetComponent<PlayerFlightControl>().speed;
        currentHealth = GetComponent<PlayerHealth>();
    }

    void FixedUpdate()
    {
        if (setBool)
        {
            if (Input.GetButtonDown("Submit"))
            {                              
                anims[1].SetTrigger("KeyPress");                
            }            
        }        
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "Terrain" )
        {
            isHit = true;
            currentHealth.Health = 0;
            currentHealth.StartDeathSequence();                   
        }
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

            if (anims[0].GetBool("IsFlying") == true && anims[0].GetBool("IsLiftOff") == false)
            {
                StartCoroutine(SlowSpeedApproach());
            }         
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Land"))
        {
            anims[0].SetBool("IsNearLandingPad", false);
            anims[1].SetBool("PlayText", false);
            setBool = false;

            StartCoroutine(SlowSpeedLeave());
            //GetComponent<PlayerFlightControl>().speed = initSpeed;
        }
    }

    IEnumerator SlowSpeedApproach()
    {
        timer = 0.0f;

        currentSpeed = GetComponent<PlayerFlightControl>().speed;

        Quaternion currentPlayerRo = transform.rotation;
        Quaternion currentCoreRo = core_Trans.localRotation;

        Quaternion targetPlayerRo = Quaternion.identity;
        Quaternion targetCoreRo = Quaternion.identity;

        targetPlayerRo.eulerAngles = new Vector3(0.0f, transform.rotation.eulerAngles.y, 0.0f);
        targetCoreRo.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
         
        while (timer < triggerEnterSpeed)
        {
            yield return new WaitForFixedUpdate();

            timer += Time.fixedDeltaTime;
            float ratio = timer / triggerEnterSpeed;

            if (ratio > 1.0f)//reaches %100
            {
                break;
            }

            GetComponent<PlayerFlightControl>().speed = Mathf.SmoothStep(currentSpeed, targetSpeed, ratio);
            core_Trans.localRotation = Quaternion.Slerp(currentCoreRo, targetCoreRo, ratio);
            transform.rotation = Quaternion.Slerp(currentPlayerRo, targetPlayerRo, ratio);
        }

        yield return new WaitForSeconds(5.0f);        
        //yield return new WaitForEndOfFrame();
        GetComponent<PlayerFlightControl>().speed = initSpeed;
    }

    IEnumerator SlowSpeedLeave()
    {
        currentSpeed = GetComponent<PlayerFlightControl>().speed;

        timer = 0.0f;

        while (timer < triggerLeaveSpeed)
        {
            yield return new WaitForFixedUpdate();

            timer += Time.fixedDeltaTime;
            float ratio = timer / triggerLeaveSpeed;

            if (ratio > 1.0f)//reaches %100
            {
                break;
            }
           GetComponent<PlayerFlightControl>().speed = Mathf.SmoothStep(currentSpeed, initSpeed, ratio);           
        }       

       yield return new WaitForEndOfFrame();

        //GetComponent<PlayerFlightControl>().speed = initSpeed;
    }
}
