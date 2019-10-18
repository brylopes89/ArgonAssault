using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightAnim : MonoBehaviour
{
   
    public enum FlightState { None, Ground, Flight };
    private FlightState _state = FlightState.Ground;
    public FlightState state;

    public Animator anim;
    public bool isActive;

    public bool enabledLaunchIfAirborn = true;
    public bool enabledDiving = false;
    public bool enabledTakeoff = true;
    public bool enabledLanding = true;
    public bool enabledCrashing = false;
    public float crashSpeed = 40f;

    //===========
    //USER INPUT FLIGHT
    //===========

    protected bool _inputTakeoff = false;
    protected bool _inputFlaring = false;
    protected bool _inputDiving = false;
    protected bool _inputFlap = false;

    public bool InputTakeoff { get { return _inputTakeoff; } }
    public bool InputDiving { get { return _inputDiving; } }

    // Start is called before the first frame update
    void Start()
    {
       anim = GetComponent<Animator>();
    }

    void Update()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetButton("Submit"))
        {           
            StartCoroutine(ObjectActive());            
        }           
    }

    IEnumerator ObjectActive()
    {        
        anim.SetTrigger("LiftOff");
        isActive = true;
        yield return new WaitForSeconds(5.1f);
        anim.enabled = false;
        isActive = false;       
    }
}



