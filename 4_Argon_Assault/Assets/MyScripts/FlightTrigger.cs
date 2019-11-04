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

    Vector3 groundPos;
    Quaternion groundRot;

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

    public float landingTime = 5.0f;
    private float timer = 0.0f;

    bool isTransitioning = false;

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

        groundPos = player_Rig.transform.position;            
        groundRot = player_Rig.transform.rotation;

       // rb.freezeRotation = true;
        //rb.isKinematic = false;
        //anim.enabled = true;

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
            getFlightInputs();
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
            else if(isFlying() && anim.GetBool("IsNearLandingPad") == true)
            {
                StartCoroutine(Land());
            }
        }        
    }         

    IEnumerator TakeOff()
    {
        if (isTransitioning)
        {
            yield break;
        }

        Debug.Log("takeoff");        
        state = FlightState.Flight;
        anim.SetBool("IsFlying", true);
        anim.SetBool("IsLiftOff", true);

        isTransitioning = true;        
        anim.SetTrigger("TakeOffTrigger");         

        yield return new WaitForSeconds(5.0f);
        anim.enabled = false;
        rb.freezeRotation = false;
        isTransitioning = false;
        anim.SetBool("IsLiftOff", false);

    }

    IEnumerator Land()
    {
        if (isTransitioning)
        {
            yield break;
        }

        Debug.Log("land");
        
        state = FlightState.Ground;
        isTransitioning = true;

        anim.SetTrigger("LandTrigger");        
        anim.enabled = true;

        //rb.isKinematic = true;       

        Vector3 currentPos = player_Rig.transform.position;
        Quaternion currentRot = player_Rig.transform.rotation;

        Quaternion desiredRotation = Quaternion.identity;
        //Vector3 newPos = new Vector3(0.0f, 0.0f, 0.0f);
        desiredRotation.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);

        timer = 0.0f;

        while(timer < landingTime)
        {
            yield return new WaitForFixedUpdate();

            timer += Time.fixedDeltaTime;
            float ratio = timer / landingTime;

            if(ratio > 1.0f)
            {
                break;
            }            

            actual_Model.GetComponent<Transform>().localRotation = desiredRotation;

            player_Rig.transform.rotation = Quaternion.Slerp(currentRot, groundRot, ratio);

            player_Rig.transform.position = Vector3.Lerp(currentPos, groundPos, ratio);
        }

        //yield return new WaitForFixedUpdate();        
       
        //Vector3 newPos = new Vector3(0.0f, 0.0f, 0.0f);
        desiredRotation.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);

        actual_Model.GetComponent<Transform>().localRotation = desiredRotation;

        player_Rig.transform.rotation = groundRot;
        player_Rig.transform.position = groundPos;
        

        //yield return new WaitForSeconds(3.0f);
        yield return new WaitForFixedUpdate();

        isTransitioning = false;
        anim.SetBool("IsFlying", false);      
    }
}
