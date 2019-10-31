using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightTrigger : MonoBehaviour
{
    public Animator anim;   
    GameObject child;
    Rigidbody rb;    

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
    public float maxGroundForwardSpeed = 40;
    //degrees/second
    public float groundDrag = 0;
    public float maxGroundTurningDegreesSecond = 40;

    void Awake()
    {       
        
    }

    // Start is called before the first frame update
    void Start()
    {
        child = GameObject.FindGameObjectWithTag("Player");
        rb = child.GetComponent<Rigidbody>();             
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
            rb.AddRelativeForce(Vector3.forward * maxGroundForwardSpeed * _inputGroundForward * Time.deltaTime, ForceMode.VelocityChange);
        }
        else
        {
           // anim.enabled = true;
            anim.SetBool("Movement", false);
        }

        float turningSpeed = maxGroundTurningDegreesSecond * _inputGroundTurning * Time.deltaTime;
        rb.rotation *= Quaternion.AngleAxis(turningSpeed, Vector3.up);

        anim.SetFloat("ForwardSpeed", rb.velocity.magnitude);
        anim.SetFloat("AngularSpeed", turningSpeed);

        //anim.transform.position = player.transform.position;
    }

    void StateChange(bool triggerSet)
    {
        if(triggerSet == true)
        {
            if (!isFlying())
            {
                StartCoroutine(TakeOff());
            }

            if (isFlying())
            {
                StartCoroutine(Land());
            }
        }        
    }

    IEnumerator TakeOff()
    {
        anim.SetBool("IsFlying", true);
        state = FlightState.Flight;
        yield return new WaitForSeconds(5.1f);      
        anim.enabled = false;       
    }

    IEnumerator Land()
    {
        anim.SetBool("IsFlying", false);
        anim.enabled = true;
        state = FlightState.Ground;
        yield return new WaitForSeconds(5.1f);
        

    }
}
