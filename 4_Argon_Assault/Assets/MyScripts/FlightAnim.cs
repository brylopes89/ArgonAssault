using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightAnim : MonoBehaviour
{

    public enum FlightState { None, Ground, Flight };
    private FlightState _state = FlightState.Ground;   

    public FlightState state
    {
        get { return _state; }
        set { _state = value; }
    }

    public Animator anim;
    public bool isActive;

    [Range(-1.0f, 1.0f)]
    protected float _inputGroundForward;
    [Range(-1.0f, 1.0f)]
    protected float _inputGroundTurning;

    protected bool _inputSubmit;
    public bool InputSubmit { get { return _inputSubmit; } }

    protected bool _inputTakeoff = false;
    public bool InputTakeoff { get { return _inputTakeoff; } }

    public float InputGroundForward { get { return _inputGroundForward; } }
    public float InputGroundTurning { get { return _inputGroundTurning; } }

    public bool enabledTakeoff = true;
    public AudioClip takeoffSoundClip;
    private AudioSource takeoffSoundSource;

    public bool enabledLaunchIfAirborn = true;
    public float minHeightToLaunchIfAirborn = 2f;

    //meters/second
    public float maxGroundForwardSpeed = 40;
    //degrees/second
    public float groundDrag = 1.0f;
    public float maxGroundTurningDegreesSecond = 40;

    void Awake()
    {       
        //GetComponent<Rigidbody>().freezeRotation = true;
        //GetComponent<Rigidbody>().isKinematic = false;
    }


    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isGrounded())
        {
            getGroundInputs();
        }       
    }   

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isGrounded())
        {
            applyGroundInputs();
        }
        else if (isFlying())
        {
            GetComponentInChildren<Rigidbody>().drag = 0;
        } 
    }

    public bool isGrounded()
    {
        if (state == FlightState.Ground)
            return true;
        return false;
    }

    public bool isFlying()
    {
        if (state == FlightState.Flight)
            return true;
        return false;
    }

    void getGroundInputs()
    {

        _inputGroundForward = Input.GetAxis("Vertical");
        _inputGroundTurning = Input.GetAxis("Horizontal");
        _inputSubmit = Input.GetButton("Submit");
        _inputTakeoff = _inputSubmit;
        //anim.enabled = false;
    }

    void applyGroundInputs()
    {
        if (_inputSubmit)
        {
            Submit();
        }

        groundMove();
        //launchIfAirborn(minHeightToLaunchIfAirborn);
    }

    void Submit()
    {
        state = FlightState.Flight;
        StartCoroutine(ObjectActive());
        anim.enabled = true;
        
    }

    private void groundMove()
    {
        GetComponentInChildren<Rigidbody>().drag = 1.0f;
        if (_inputGroundForward > 0f)
        {           
            GetComponentInChildren<Rigidbody>().AddRelativeForce(Vector3.forward * maxGroundForwardSpeed * _inputGroundForward * Time.deltaTime, ForceMode.VelocityChange);
        }
     
        float turningSpeed = maxGroundTurningDegreesSecond * _inputGroundTurning * Time.deltaTime;
        GetComponentInChildren<Rigidbody>().rotation *= Quaternion.AngleAxis(turningSpeed, Vector3.up);
    }

    private void launchIfAirborn(float minHeight)
    {
        if (!Physics.Raycast(transform.position, Vector3.down, minHeight))
        {
            takeoff();
        }
    }

    protected void takeoff()
    {
        if (!isFlying())
        {
            state = FlightState.Flight;            
            //GetComponent<Rigidbody>().isKinematic = false;            
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
