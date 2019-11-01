using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightTrigger : MonoBehaviour
{
    public Animator anim;
    public GameObject actual_Model;
    GameObject player_Rig;

    Rigidbody rb;
    Transform player_Trans;
    Transform core_Trans;
   

    public enum FlightState { None, Ground, Flight };
    public FlightState state { get; set; } = FlightState.Ground;

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

    public bool enabledGround = true;
    public bool enabledTakeoff = true;

    //meters/second
    public float maxGroundForwardSpeed = 100;
    //degrees/second
    public float groundDrag = 0;
    public float maxGroundTurningDegreesSecond = 40;

    public float standUpSpeed = 2.0f;
    float magSpeed;

    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        player_Rig = GameObject.FindGameObjectWithTag("Player");
        rb = player_Rig.GetComponent<Rigidbody>();

        player_Trans = player_Rig.GetComponent<Transform>();
        core_Trans = actual_Model.GetComponent<Transform>();
        magSpeed = player_Rig.GetComponent<PlayerFlightControl>().speed;

        rb.freezeRotation = true;
        rb.isKinematic = false;

        anim.enabled = true;

    }

    // Update is called once per frame
    void Update()
    {      
        if (isGrounded())
        {
            getGroundInputs();
        }
        else if (isFlying())
        {
            getGroundInputs();
        }
    }

    void FixedUpdate()
    {       
        if (isGrounded())
        {
            applyGroundInputs();
        }
        if (isFlying())
        {
            applyFlightInputs();
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
    }

    void getFlightInputs()
    {        
        _inputSubmit = Input.GetButton("Submit");
        _inputTakeoff = _inputSubmit;
    }

    void applyGroundInputs()
    {
        if (enabledGround)
        {
            groundMove();
        }

        if (enabledTakeoff)
        {
            StateChange(_inputTakeoff);
        }                
    }

    void applyFlightInputs()
    {       
        StateChange(_inputTakeoff);        
    }

    private void groundMove()
    {
        if (_inputGroundForward > 0f)
        {
           
            //anim.enabled = false;
            anim.SetBool("Movement", true);
            rb.AddForce(Vector3.forward * maxGroundForwardSpeed * _inputGroundForward * Time.deltaTime, ForceMode.VelocityChange);           
        }
        else
        {
           
            // anim.enabled = true;
            anim.SetBool("Movement", false);
        }

        float turningSpeed = maxGroundTurningDegreesSecond * _inputGroundTurning * Time.deltaTime;
        //rb.rotation *= Quaternion.AngleAxis(turningSpeed, Vector3.up);

        anim.SetFloat("ForwardSpeed", rb.velocity.magnitude);
        anim.SetFloat("AngularSpeed", turningSpeed);
        
    }

    void StateChange(bool triggerSet)
    {
        if(triggerSet == true)
        {
            if (isGrounded())
            {
                StartCoroutine(TakeOff());
                
            }

            else if (isFlying())
            {
                StartCoroutine(Land());                
            }
        }        
    }

    IEnumerator TakeOff()
    {
        anim.SetBool("IsFlying", true);
        state = FlightState.Flight;
        player_Rig.GetComponent<PlayerFlightControl>().targetSpeed = magSpeed;

        yield return new WaitForSeconds(5.1f);
        anim.enabled = false;
        rb.freezeRotation = false;   
        
    }

    IEnumerator Land()
    {        
        state = FlightState.Ground;
        anim.SetBool("IsFlying", false);
        rb.isKinematic = true;

        Quaternion desiredRotation = Quaternion.identity;

        desiredRotation.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);

        actual_Model.GetComponent<Transform>().localRotation = desiredRotation;       

        player_Trans.rotation = desiredRotation;

        yield return new WaitForFixedUpdate();

        anim.enabled = true;

    }
}
