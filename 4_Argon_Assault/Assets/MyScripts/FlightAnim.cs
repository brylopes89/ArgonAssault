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
    public float groundDrag = 1;
    public float maxGroundTurningDegreesSecond = 40;

    void Awake()
    {
        setupSound(takeoffSoundClip, ref takeoffSoundSource);
        //setupSound(landingSoundClip, ref landingSoundSource);
       
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
            GetComponent<Rigidbody>().drag = 0;
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
        StartCoroutine(ObjectActive());
        state = FlightState.Flight;
    }

    private void groundMove()
    {
        GetComponent<Rigidbody>().drag = 0;
        if (_inputGroundForward > 0f)
        {           
            GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * maxGroundForwardSpeed * _inputGroundForward * Time.deltaTime, ForceMode.VelocityChange);
        }
     
        float turningSpeed = maxGroundTurningDegreesSecond * _inputGroundTurning * Time.deltaTime;
        GetComponent<Rigidbody>().rotation *= Quaternion.AngleAxis(turningSpeed, Vector3.up);
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
            GetComponent<Rigidbody>().isKinematic = false;
            playSound(takeoffSoundSource);
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

    protected AudioSource setupSound(AudioClip sound, ref AudioSource source)
    {

        if (!sound && source)
            Destroy(source);

        if (!sound && !source)
            return null;

        if (sound && !source)
        {
            source = gameObject.AddComponent<AudioSource>();
            source.loop = false;
        }

        if (!source.clip)
        {
            source.clip = sound;
        }

        return source;
    }

    protected void playSound(AudioSource source)
    {
        if (source)
        {
            source.Play();
        }

    }

}
