using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightTrigger : MonoBehaviour
{
    public Animator anim;
    GameObject parent;
    Rigidbody player;

    public enum FlightState { None, Ground, Flight };
    private FlightState _state = FlightState.Ground;
    public FlightState state
    {
        get { return _state; }
        set { _state = value; }
    }

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
    public float groundDrag = 5;
    public float maxGroundTurningDegreesSecond = 40;

    // Start is called before the first frame update
    void Start()
    {
        parent = GameObject.FindGameObjectWithTag("Player");
        player = parent.GetComponent<Rigidbody>();        
    }

    // Update is called once per frame
    void Update()
    {
        /* if (Input.GetButton("Submit"))
         {
             StartCoroutine(TriggerActive());
         }*/
        if (isGrounded())
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
    }

    public bool isGrounded()
    {
        if (state == FlightState.Ground)
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

    void applyGroundInputs()
    {
        if (enabledGround)
        {
            groundMove();
        }


        if (enabledTakeoff)
        {
            TakeOff(_inputTakeoff);
        }                
    }

    private void groundMove()
    {
        player.drag = 5.0f;

        if (_inputGroundForward > 0f)
        {
            //anim.enabled = false;
            anim.SetBool("Movement", true);
            player.AddRelativeForce(Vector3.forward * maxGroundForwardSpeed * _inputGroundForward * Time.deltaTime, ForceMode.VelocityChange);
        }
        else
        {
           // anim.enabled = true;
            anim.SetBool("Movement", false);
        }

        float turningSpeed = maxGroundTurningDegreesSecond * _inputGroundTurning * Time.deltaTime;
        player.rotation *= Quaternion.AngleAxis(turningSpeed, Vector3.up);

        anim.SetFloat("ForwardSpeed", player.velocity.magnitude);
        anim.SetFloat("AngularSpeed", turningSpeed);

        //anim.transform.position = player.transform.position;

    }

    void TakeOff(bool triggerSet)
    {
        if(triggerSet == true)
        {
            state = FlightState.Flight;
            player.drag = 0;
            StartCoroutine(TriggerActive());
        }
    }

    IEnumerator TriggerActive()
    {
        anim.SetTrigger("LiftOff");       
        yield return new WaitForSeconds(5.1f);
        anim.enabled = false;
       
    }
}
